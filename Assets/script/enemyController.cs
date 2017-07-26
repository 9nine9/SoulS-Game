using UnityEngine;

public class enemyController : MonoBehaviour {
	public GameObject map;
	tileMap node;
	scoreManager score;

	public float speed;
	public float[] speedRandom;
	public bool isSpeedUltimate;
	public int chanceUltimate = 25;
	public float speedUltimate;
	public int blueScoreForSpeedUtimate;
	public float durationSpeed;

	void Error () {
		if (!map) Debug.LogError ("map is null (enemyController)");
	}

	void Start () {
		Error ();
		if (map) {
			node = map.GetComponent<tileMap> ();
			score = map.GetComponent<scoreManager> ();

			if (!score) Debug.LogError ("score (map) is null (enemyController)");
			else InvokeRepeating ("ChangeSpeed", durationSpeed, durationSpeed);
			if (!node) Debug.LogError ("score (map) is null (enemyController)");
			else ChangeNode ();
		}
	}

	void Update () {
		if (node) ChangeNode ();
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
	}

	void ChangeSpeed () {
		int newSpeed = Random.Range (0, speedRandom.Length - 1);
		speed = speedRandom [newSpeed];

		if (isSpeedUltimate && Random.Range (1, 100) <= chanceUltimate)
			speed = speedUltimate;
		else if (score.blueSoulScore >= blueScoreForSpeedUtimate)
			isSpeedUltimate = true;
	}
}