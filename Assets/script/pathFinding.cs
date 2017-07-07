using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class pathFinding : MonoBehaviour {
	public tileMap node;
	public List<tileMap.Node> path;
	public List<tileMap.Node> close;
	public int speed;
	float rotate;
	public GameObject p;
	public int o = 0;

	void Error () {
		//error
		if (!node) Debug.LogError ("node is null (pathFinding)");
	}

	void Start () {
		Error ();
		rotate 	= transform.eulerAngles.z;
		path 	= new List<tileMap.Node> ();
	}

	void Update () {
		//RunPath ();

	}
		
	void RotateSelf (tileMap.Node A, tileMap.Node B) {
		if (B.x > A.x) { //right
			rotate = 270f;
		}
		else if (B.x < A.x) { //left
			rotate = 90f;
		}
		else if (B.y > A.y) { //up
			rotate = 0f;
		}
		else if (B.y < A.y) { //down
			rotate = 180f;
		}

		transform.eulerAngles = new Vector3(0, 0, rotate);
	}

	void GetPath (tileMap.Node A, tileMap.Node B) {
	
	}

	/** void RunPath(){
		if (path.Count > 0) {
			float x = node.map [path [0].x, path [0].y].position.x;
			float y = node.map [path [0].x, path [0].y].position.y;

			Vector2 currentPosition = transform.position;
			Vector2 targetPosition = new Vector2 (x, y);

			if (Vector2.Distance (currentPosition, targetPosition) > 0) {
				transform.position = Vector2.MoveTowards (currentPosition, targetPosition, speed * Time.deltaTime);
			}
			else {
				path.RemoveAt (0);
				//print(path[0].x+","+path[0].y); //path target

				if (path.Count > 0) {
					RotateSelf (node.enemy, path [0]);
				}
			}
		} 
		else {
			if (node) {
				close = new List<tileMap.Node> ();
				print ("(" + node.enemy.x + "," + node.enemy.y + ") - (" + node.hero.x + "," + node.hero.y + ")");
				GetPath (node.enemy, node.hero);
				//print (close.Count);
				/*for (int a = 0; a < node.mapWidth; a++) {
					for (int b = 0; b < node.mapHeight; b++) {
						if (!node.map [a, b].isOpen || node.map [a, b].cost != 9999) {
							GameObject c = Instantiate (p, new Vector2 (node.map [a, b].position.x, node.map [a, b].position.y), Quaternion.identity);
							Destroy (c, 3);
						}
					}
				}
				for (int i = 0; i < close.Count; i++) {
					//GameObject a = Instantiate (p, new Vector2 (node.map [close [i].x, close [i].y].position.x, node.map [close [i].x, close [i].y].position.y), Quaternion.identity);
					//Destroy (a, 3);
					node.map [close [i].x, close [i].y].cost = 9999;
					node.map [close [i].x, close [i].y].isOpen = true;
				}

			} 

		}
	} **/


	/** bool GetCost (tileMap.Node A, tileMap.Node B, int step, string next){
		int hor = 0, ver = 0;

		switch (next){
			case "up":
				ver = 1;
				break;
			case "down":
				ver = -1;
				break;
			case "right":
				hor = 1;
				break;
			case "left":
				hor = -1;
				break;
			default:
				break;
		}
			
		int distance = Mathf.Abs ((B.x - (A.x + hor))) + Mathf.Abs ((B.y - (A.y + ver)));
		int cost 	 = step + distance;

		if (cost <= node.map [(A.x + hor), (A.y + ver)].cost) {
			node.map [(A.x + hor), (A.y + ver)].cost = cost;
			return true;
		}
		else {
			node.map [(A.x + hor), (A.y + ver)].isOpen = true;
			return false;
		}
	}



	void GetPath (tileMap.Node A, tileMap.Node B) {
		close.Add (A);
		path.Add (A);
		int currentNode = path.Count - 1;
		GetCost (path [currentNode], B, 0, "");
	
		tileMap.Node next;
		next.x = path [currentNode].x;
		next.y = path [currentNode].y;
		int right, left, up, down, x, y, cost, min;
		bool block;

		for (int step = 1;;step++) {
			min = 9999;
			node.map [path [currentNode].x, path [currentNode].y].isOpen = false;

			block 	= true;
			up 		= path [currentNode].y + 1;
			down	= path [currentNode].y - 1;
			right 	= path [currentNode].x + 1;
			left 	= path [currentNode].x - 1;
			x 		= path [currentNode].x;
			y 		= path [currentNode].y;

			//update cost di masing-masing tile cabang (child) dari tile currentNode(parent)
			if (up < node.mapHeight && node.map [x, up].isOpen) { //up path
				
				if (GetCost (path [currentNode], B, step, "up")) {
					cost = node.map [x, up].cost;
					if (cost < min) {
						min = cost;
						next.x	= x;
						next.y  = up;
						block 	= false;
					}
					close.Add (next);
				} 
			}

			if (down >= 0 && node.map [x, down].isOpen) {  //bottom path

				if (GetCost (path [currentNode], B, step, "down")) {
					cost = node.map [x, down].cost;
					if (cost < min) {
						min = cost;
						next.x	= x;
						next.y  = down;
						block 	= false;
					}
					close.Add (next);
				}
			}

			if (right < node.mapWidth && node.map [right, y].isOpen){ //right path
				
				if (GetCost (path [currentNode], B, step, "right")) {
					cost = node.map [right, y].cost;
					if (cost < min) {
						min = cost;
						next.x	= right;
						next.y	= y;
						block	= false;
					}
					close.Add (next);
				}
			}

			if (left >= 0 && node.map [left, y].isOpen) { //left path
				
				if (GetCost (path [currentNode], B, step, "left")) {
					cost = node.map [left, y].cost;
					if (cost < min) {
						min = cost;
						next.x 	= left;
						next.y	= y;
						block 	= false;
					}
					close.Add (next);
				}
			}

			if (block) {
				step -= 2;
				path.RemoveAt (currentNode);
				currentNode--;
				if (currentNode >= 0 && o == 1) {
					GameObject a = Instantiate (p, new Vector2 (node.map [path [currentNode].x, path [currentNode].y].position.x, node.map [path [currentNode].x, path [currentNode].y].position.y), Quaternion.identity);
					Destroy (a, step);
					//StartCoroutine (inc(path[currentNode], 1));
					//print (step + "back" + path [currentNode].x + "," + path [currentNode].y + "=" + node.map [path [currentNode].x, path [currentNode].y].cost);
				}
			} else {
				if (o == 1) {
					GameObject a = Instantiate (p, new Vector2 (node.map [next.x, next.y].position.x, node.map [next.x, next.y].position.y), Quaternion.identity);
					Destroy (a, step);
					//StartCoroutine (inc(next, 1));
					//print (step + "next" + next.x + "," + next.y + "=" + node.map [next.x, next.y].cost);
				}
				//step++;
				path.Add (next);
				currentNode++;
			}

			if (currentNode < 0)
				break;
			else {
				if (path [currentNode].x == B.x && path [currentNode].y == B.y) {
					print ("getpath");
					break;
					//print (B.xPos+"," + B.yPos + " == "+path [currentNode].xPos +","+path [currentNode].yPos + " = "+map.map[path [currentNode].xPos, path [currentNode].yPos].cost);
				}
			}

		}

	} **/

	IEnumerator inc(tileMap.Node J, float delay){
		yield return new WaitForSeconds (delay);
		Vector2 A;
		print ("a");
		A.x = node.map [J.x, J.y].position.x;
		A.y = node.map [J.x, J.y].position.y;
		GameObject a = Instantiate (p, A, Quaternion.identity);
		Destroy (a, 3);
	}
}
