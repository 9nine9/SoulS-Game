using UnityEngine;
using UnityEngine.UI;

public class scoreManager : MonoBehaviour {
	public int blueSoulScore;		//score blue soul
	public int redSoulScore;		//score red soul
	public int yellowSoulScore;		//score yellow soul
	public int highBlueSoul;
	public int highRedSoul;
	public int highYellowSoul;

	public int[] minRedSoul;
	public GameObject[] yellowSoul;
	public bool isExplorePath;

	public Text scoreDisplay;

	void Error () {
		//error
		if (!scoreDisplay) Debug.LogError ("scoreDisplay is null (scoreManager)");
	}

	void Start 	() {
		Error ();
		if (PlayerPrefs.HasKey ("highBlueSoul") || PlayerPrefs.HasKey ("highRedSoul") || PlayerPrefs.HasKey ("highYellowSoul")) {
			highBlueSoul 	= PlayerPrefs.GetInt ("highBlueSoul");
			highRedSoul 	= PlayerPrefs.GetInt ("highRedSoul");
			highYellowSoul 	= PlayerPrefs.GetInt ("highYellowSoul");
		} 
		else ResetScore ();

		isExplorePath = false;
	}

	void Update () {
		ChangeBlueScore ();
		SpawnYellowSoul ();
	}

	void ChangeBlueScore () {
		if (scoreDisplay) scoreDisplay.text = blueSoulScore.ToString ();
	}

	void SpawnYellowSoul () {
		if (yellowSoulScore < minRedSoul.Length) {
			int step = yellowSoulScore;
			if (redSoulScore >= minRedSoul [step] && isExplorePath) {
				if (yellowSoul [step]) {
					yellowSoul [step].SetActive (true);
				}
				else Debug.LogError ("yellowSoul (" + step + ") is null (scoreManager)");
			}
		}
	}

	public void SaveScore () {
		if(blueSoulScore > highBlueSoul)
			PlayerPrefs.SetInt ("highBlueSoul", blueSoulScore);
		if(redSoulScore > highRedSoul)
			PlayerPrefs.SetInt ("highRedSoul", redSoulScore);
		if(yellowSoulScore > highYellowSoul)
			PlayerPrefs.SetInt ("highYellowSoul", yellowSoulScore);
		PlayerPrefs.Save ();
	}

	public void ResetScore () {
		PlayerPrefs.SetInt ("highBlueSoul", 0);
		PlayerPrefs.SetInt ("highRedSoul", 0);
		PlayerPrefs.SetInt ("highYellowSoul", 0);
		PlayerPrefs.Save ();
	}
}