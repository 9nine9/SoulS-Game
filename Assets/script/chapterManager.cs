using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class chapterManager : MonoBehaviour {
	public Text textChapter;
	public string[] textStart;
	public string[] textFinish;

	public AudioClip[] audioChapter;
	AudioSource audioSource;

	public GameObject[] objectChapter;

	public GameObject map;
	spawnSoul soul;
	scoreManager score;
	public bool[] isSpawnYellowSoul;

	void Error (){
		if(!textChapter) Debug.LogError ("textChapter is null (chapterManager)");
		if(!map) Debug.LogError ("map is null (chapterManager)");
	}

	void Start () {
		Error ();
	
		if (map) {
			soul = map.GetComponent<spawnSoul> ();
			score = map.GetComponent<scoreManager> ();

			if(!soul) Debug.LogError ("soul (map) is null (chapterManager)");
			if(!score) Debug.LogError ("score (map) is null (chapterManager)");
		}

		audioSource = GetComponent<AudioSource> ();
		if(!audioSource) Debug.LogError ("audioSource (AudioSource) is null (chapterManager)");

		for (int i = 0; i < isSpawnYellowSoul.Length; i++)
			isSpawnYellowSoul [i] = false;

		TextEnabled (textStart [0]);
	}

	void Update () {
		if (soul) {
			if (soul.isExplorePath && !isSpawnYellowSoul [0]) {
				if (soul.yellowSoul [0].activeInHierarchy) {
					TextEnabled (textFinish [0]);
					isSpawnYellowSoul [0] = true;
				}
			}
			else if (!isSpawnYellowSoul [1]) {
				if (soul.yellowSoul [1].activeInHierarchy) {
					TextEnabled (textFinish [1]);
					isSpawnYellowSoul [1] = true;
				}
			}
			else if (!isSpawnYellowSoul [2]) {
				if (soul.yellowSoul [2].activeInHierarchy) {
					TextEnabled (textFinish [2]);
					isSpawnYellowSoul [2] = true;
				}
			}
			else if (!isSpawnYellowSoul [3]) {
				if (soul.yellowSoul [3].activeInHierarchy) {
					TextEnabled (textFinish [3]);
					isSpawnYellowSoul [3] = true;
				}
			}
			else if (!isSpawnYellowSoul [4]) {
				if (soul.yellowSoul [4].activeInHierarchy) {
					TextEnabled (textFinish [4]);
					isSpawnYellowSoul [4] = true;
				}
			}
		}
	}

	void TextEnabled (string text) {
		if (textChapter) {
			textChapter.enabled = true;
			textChapter.text = text.Replace ("<br>", "\n");;
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

	public void TitleChapter () {
		switch (score.yellowSoulScore) {
		case 1:
			TextEnabled (textStart [1]);
			break;
		case 2:
			TextEnabled (textStart [2]);
			break;
		case 3:
			TextEnabled (textStart [3]);
			break;
		case 4:
			TextEnabled (textStart [4]);
			break;
		case 5:
			TextEnabled (textStart [5]);
			break;
		}
	}

	public void Chapter1 () {
		if (audioSource && audioChapter [0])
			audioSource.PlayOneShot (audioChapter [0], 0.2f);

		GameObject obj = objectChapter [0];
		if (obj) {
			obj.SetActive (true);
			Destroy (obj, soul.hero.yellowDuration * 9);
		}
	}

	public void Chapter2 () {
		if (audioSource && audioChapter [1])
			audioSource.PlayOneShot (audioChapter [1], 0.5f);

		GameObject obj = objectChapter [1];
		if (obj) {
			obj.SetActive (true);
			Destroy (obj, soul.hero.yellowDuration * 7);
		}
	}

	public void Chapter3 () {
		if (audioSource && audioChapter [2])
			audioSource.PlayOneShot (audioChapter [2], 0.6f);

		GameObject obj = objectChapter [2];
		if (obj) {
			obj.SetActive (true);
			Destroy (obj, soul.hero.yellowDuration * 9f);

			Vector2[] position = new Vector2 [3];
			position [2] = new Vector2 (0, 13);
			position [1] = new Vector2 (0, 14);
			position [0] = new Vector2 (0, 15);

			StartCoroutine (ChangePosition (obj, position, soul.hero.yellowDuration, position.Length));
		}
	}

	public void Chapter4 () {
		if (audioSource && audioChapter [3])
			audioSource.PlayOneShot (audioChapter [3], 0.5f);

		GameObject obj = objectChapter [3];
		if (obj) {
			obj.SetActive (true);
			Destroy (obj, soul.hero.yellowDuration * 9);
		}
	}

	public void Chapter5 () {
		if (audioSource && audioChapter [4])
			audioSource.PlayOneShot (audioChapter [4], 0.7f);

		GameObject obj = objectChapter [4];
		if (obj) {
			obj.SetActive (true);
			Destroy (obj, soul.hero.yellowDuration * 9);
		}
	}
}