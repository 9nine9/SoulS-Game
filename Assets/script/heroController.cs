using UnityEngine;
using UnityEngine.UI;

public class heroController : MonoBehaviour {
	public float speedStart;		//kecepatan hero
	public float speed;
	public bool isMove;
	float rotate;
	float swipeDistance = 50f;

	public int enemyNoticeDistance;

	public float redDuration;		//durasi terkena red soul
	public float yellowDuration;	//durasi terkena yellow soul

	public float lightTimeMax;		//max durasi light
	public float lightTime;			//durasi light yang tersisa
	public float lightTimeAdd;		//durasi tambahan light

	public Light heroLight;
	public RectTransform lightBar;
	public GameObject map;
	public Text scoreDisplay;

	tileMap node;
	spawnSoul spawn;
	scoreManager score;
	gameOver status;
	Vector2 world;
	Vector2 direct;
	Animator anim;

	Vector2 firstFinger;
	Vector2 lastFinger;

	void Error () {
		//error
		if (!map) Debug.LogError ("map is null (heroController)");
		if (!lightBar) Debug.LogError ("lightBar is null (heroController)");
		if (!heroLight) Debug.LogError ("heroLight is null (heroController)");
		if (!scoreDisplay) Debug.LogError ("scoreDisplay is null (heroController)");
	}

	void Start (){
		Error ();
		if (map){
			node 	= map.GetComponent<tileMap> ();
			score 	= map.GetComponent<scoreManager> ();
			spawn 	= map.GetComponent<spawnSoul> ();
			status	= map.GetComponent<gameOver> ();

			if (!node) Debug.LogError ("node (map) is null (heroController)");
			if (!score) Debug.LogError ("score (map) is null (heroController)");
			if (!spawn) Debug.LogError ("spawn (map) is null (heroController)");
			if (!status) Debug.LogError ("status (map) is null (heroController)");
		}

		if (scoreDisplay) {
			scoreDisplay.text = "0";
		}

		anim = gameObject.GetComponent<Animator> ();
		if (!anim) Debug.LogError ("anim (Animator) is null (heroController)");

		isMove 		= false;
		world.x 	= Screen.width / 2;
		world.y 	= Screen.height / 2;
		speed 		= speedStart;
		rotate 		= transform.eulerAngles.z;

		if (rotate == 0f) {
			direct = Vector2.up;
		}
		else if (rotate == 90f) {
			direct = Vector2.left;
		}
		else if (rotate == 180f) {
			direct = Vector2.down;
		}
		else if (rotate == 270f) {
			direct = Vector2.right;
		}

	}

	void Update () {
		Movement ();

		if (node) {
			ChangeNode ();
			EnemyNotice ();
		}

		LightStatus ();	
	}

	//pergerakan hero dengan mengklik/touch layar
	void Movement () {

		//click control
		if (Input.GetMouseButtonDown (0)) {
			Vector2 pos;
			Vector2 click = Input.mousePosition;

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

		//swipe control
		/*if (Input.touchCount > 0) {
			foreach (Touch touch in Input.touches) {
				if (touch.phase == TouchPhase.Began) {
					firstFinger = touch.position;
					lastFinger = touch.position;
				}
				if (touch.phase == TouchPhase.Moved) {
					lastFinger = touch.position;
				}
				if (touch.phase == TouchPhase.Ended) {
					if ((firstFinger.x - lastFinger.x) > swipeDistance) { //left swipe
						rotate = 90f;
						direct = Vector2.left;
					} else if ((firstFinger.x - lastFinger.x) < (-1 * swipeDistance)) { //right swipe
						rotate = 270f;
						direct = Vector2.right;
					} else if ((firstFinger.y - lastFinger.y) > swipeDistance) { //down swipe
						rotate = 180f;
						direct = Vector2.down;
					} else if ((firstFinger.y - lastFinger.y) < (-1 * swipeDistance)) { //up swipe
						rotate = 0f;
						direct = Vector2.up;
					}
				}
			}
		}
*/
		RaycastHit2D[] hits = Physics2D.RaycastAll (transform.position, direct, (node.tileSize / 2) + 0.1f);
		for (int i = 0; i < hits.Length; i++) {
			RaycastHit2D hit = hits [i];
			if (hit.collider.gameObject.tag == "Wall") { //jika di depan ada tembok, berhenti
				isMove = false;
				break;
			}
		}

		transform.eulerAngles = new Vector3(0, 0, rotate);
		if (isMove && !spawn.isYellowSoul) {
			anim.SetBool ("isMove", true);
			transform.Translate (Vector2.up * speed * Time.deltaTime);
		}
		else {
			anim.SetBool ("isMove", false);
			isMove = true;
		}
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
			
		if (node.nodeOpen.Count > 0) {
			//jika current node nya hero belum dilalui, hapus node tersebut dari list nodeOpen 
			if (node.nodeOpen.Contains (node.hero)) {
				node.nodeOpen.Remove (node.hero);
			}
		} 
		else spawn.isExplorePath = true;
	}

	//status light hero
	void LightStatus () {
		//jika waktu light habis  maka game over
		if (lightTime <= 0) {
			status.isGameOver = true;
		}
		else {
			if (lightBar) {
				float timeDown = lightTime / lightTimeMax;
				lightBar.localScale = new Vector2(1, timeDown);
				lightTime -= Time.deltaTime;
			}
		}
	}

	void EnemyNotice () {
		int distance = Mathf.Abs (node.hero.x - node.enemy.x) + Mathf.Abs (node.hero.y - node.enemy.y);
		if (distance < enemyNoticeDistance) {
			print ("enemy Closed");
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
			if (node && score && scoreDisplay) {
				spawn.Explode (other.gameObject.transform.position, new Color (0, 255, 255)); //buat partikel blue
				score.blueSoulScore++;		//update score blue soul
				scoreDisplay.text = score.blueSoulScore.ToString();
			}

			Destroy (other.gameObject);
			spawn.blueSoulCount--;			//update jumlah blue soul
			node.nodeSpawn.Add (node.hero);	//masukkan node blue soul ke list nodeSpawn

			//tambah durasi light yang terisa
			lightTime += lightTimeAdd;
			if (lightTime > lightTimeMax) {
				lightTime = lightTimeMax;
			}
		}

		//jika menabrak red soul
		if (other.gameObject.tag == "RedSoul") {
			if (node && score) {
				spawn.Explode (other.gameObject.transform.position, new Color (255, 0, 0)); //buat partikel red
				score.redSoulScore++;		//update score red soul
			}
	
			Destroy (other.gameObject);
			StartCoroutine (spawn.RedEffect (redDuration));
		}

		//jika menabrak yellow soul
		if (other.gameObject.tag == "YellowSoul") {
			if (node && score) {
				spawn.Explode (other.gameObject.transform.position, new Color (255, 255, 0)); //buat partikel yellow
				score.yellowSoulScore++;		//update score yellow soul
			}

			Destroy (other.gameObject);
			StartCoroutine (spawn.YellowEffect (yellowDuration, 5));
		}

	}

}
