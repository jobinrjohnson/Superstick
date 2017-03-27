using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBases : MonoBehaviour
{

	GameObject[] bases;

	public Transform mbase;

	bool paused = true;

	void OnPauseGame () {
		paused = true;
	}

	void OnResumeGame () {
		paused = false;
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
		float time = Time.deltaTime * .4f;
		for (int i = 0; i < mbase.childCount; i++) {
			float x = bases [i].transform.localPosition.y + time;
			if (!((bases [i].transform.localPosition.y + time) < worldScreenWidth)) {
				x = (mbase.childCount - 1) * -1.5f;
				bases [i].GetComponent<MeshRenderer> ().enabled = true;
			}
			bases [i].transform.localPosition = new Vector3 (0, x, 0f);
		}
	}

}
