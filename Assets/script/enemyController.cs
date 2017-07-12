using UnityEngine;

public class enemyController : MonoBehaviour {
	public tileMap node;
	public float speed;

	void Error () {
		//error
		if (!node) Debug.LogError ("node is null (enemyController)");
	}

	void Start () {
		Error ();
	}

	void Update () {
		if (node) {
			ChangeNode ();
		}

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

}
