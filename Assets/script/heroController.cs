using UnityEngine;
using UnityEngine.UI;

public class heroController : MonoBehaviour {
	public float speedStart;		//kecepatan hero
	public float speed;
	public bool isMove;
	float rotate;

	public float redDuration;		//durasi terkena red soul
	public float yellowDuration;	//durasi terkena yellow soul

	public Light heroLight;
	public RectTransform lightBar;
	public float lightTimeMax;		//max durasi light
	public float lightTime;			//durasi light yang tersisa
	public float lightTimeAdd;		//durasi tambahan light

	public GameObject map;
	tileMap node;
	spawnSoul spawn;
	scoreManager score;
	gameOver status;

	public Text scoreDisplay;
	public chapterManager chapter;

	public GameObject teleport;
	public float teleportTime;
	public float teleportCurrentTime;
	Image imgTeleport;
	Button btnTeleport;
	public bool isTeleport;

	public GameObject explore;
	Image imgExplore;
	int maxOpenPath;
	public float explorePercentage;

	Vector2 world;
	Vector2 direct;
	Animator anim;

	public int enemyNoticeDistance;
	public AudioClip enemyClosedSound;
	AudioSource audioSource;

	void Error () {
		if (!map) Debug.LogError ("map is null (heroController)");
		if (!lightBar) Debug.LogError ("lightBar is null (heroController)");
		if (!heroLight) Debug.LogError ("heroLight is null (heroController)");
		if (!scoreDisplay) Debug.LogError ("scoreDisplay is null (heroController)");
		if (!teleport) Debug.LogError ("teleport is null (heroController)");
		if (!enemyClosedSound) Debug.LogError ("enemyClosedSound is null (heroController)");
		if (!chapter) Debug.LogError ("chapter is null (heroController)");
		if (!explore) Debug.LogError ("explore is null (heroController)");
	}

	void Start (){
		Error ();
		if (map){
			node 	= map.GetComponent<tileMap> ();
			score 	= map.GetComponent<scoreManager> ();
			spawn 	= map.GetComponent<spawnSoul> ();
			status	= map.GetComponent<gameOver> ();

			if (!score) Debug.LogError ("score (map) is null (heroController)");
			if (!status) Debug.LogError ("status (map) is null (heroController)");
			if (!spawn) Debug.LogError ("spawn (map) is null (heroController)");
			if (!node) Debug.LogError ("node (map) is null (heroController)");
			else ChangeNode ();
		}

		if (teleport) {
			isTeleport	= true;
			imgTeleport = teleport.GetComponent<Image> ();
			btnTeleport = teleport.GetComponent<Button> ();

			if (!imgTeleport) Debug.LogError ("imgTeleport (teleport) is null (heroController)");
			else imgTeleport.fillAmount = 0f;
			if (!btnTeleport) Debug.LogError ("btnTeleport (teleport) is null (heroController)");
			else btnTeleport.interactable = false;
		}
			
		if (explore) {
			if (score.isExplore == 1) Destroy (explore);
			else {
				imgExplore = explore.GetComponent<Image> ();
				if (!imgExplore) Debug.LogError ("imgExplore (explore) is null (heroController)");
				else imgExplore.fillAmount = 1f;
			}
		}

		if (scoreDisplay) {
			scoreDisplay.text = "0";
		}

		anim = GetComponent<Animator> ();
		audioSource = GetComponent<AudioSource> ();
		if (!anim) Debug.LogError ("anim (Animator) is null (heroController)");
		if (!audioSource) Debug.LogError ("audioSource (AudioSource) is null (heroController)");

		isMove 		= false;
		world.x 	= Screen.width / 2;
		world.y 	= Screen.height / 2;
		speed 		= speedStart;
		rotate 		= transform.eulerAngles.z;
		maxOpenPath = node.nodeOpen.Count;

		if (rotate == 0f) direct = Vector2.up;
		else if (rotate == 90f) direct = Vector2.left;
		else if (rotate == 180f) direct = Vector2.down;
		else if (rotate == 270f) direct = Vector2.right;
	}

	void Update () {
		if (node && spawn && teleport && score) {
			Movement ();
			ChangeNode ();
			EnemyNotice ();

			if (!spawn.isYellowSoul)
				LightStatus ();

			if (isTeleport)
				TeleportReload ();
		
			if (score.isExplore == 0)
				ExplorePath ();
		}
	}

	//pergerakan hero dengan mengklik/touch layar
	void Movement () {
		//click control
		if (Input.GetMouseButtonDown (0)) {
			Vector2 click = Input.mousePosition;
			Vector2 pos;
			pos.x = Mathf.Abs(world.x - click.x);
			pos.y = Mathf.Abs(world.y - click.y);

			if (pos.x >= pos.y) {
				if (click.x >= world.x) { //right
					rotate = 270f;
					direct = Vector2.right;
				}
				else if (click.x < world.x) { //left
					rotate = 90f;
					direct = Vector2.left;
				}	
			} 
			else {
				if (click.y >= world.y) { //up
					rotate = 0f;
					direct = Vector2.up;
				}
				else if (click.y < world.y) { //down
					rotate = 180f;
					direct = Vector2.down;
				}
			}
		}
			
		RaycastHit2D[] hits = Physics2D.RaycastAll (transform.position, direct, (node.tileSize / 2) + 0.1f);
		isMove = (hits.Length > 1 && (hits [1].collider.gameObject.tag == "Wall")) ? false : true;

		transform.eulerAngles = new Vector3(0, 0, rotate);
		if (isMove && !spawn.isYellowSoul) {
			anim.SetBool ("isMove", true);
			transform.Translate (Vector2.up * speed * Time.deltaTime);
		}
		else anim.SetBool ("isMove", false);
	}
		
	//node yang ditempati hero
	void ChangeNode () {
		int x = (int) Mathf.Round (transform.position.x / node.tileSize);
		int y = (int) Mathf.Round (transform.position.y / node.tileSize);

		//jika node tidak melewati batas map, maka update
		if (x >= 0 && x < node.mapWidth && y >= 0 && y < node.mapHeight) {
			node.hero.x = x;
			node.hero.y = y;
		}
	}

	//status light hero
	void LightStatus () {
		//jika waktu light habis  maka game over
		if (lightTime <= 0 && status) status.isGameOver = true;
		else if (lightBar) {
			float timeDown = lightTime / lightTimeMax;
			lightBar.localScale = new Vector2 (1, timeDown);
			lightTime -= Time.deltaTime;
		}
	}

	void EnemyNotice () {
		int distance = Mathf.Abs (node.hero.x - node.enemy.x) + Mathf.Abs (node.hero.y - node.enemy.y);
		if (distance < enemyNoticeDistance && !audioSource.isPlaying) {
			audioSource.PlayOneShot (enemyClosedSound, 1f);
		}
	}

	void OnCollisionEnter2D (Collision2D other) {
		//jika menabrak musuh
		if (other.gameObject.tag == "Enemy") {
			lightTime  = 0;
			if (lightBar) lightBar.localScale = new Vector2(1, 0);
		}

		//jika menabrak blue soul
		if (other.gameObject.tag == "BlueSoul") {
			if (node && score && scoreDisplay && spawn) {
				spawn.Explode (other.gameObject.transform.position, new Color (0, 255, 255)); //buat partikel blue
				score.blueSoulScore++;		//update score blue soul
				scoreDisplay.text = score.blueSoulScore.ToString ();

				Destroy (other.gameObject);
				spawn.blueSoulCount--;			//update jumlah blue soul
				node.nodeSpawn.Add (node.hero);	//masukkan node blue soul ke list nodeSpawn

				//tambah durasi light yang terisa
				lightTime += lightTimeAdd;
				if (lightTime > lightTimeMax) {
					lightTime = lightTimeMax;
				}
			}
		}

		//jika menabrak red soul
		if (other.gameObject.tag == "RedSoul") {
			if (score && spawn) {
				spawn.Explode (other.gameObject.transform.position, new Color (255, 0, 0)); //buat partikel red
				score.redSoulScore++;		//update score red soul
				spawn.currentRedSoul++;

				spawn.StartShake (3f);
				Destroy (other.gameObject);
				StartCoroutine (spawn.RedEffect (redDuration));
			}
		}

		//jika menabrak yellow soul
		if (other.gameObject.tag == "YellowSoul") {
			if (chapter && score) {
				spawn.Explode (other.gameObject.transform.position, new Color (255, 255, 0)); //buat partikel yellow
				score.yellowSoulScore++;		//update score yellow soul

				Handheld.Vibrate ();
				Destroy (other.gameObject);
				StartCoroutine (spawn.YellowEffect (yellowDuration, 5));

				switch (score.yellowSoulScore) {
				case 1:
					chapter.Chapter1 ();
					break;
				case 2:
					chapter.Chapter2 ();
					break;
				case 3:
					chapter.Chapter3 ();
					break;
				case 4:
					chapter.Chapter4 ();
					break;
				case 5:
					chapter.Chapter5 ();
					break;
				}
			}
		}

	}

	public void Teleport () {
		if (!spawn.isYellowSoul && !status.isGameOver) {
			btnTeleport.interactable = false;
			imgTeleport.fillAmount = 0f;
			isTeleport = true;

			int spawnLocation = Random.Range (0, (node.nodeSpawn.Count - 1));
			Vector2 newLocation = node.map [node.nodeSpawn [spawnLocation].x, node.nodeSpawn [spawnLocation].y].position;
			transform.position = newLocation;
		}
	}

	void TeleportReload () {
		if (imgTeleport.fillAmount == 1f) {
			btnTeleport.interactable = true;
			isTeleport = false;
			teleportCurrentTime = 0f;
		}
		else {
			imgTeleport.fillAmount = teleportCurrentTime / teleportTime;
			teleportCurrentTime += Time.deltaTime;
		}
	}

	void ExplorePath () {
		float percentage = (float) node.nodeOpen.Count / maxOpenPath;
		if (percentage >= 1 - (explorePercentage / 100)) {
			//jika current node nya hero belum dilalui, hapus node tersebut dari list nodeOpen 
			if (node.nodeOpen.Contains (node.hero)) {
				node.nodeOpen.Remove (node.hero);
				imgExplore.color = Color.white;
				imgExplore.fillAmount = percentage;
			}
			else imgExplore.color = Color.grey;
		}
		else {
			spawn.isExplorePath = true;
			score.isExplore = 1;
			if (imgExplore) Destroy (explore);
		}
	}
}
