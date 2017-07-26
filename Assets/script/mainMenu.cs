using UnityEngine;
using UnityEngine.UI;

public class mainMenu : MonoBehaviour {
	scoreManager score;
	public bool reset;
	public Text blueScore;
	public Text redScore;
	public Text yellowScore;
	public int yellowScoreMax;

	void Error () {
		if (!blueScore) Debug.LogError ("blueScore is null (mainMenu)");
		if (!redScore) Debug.LogError ("redScore is null (mainMenu)");
		if (!yellowScore) Debug.LogError ("yellowScore is null (mainMenu)");
	}

	void Start () {
		Time.timeScale = 1;
		Error ();

		score = GetComponent<scoreManager> ();
		if (!score) Debug.LogError ("score (scoreManager) is null (mainMenu)");

		if (score) {
			if (reset) score.ResetScore ();
			StatusHighScore ();
		}
	}

	void StatusHighScore () {
		blueScore.text = score.highBlueSoul.ToString ();
		redScore.text = score.highRedSoul.ToString ();

		float yellowPercentage = ((float) score.highYellowSoul / yellowScoreMax) * 100;
		yellowScore.text = yellowPercentage.ToString () + " %";
	}

	public void Exit (){
		Application.Quit ();
	}

	public void Review (string url){
		Application.OpenURL (url);
	}
}
