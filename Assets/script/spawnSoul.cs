using UnityEngine;
using System.Collections;

public class spawnSoul : MonoBehaviour {
	public chapterManager chapter;
	public GameObject particle;

	public GameObject blueSoul;
	public int blueSoulMax, blueSoulCount;

	public GameObject redSoul;
	public Transform enemy;
	public float startRedSpawn, spawnRedTime, redSoulDuration;

	public Light heroLight;
	public heroController hero;
	public int currentRedSoul;
	public int[] minRedSoul;
	public GameObject[] yellowSoul;
	public bool isExplorePath, isYellowSoul, isRedSoul;
	public AudioClip[] soulSoundEffect;
	AudioSource audioSoul;

	tileMap node;
	scoreManager score;

	void Error () {
		if (!blueSoul) Debug.LogError ("blueSoul is null (spawnSoul)");
		if (!redSoul) Debug.LogError ("redSoul is null (spawnSoul)");
		if (!particle) Debug.LogError ("particle is null (spawnSoul)");
		if (!enemy) Debug.LogError ("enemy is null (spawnSoul)");
		if (!heroLight) Debug.LogError ("heroLight is null (spawnSoul)");
		if (!hero) Debug.LogError ("hero is null (spawnSoul)");
		if (!chapter) Debug.LogError ("chapter is null (spawnSoul)");
	}

	void Start () {
		Error ();

		node = gameObject.GetComponent<tileMap> ();
		score = gameObject.GetComponent<scoreManager> ();
		audioSoul = gameObject.GetComponent<AudioSource> ();

		if (!audioSoul) Debug.LogError ("audioSoul (AudioSource) is null (spawnSoul)");
		if (!node) Debug.LogError ("node (tileMap) is null (spawnSoul)");
		if (!score) Debug.LogError ("score (scoreManager) is null (spawnSoul)");

		if (redSoul) {
			InvokeRepeating("SpawnRedSoul", startRedSpawn, spawnRedTime);
		}
			

		isExplorePath 	= false;
		isRedSoul 		= false;
		isYellowSoul	= false;
	}
	
	void Update () {
		if (blueSoul && node) {
			SpawnBlueSoul ();
		}
		if (score) {
			SpawnYellowSoul ();
		}
	}

	//spawn particle
	public void Explode (Vector2 position, Color color) {
		if (particle) {
			GameObject newParticle 			= Instantiate (particle, position, Quaternion.identity) as GameObject;
			ParticleSystem.MainModule part 	= newParticle.GetComponent<ParticleSystem> ().main;
			part.startColor 				= color;
			float duration 					= part.duration + part.startLifetime.constant;

			Destroy (newParticle, duration);
		}
	}

	//spawn blue soul
	void SpawnBlueSoul () {
		//jika jumlah blue soul kurang dari maximum, maka spawn blue soul
		while (blueSoulCount < blueSoulMax && node.nodeSpawn.Count > 0) {
			int spawnLocation = Random.Range (0, (node.nodeSpawn.Count - 1));
			Vector2 location  = node.map [node.nodeSpawn [spawnLocation].x, node.nodeSpawn [spawnLocation].y].position;
			Instantiate (blueSoul, location, Quaternion.identity);
			node.nodeSpawn.RemoveAt (spawnLocation);	//hapus node dari list nodeSpawn
			blueSoulCount++;
		}
	}

	//spawn red soul
	void SpawnRedSoul () {
		if (isExplorePath) {
			Vector2 location = enemy.position;
			GameObject spawnRedSoul = Instantiate (redSoul, location, Quaternion.identity);
			Destroy (spawnRedSoul, redSoulDuration);
		}
	}

	void SpawnYellowSoul () {
		if (score.yellowSoulScore < minRedSoul.Length) {
			int step = score.yellowSoulScore;
			if (currentRedSoul >= minRedSoul [step] && isExplorePath) {
				if (yellowSoul [step]) {
					currentRedSoul = 0;
					yellowSoul [step].SetActive (true);
				}
				else Debug.LogError ("yellowSoul (" + step + ") is null (spawnSoul)");
			}
		}
	}

	public IEnumerator YellowEffect(float waitTime, int loop) {
		if (heroLight) {
			isYellowSoul = true;
			while (loop > 0) {
				heroLight.enabled = false;

				yield return new WaitForSeconds (waitTime);
				heroLight.enabled = true;
				loop--;
				yield return new WaitForSeconds (waitTime);
			}
			isYellowSoul = false;

			if (chapter) {
				switch (score.yellowSoulScore) {
				case 1:
					chapter.TextEnabled ("Chapter 2\r\nGet a Red Soul");
					break;
				case 2:
					chapter.TextEnabled ("Chapter 3\r\nGet 3 more Red Souls");
					break;
				case 3:
					chapter.TextEnabled ("Chapter 4\r\nGet 5 more Red Souls");
					break;
				case 4:
					chapter.TextEnabled ("Final Chapter\r\nGet 10 more Red Souls");
					break;
				case 5:
					chapter.TextEnabled ("The End");
					break;
				}

			}
		}
	}

	public IEnumerator RedEffect(float waitTime) {
		if (hero && heroLight) {
			if (soulSoundEffect [0]) {
				audioSoul.PlayOneShot (soulSoundEffect [0], 0.5f);
			}

			heroLight.color = new Color (1, 0.5f, 0.5f);

			isRedSoul = true;
			hero.speed = hero.speedStart / 2;
			hero.lightTime /= 2;

			yield return new WaitForSeconds (waitTime);
			hero.speed = hero.speedStart;
			heroLight.color	= Color.white;
			isRedSoul = false;
		}
	}

}
