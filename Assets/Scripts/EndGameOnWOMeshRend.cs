using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameOnWOMeshRend : MonoBehaviour {
	bool paused = true;

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

	void OnCollisionEnter (Collision collision) {
		if (collision.gameObject.tag.Equals ("Sticks")) {
			EndForNow ();
		}
	}

}
