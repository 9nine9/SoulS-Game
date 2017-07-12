using UnityEngine;
using System.Collections.Generic;

public class pathFinding : MonoBehaviour {
	public tileMap node;
	public List<tileMap.Node> open, close, path;

	public float timeStart;
	public float timeGetPath;
	float rotate;

	enemyController enemy;
	Animator anim;

	void Error () {
		//error
		if (!node) Debug.LogError ("node is null (pathFinding)");
	}

	void Start () {
		Error ();

		enemy = gameObject.GetComponent<enemyController> ();
		if (!enemy) Debug.LogError ("enemy (enemyController) is null (pathFinding)");

		anim = gameObject.GetComponent<Animator> ();
		if (!anim) Debug.LogError ("anim is null (pathFinding)");

		rotate 	= transform.eulerAngles.z;

		path 	= new List<tileMap.Node> ();
		open 	= new List<tileMap.Node> ();
		close 	= new List<tileMap.Node> ();

		InvokeRepeating ("UpdateTarget", timeStart, timeGetPath);
	}

	void Update () {
		RunPath ();
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

	void RunPath(){
		int current = path.Count - 1;
		if (current >= 0) {
			RotateSelf (node.enemy, path [current]);

			float x = node.map [path [current].x, path [current].y].position.x;
			float y = node.map [path [current].x, path [current].y].position.y;

			Vector2 currentPosition = transform.position;
			Vector2 targetPosition = new Vector2 (x, y);

			if (Vector2.Distance (currentPosition, targetPosition) > 0) {
				if (enemy) {
					transform.position = Vector2.MoveTowards (currentPosition, targetPosition, enemy.speed * Time.deltaTime);
				}
			}
			else {
				path.RemoveAt (current);
			}
		}
	}


	void GetCost (tileMap.Node A, tileMap.Node B, int step) {

		int distance = Mathf.Abs (B.x - A.x) + Mathf.Abs (B.y - A.y);
		int cost 	 = step + distance;

		if (cost <= node.map [A.x, A.y].cost) {
			node.map [A.x, A.y].cost = cost;
			node.map [A.x, A.y].step = step;
		}
	}

	void GetPath (tileMap.Node A, tileMap.Node B) {
		ResetNode ();

		int cost, up, down, right, left, x, y, step = 0;
		bool check;
		tileMap.Node current, child;

		open.Add (A);
		current = open [0];
		GetCost (current, B, step);

		for(;;){
			
			if (open.Contains (B)) { //path ditemukan
				print("ketemu");
				path.Add (B);

				while (!path.Contains (A)) {
					path.Add (current);
					current = node.map [current.x, current.y].parent;
				}

				path.Remove (A);

				if (anim) {
					anim.SetBool ("isMove", true);
				}
				break;
			}
			else if (open.Count <= 0) { //path tidak ditemukan
				print("not");
				if (anim) {
					anim.SetBool ("isMove", false);
				}
				break;
			}

			cost 	= 9999;
			for (int i = 0; i < open.Count; i++) {
				if (node.map [open [i].x, open [i].y].cost < cost) {
					cost    = node.map [open [i].x, open [i].y].cost;
					current = open [i];
				}
			}

			open.Remove (current);
			close.Add (current);

			up 		= current.y + 1;
			down	= current.y - 1;
			right 	= current.x + 1;
			left 	= current.x - 1;
			x		= current.x;
			y 		= current.y;

			if (up < node.mapHeight && node.map [x, up].isOpen) { //up path
				child.x = x;
				child.y = up;
				check 	= false;
				if (!close.Contains (child)) {
					if (!open.Contains (child)) {
						check = true;
						open.Add (child);
					}
					else if (node.map [x, y].step < node.map [x, up].step) {
						check = true;
					}

					if (check) {
						node.map [x, up].parent = current;
						step = node.map [x, y].step + 1;
						GetCost (child, B, step);
					}
				}
			}

			if (down >= 0 && node.map [x, down].isOpen) {  //bottom paths
				child.x = x;
				child.y = down;
				check 	= false;
				if (!close.Contains (child)) {
					if (!open.Contains (child)) {
						check = true;
						open.Add (child);
					}
					else if (node.map [x, y].step < node.map [x, down].step) {
						check = true;
					}

					if (check) {
						node.map [x, down].parent = current;
						step = node.map [x, y].step + 1;
						GetCost (child, B, step);
					}
				}
			}

			if (right < node.mapWidth && node.map [right, y].isOpen){ //right path
				child.x = right;
				child.y = y;
				check 	= false;
				if (!close.Contains (child)) {
					if (!open.Contains (child)) {
						check = true;
						open.Add (child);
					}
					else if (node.map [x, y].step < node.map [right, y].step) {
						check = true;
					}

					if (check) {
						node.map [right, y].parent = current;
						step = node.map [x, y].step + 1;
						GetCost (child, B, step);
					}
				}
			}

			if (left >= 0 && node.map [left, y].isOpen) { //left path
				child.x = left;
				child.y = y;
				check 	= false;
				if (!close.Contains (child)) {
					if (!open.Contains (child)) {
						check = true;
						open.Add (child);
					}
					else if (node.map [x, y].step < node.map [left, y].step) {
						check = true;
					}

					if (check) {
						node.map [left, y].parent = current;
						step = node.map [x, y].step + 1;
						GetCost (child, B, step);
					}
				}
			}


		}
	}

	void ResetNode () {
		for (int i = 0; i < open.Count; i++) {
			node.map [open [i].x, open [i].y].step = 9999;
			node.map [open [i].x, open [i].y].cost = 9999;
		}
		for (int i = 0; i < close.Count; i++) {
			node.map [close [i].x, close [i].y].step = 9999;
			node.map [close [i].x, close [i].y].cost = 9999;
		}

		open 	= new List<tileMap.Node> ();
		close 	= new List<tileMap.Node> ();
		path 	= new List<tileMap.Node> ();
	}

	void UpdateTarget () {
		GetPath (node.enemy, node.hero);
	}
}
