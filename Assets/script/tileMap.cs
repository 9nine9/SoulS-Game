using UnityEngine;
using System.Collections.Generic;

public class tileMap : MonoBehaviour {
	public struct Node{
		public int x, y;
	};

	public struct Tile{
		public Vector2 position;
		public bool isOpen;
		public int step;
		public int cost;
		public Node parent;
	};

	public int mapHeight, mapWidth;
	public float tileSize;
	public Tile[,] map;
	public List<Node> nodeOpen, nodeSpawn;
	public Node hero, enemy;

	void Awake () {
		CreateMapTile ();
	}

	void CreateMapTile () {
		nodeOpen = new List<Node> ();
		map  	  = new Tile[mapWidth, mapHeight];

		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {
				Vector2 here 		= new Vector2 ((x * tileSize) + transform.position.x, (y * tileSize) + transform.position.y);

				map [x, y].isOpen 	= true;
				map [x, y].position	= here;
				map [x, y].cost 	= 9999;
				map [x, y].step 	= 9999;
				map [x, y].parent.x	= x;
				map [x, y].parent.y	= y;

				nodeOpen.Add (map [x, y].parent);

				RaycastHit2D hit;
				if (hit = Physics2D.Raycast (here, Vector2.up, 0.1f)) {
					if (hit.collider.gameObject.tag == "Wall") {
						map [x, y].isOpen = false;
						nodeOpen.Remove (map [x, y].parent);
					}
				}
			}
		}
		nodeSpawn = new List<Node>(nodeOpen);
	}
}