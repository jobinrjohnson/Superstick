using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBases : MonoBehaviour {

	private float baseSpeed = .5f;
	GameObject[] bases;

	public Transform mbase;

	bool paused = true;


	void OnRestartGame () {
		paused = false;
		baseSpeed = .5f;
	}

	void OnPauseGame () {
		paused = true;
	}

	void OnResumeGame () {
		paused = false;
	}

	void OnEndGame () {
		paused = true;
	}

	void EndForNow () {
		Object[] objects = FindObjectsOfType (typeof(GameObject));
		foreach (GameObject go in objects) {
			go.SendMessage ("OnEndGame", SendMessageOptions.DontRequireReceiver);
		}
	}

	void Start () {
		bases = new GameObject[3];
		for (int i = 0; i < mbase.childCount; i++) {
			bases [i] = mbase.GetChild (i).gameObject;
		}
	}

	void Update () {
		if (paused) {
			return;
		}


		float worldScreenWidth = (float)((Camera.main.orthographicSize * 2.0) / Screen.height * Screen.width * .5) * .5f;
		float time = Time.deltaTime * baseSpeed;
		for (int i = 0; i < mbase.childCount; i++) {
			float x = bases [i].transform.localPosition.y + time;
			if (!((bases [i].transform.localPosition.y + time) < worldScreenWidth)) {

				if (bases [i].GetComponent<MeshRenderer> ().enabled == true) {
					EndForNow ();
				}

				bases [i].transform.localScale = new Vector3 (bases [i].transform.localScale.x, 1 - baseSpeed / 3, bases [i].transform.localScale.z);

				x = (mbase.childCount - 1) * -1.5f;
				bases [i].GetComponent<MeshRenderer> ().enabled = true;
				baseSpeed += baseSpeed < 1 ? .05f : 0.009f;
				Time.timeScale = 1f + baseSpeed - .5f;

				//print (baseSpeed);

			}
			bases [i].transform.localPosition = new Vector3 (0, x, 0f);
		}
	}

}
