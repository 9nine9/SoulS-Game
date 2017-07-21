using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManager : MonoBehaviour {
	public string loadSceneName = "LoadScene";

	public void SceneName (string name){
		PlayerPrefs.SetString ("sceneName", name);
		SceneManager.LoadSceneAsync (loadSceneName);
	}

	public void WaitTime (float time){
		PlayerPrefs.SetFloat ("waitTime", time);
	}
}