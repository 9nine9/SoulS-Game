using UnityEngine;
using System.Collections;

public class heroController : MonoBehaviour {
	public float speedStart;		//kecepatan hero
	public float redDuration;		//durasi terkena red soul
	public float yellowDuration;	//durasi terkena yellow soul
	public float lightTimeMax;		//max durasi light
	public float lightTime;			//durasi light yang tersisa
	public float lightTimeAdd;		//durasi tambahan light
	public float durationLightOff;	//durasi light meredup
	public Light heroLight;
	public RectTransform lightBar;
	public GameObject map;
	public bool isMove, isGameOver, isYellowSoul, isRedSoul;
	tileMap node;
	scoreManager score;
	float rotate, speed, effect;
	Vector2 world;
	Vector2 direct ;
	Animator anim;

	void Error () {
		//error
		if (!map) Debug.LogError ("map is null (heroController)");
		if (!lightBar) Debug.LogError ("lightBar is null (heroController)");
		if (!heroLight) Debug.LogError ("heroLight is null (heroController)");
	}

	void Start (){
		Error ();
		if (map){
			node 	= map.GetComponent<tileMap> ();
			score 	= map.GetComponent<scoreManager> ();

			if (!node) Debug.LogError ("node (map) is null (heroController)");
			if (!score) Debug.LogError ("score (map) is null (heroController)");
		}

		anim = gameObject.GetComponent<Animator> ();
		if (!anim) Debug.LogError ("anim (Animator) is null (heroController)");

		isGameOver 	= false;
		isMove 		= false;
		isRedSoul 	= false;
		isYellowSoul= false;
		world.x 	= Screen.width / 2;
		world.y 	= Screen.height / 2;
		speed 		= speedStart;
		rotate 		= transform.eulerAngles.z;
	}

	void Update () {
		if (isGameOver) {
			GameOver ();
		}

		Movement ();

		if (node) {
			ChangeNode ();
		}

		LightStatus ();
	}

	//pergerakan hero dengan mengklik/touch layar
	void Movement () {
		
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

		RaycastHit2D[] hits = Physics2D.RaycastAll (transform.position, direct, (node.tileSize / 2) + 0.1f);
		for (int i = 0; i < hits.Length; i++) {
			RaycastHit2D hit = hits [i];
			if (hit.collider.gameObject.tag == "Wall") { //jika di depan ada tembok, berhenti
				isMove = false;
				break;
			}
		}

		transform.eulerAngles = new Vector3(0, 0, rotate);
		if (isMove && !isYellowSoul) {
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
		else score.isExplorePath = true;;
	}

	//status light hero
	void LightStatus () {
		//jika waktu light habis  maka game over
		if (lightTime <= 0) {
			isGameOver = true;
		}
		else {
			if (lightBar) {
				float timeDown = lightTime / lightTimeMax;
				lightBar.localScale = new Vector2(1, timeDown);
				lightTime -= Time.deltaTime;
			}
		}
	}
		
	void GameOver () {
		Time.timeScale = 0; //pause
		if (heroLight) {
			heroLight.intensity -= durationLightOff + Time.deltaTime;
			if (heroLight.intensity <= 0) {
				print ("gameover");
				score.SaveScore ();
			}
		}
	}

	void OnCollisionEnter2D (Collision2D other){
		//jika menabrak musuh
		if (other.gameObject.tag == "Enemy") {
			lightTime  = 0;
			if (lightBar) lightBar.localScale = new Vector2(1, 0);
		}

		//jika menabrak blue soul
		if (other.gameObject.tag == "BlueSoul") {
			if (node && score) {
				node.Explode (other.gameObject.transform.position, new Color (0, 255, 255)); //buat partikel blue
				score.blueSoulScore++;		//update score blue soul
			}

			Destroy (other.gameObject);
			node.blueSoulCount--;			//update jumlah blue soul
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
				node.Explode (other.gameObject.transform.position, new Color (255, 0, 0)); //buat partikel red
				score.redSoulScore++;		//update score red soul
			}
	
			Destroy (other.gameObject);
			StartCoroutine (RedEffect (redDuration));
		}

		//jika menabrak yellow soul
		if (other.gameObject.tag == "YellowSoul") {
			if (node && score) {
				node.Explode (other.gameObject.transform.position, new Color (255, 255, 0)); //buat partikel yellow
				score.yellowSoulScore++;		//update score yellow soul
			}

			Destroy (other.gameObject);
			StartCoroutine (YellowEffect (yellowDuration, 5));
		}

	}

	IEnumerator YellowEffect(float waitTime, int loop)
	{
		isYellowSoul	  = true;
		while (loop > 0) {
			heroLight.enabled = false;

			yield return new WaitForSeconds (waitTime);
			heroLight.enabled = true;
			loop--;
			yield return new WaitForSeconds (waitTime);
		}
		isYellowSoul	  = false;

	}

	IEnumerator RedEffect(float waitTime)
	{
		if (heroLight) {
			heroLight.color = new Color (1, 0.5f, 0.5f);
		}
		isRedSoul 	= true;
		speed  		= speedStart / 2;
		lightTime 	/= 2;

		yield return new WaitForSeconds (waitTime);
		speed 			= speedStart;
		heroLight.color	= Color.white;
		isRedSoul 		= false;
	}
}
