using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class loadScene : MonoBehaviour {
	public string[] tipsString;
	public Text tipsText;
	public Sprite[] soul;
	public Image soulImage;

	public string loadString = "Loading...";
	public Text loadingText;
	string sceneName = "Menu"; //default
	float waitTime = 0f; //default

	void Error () {
		if (!tipsText) Debug.LogError ("tipsText is null (loadScene)");
		if (!loadingText) Debug.LogError ("loadingText is null (loadScene)");
		if (!soulImage) Debug.LogError ("soulImage is null (loadScene)");
	}

	void Start () {
		Time.timeScale = 1;
		Error ();

		if (tipsText) {
			int tips = Random.Range (0, tipsString.Length - 1);
			tipsText.text = tipsString [tips];
		}

		if (soulImage) {
			int tips = Random.Range (0, soul.Length - 1);
			soulImage.sprite = soul [tips];
		}

		if(loadingText)
			loadingText.text = loadString;

		if (PlayerPrefs.HasKey ("sceneName")) {
			sceneName = PlayerPrefs.GetString ("sceneName");
		}
		if (PlayerPrefs.HasKey ("waitTime")) {
			waitTime = PlayerPrefs.GetFloat ("waitTime");
		}

		StartCoroutine (Load ());
	}

	void Update () {
		if(loadingText)
			loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong (Time.time, 1));
	}

	IEnumerator Load () {
		yield return new WaitForSeconds (waitTime);
		AsyncOperation async = SceneManager.LoadSceneAsync (sceneName);
		while (!async.isDone) {
			yield return null;
		}
	}

}