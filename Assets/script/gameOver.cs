using UnityEngine;

public class gameOver : MonoBehaviour {
	scoreManager score;
	public bool isGameOver;
	public Light heroLight;
	public Light enemyLight;
	public float durationLightOff;	//durasi light meredup

	void Error () {
		//error
		if (!heroLight) Debug.LogError ("heroLight is null (gameOver)");
		if (!enemyLight) Debug.LogError ("enemyLight is null (gameOver)");
	}

	void Start () {
		Error ();

		score = gameObject.GetComponent<scoreManager> ();
		if (!score) Debug.LogError ("score (scoreManager) is null (gameOver)");

		isGameOver 	= false;
	}
	
	void Update () {
		if (isGameOver) {
			Time.timeScale = 0; //pause
			HeroLightOff ();
			EnemyLightOff ();
		}
	}

	void HeroLightOff () {
		if (heroLight) {
			if (heroLight.intensity > 0) {
				heroLight.intensity -= durationLightOff + Time.deltaTime;
			}
			else {
				if(score) score.SaveScore ();
			}
		}
	}

	void EnemyLightOff () {
		if (enemyLight) {
			if (enemyLight.intensity > 0) {
				enemyLight.intensity -= durationLightOff + Time.deltaTime;
			}
		}
	}
}
