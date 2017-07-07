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

	public int mapHeight, mapWidth;			//ukuran map 
	public float tileSize;					//besar tile/node
	public int blueSoulMax, blueSoulCount;	//maximum blue soul yang dispawn
	public GameObject particle;
	public GameObject blueSoul;
	public Tile[,] map;
	public List<Node> nodeOpen, nodeSpawn;	//nodeOpen (node untuk yang belum dilewati hero), nodeSpawn (node kosong untuk spawn soul)
	public Node hero, enemy;				//node hero dan enemy

	void Error () {
		//error
		if (!blueSoul) Debug.LogError ("blueSoul is null (tileMap)");
		if (!particle) Debug.LogError ("particle is null (tileMap)");
	}

	void Awake () {
		Error ();
		CreateMapTile ();
	}

	void Update () {
		if (blueSoul) {
			SpawnBlueSoul ();
		}
	}
		
	//buat tile/node dari map
	void CreateMapTile () {
		nodeOpen  = new List<Node> ();
		nodeSpawn = new List<Node> ();
		map  	  = new Tile[mapWidth, mapHeight];

		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {
				Vector2 here 		= new Vector3 ((x * tileSize) + transform.position.x, (y * tileSize) + transform.position.y);
				map [x, y].isOpen 	= true;
				map [x, y].position	= here;
				map [x, y].cost 	= 9999;
				map [x, y].step 	= -1;
				map [x, y].parent.x	= x;
				map [x, y].parent.y	= y;

				//tambahkan node ke list
				nodeOpen.Add (map [x, y].parent);
				nodeSpawn.Add (map [x, y].parent);

				RaycastHit2D hit;
				if (hit = Physics2D.Raycast (here, Vector2.up, 0.1f)) {
					if (hit.collider.gameObject.tag == "Wall") { //block node karena terdapat penghalang
						map [x, y].isOpen = false;
						nodeOpen.Remove (map [x, y].parent);
						nodeSpawn.Remove (map [x, y].parent);
					}
				}
			}
		}

	}
		
	//spawn blue soul
	void SpawnBlueSoul () {
		//jika jumlah blue soul kurang dari maximum, maka spawn blue soul
		while (blueSoulCount < blueSoulMax && nodeSpawn.Count > 0) {
			int spawnLocation = Random.Range (0, (nodeSpawn.Count - 1));
			Vector2 location  = map [nodeSpawn [spawnLocation].x, nodeSpawn [spawnLocation].y].position;
			Instantiate (blueSoul, location, Quaternion.identity);
			nodeSpawn.RemoveAt (spawnLocation);	//hapus node dari list nodeSpawn
			blueSoulCount++;
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
}
