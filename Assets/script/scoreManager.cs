using UnityEngine;

public class scoreManager : MonoBehaviour {
	public int blueSoulScore;		//score blue soul
	public int redSoulScore;		//score red soul
	public int yellowSoulScore;		//score yellow soul
	public int highBlueSoul;
	public int highRedSoul;
	public int highYellowSoul;
	public int isExplore;

	void Awake () {
		if (PlayerPrefs.HasKey ("highBlueSoul") || PlayerPrefs.HasKey ("highRedSoul") || PlayerPrefs.HasKey ("highYellowSoul") || PlayerPrefs.HasKey ("isExplore")) {
			highBlueSoul 	= PlayerPrefs.GetInt ("highBlueSoul");
			highRedSoul 	= PlayerPrefs.GetInt ("highRedSoul");
			highYellowSoul 	= PlayerPrefs.GetInt ("highYellowSoul");
			isExplore 		= PlayerPrefs.GetInt ("isExplore");
		} 
		else ResetScore ();
	}

	public void SaveScore () {
		if (blueSoulScore > highBlueSoul)
			PlayerPrefs.SetInt ("highBlueSoul", blueSoulScore);
		if (redSoulScore > highRedSoul)
			PlayerPrefs.SetInt ("highRedSoul", redSoulScore);
		if (yellowSoulScore > highYellowSoul)
			PlayerPrefs.SetInt ("highYellowSoul", yellowSoulScore);
		if (isExplore > 0) {
			PlayerPrefs.SetInt ("isExplore", 1);
		}
		PlayerPrefs.Save ();
	}

	public void ResetScore () {
		PlayerPrefs.SetInt ("highBlueSoul", 0);
		PlayerPrefs.SetInt ("highRedSoul", 0);
		PlayerPrefs.SetInt ("highYellowSoul", 0);
		PlayerPrefs.SetInt ("isExplore", 0);
		PlayerPrefs.Save ();
	}
}