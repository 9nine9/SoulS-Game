using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class chapterManager : MonoBehaviour {
	public Text textChapter;

	public AudioClip[] audioChapter;
	AudioSource audioSource;

	public GameObject[] objectChapter;

	public spawnSoul soul;
	public bool[] isSpawnYellowSoul;

	void Error (){
		if(!textChapter) Debug.LogError ("textChapter is null (chapterManager)");
		if(!soul) Debug.LogError ("soul is null (chapterManager)");

	}

	void Start () {
		Error ();
	
		audioSource = gameObject.GetComponent<AudioSource> ();
		if(!audioSource) Debug.LogError ("audioSource (AudioSource) is null (chapterManager)");

		for (int i = 0; i < isSpawnYellowSoul.Length; i++) {
			isSpawnYellowSoul [i] = false;
		}
		TextEnabled ("Chapter 1\r\nExplore the area");
	}

	void Update () {
		if (soul) {
			if (soul.isExplorePath && !isSpawnYellowSoul [0]) {
				if (soul.yellowSoul [0].activeInHierarchy) {
					TextEnabled ("Find the first Yellow Soul");
					isSpawnYellowSoul [0] = true;
				}
			} else if (!isSpawnYellowSoul [1]) {
				if (soul.yellowSoul [1].activeInHierarchy) {
					TextEnabled ("Find the second Yellow Soul");
					isSpawnYellowSoul [1] = true;
				}
			} else if (!isSpawnYellowSoul [2]) {
				if (soul.yellowSoul [2].activeInHierarchy) {
					TextEnabled ("Find the third Yellow Soul");
					isSpawnYellowSoul [2] = true;
				}
			} else if (!isSpawnYellowSoul [3]) {
				if (soul.yellowSoul [3].activeInHierarchy) {
					TextEnabled ("Find the fourth Yellow Soul");
					isSpawnYellowSoul [3] = true;
				}
			} else if (!isSpawnYellowSoul [4]) {
				if (soul.yellowSoul [4].activeInHierarchy) {
					TextEnabled ("Find the fifth Yellow Soul");
					isSpawnYellowSoul [4] = true;
				}
			}
		}
	}

	public void TextEnabled (string text) {
		if (textChapter) {
			textChapter.enabled = true;
			textChapter.text = text;
			StartCoroutine (TextDisabled (4f));
		}
	}

	IEnumerator TextDisabled (float waitTime) {
		yield return new WaitForSeconds (waitTime);
		textChapter.enabled = false;
	}

	IEnumerator ChangePosition (GameObject obj, Vector2[] position, float waitTime, int loop) {
		while (loop > 0 && obj) {
			yield return new WaitForSeconds (waitTime);
			loop--;
			yield return new WaitForSeconds (waitTime);
			obj.transform.position = position [loop];
		}
	}

	public void Chapter1 () {
		if (audioSource && audioChapter [0]) {
			audioSource.PlayOneShot (audioChapter [0], 0.2f);
		}

		GameObject obj = objectChapter [0];
		if (obj) {
			obj.SetActive (true);
			Destroy (obj, soul.hero.yellowDuration * 9);
		}
	}

	public void Chapter2 () {
		if (audioSource && audioChapter [1]) {
			audioSource.PlayOneShot (audioChapter [1], 0.5f);
		}

		GameObject obj = objectChapter [1];
		if (obj) {
			obj.SetActive (true);
			Destroy (obj, soul.hero.yellowDuration * 7);
		}
	}

	public void Chapter3 () {
		GameObject obj = objectChapter [2];
		if (obj) {
			if (audioSource && audioChapter [2]) {
				audioSource.PlayOneShot (audioChapter [2], 0.6f);
			}

			obj.SetActive (true);
			Destroy (obj, soul.hero.yellowDuration * 9f);

			Vector2[] position = new Vector2 [3];
			position [2] = new Vector2 (0, 13);
			position [1] = new Vector2 (0, 14);
			position [0] = new Vector2 (0, 15);

			StartCoroutine (ChangePosition (obj, position, soul.hero.yellowDuration, 3));

		}
	}

	public void Chapter4 () {
		if (audioSource && audioChapter [3]) {
			audioSource.PlayOneShot (audioChapter [3], 0.5f);
		}

		GameObject obj = objectChapter [3];
		if (obj) {
			obj.SetActive (true);
			Destroy (obj, soul.hero.yellowDuration * 9);
		}
	}

	public void Chapter5 () {
		if (audioSource && audioChapter [4]) {
			audioSource.PlayOneShot (audioChapter [4], 0.7f);
		}

		GameObject obj = objectChapter [4];
		if (obj) {
			obj.SetActive (true);
			Destroy (obj, soul.hero.yellowDuration * 9);
		}
	}
}
