using UnityEngine;

public class enemyController : MonoBehaviour {
	public GameObject redSoul;
	public tileMap node;
	public heroController hero;
	public Light enemyLight;
	public float startSpawn;
	public float spawnRedSoulTime;
	public float redSoulDuration;

	void Error () {
		//error
		if(!node) Debug.LogError ("node is null (enemyController)");
		if(!hero) Debug.LogError ("hero is null (enemyController)");
		if(!redSoul) Debug.LogError ("redSoul is null (enemyController)");
		if(!enemyLight) Debug.LogError ("enemyLight is null (enemyController)");
	}

	void Start () {
		Error ();

		if (redSoul) {
			InvokeRepeating("SpawnRedSoul", startSpawn, spawnRedSoulTime);
		}
	}

	void Update () {
		if (node) {
			ChangeNode ();
		}

		if (hero && enemyLight) {
			LightStatus ();
		}
	}

	//spawn red soul
	void SpawnRedSoul () {
		Vector2 location  = transform.position;
		GameObject spawnRedSoul = Instantiate (redSoul, location, Quaternion.identity);
		Destroy (spawnRedSoul, redSoulDuration);
	}

	//node yang ditempati enemy
	void ChangeNode () {
		int x = (int) Mathf.Round (transform.position.x / node.tileSize);
		int y = (int) Mathf.Round (transform.position.y / node.tileSize);

		//jika node tidak melewati batas map, maka update
		if (x >= 0 && x < node.mapWidth && y >= 0 && y < node.mapHeight) {
			node.enemy.x = x;
			node.enemy.y = y;
		}
		//print (node.enemy.x+","+node.enemy.y);
	}

	void LightStatus () {
		if (hero.isGameOver) {
			if (enemyLight.intensity > 0) {
				enemyLight.intensity -= hero.durationLightOff + Time.deltaTime;
			}
		}
	}
}
