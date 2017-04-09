using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {

	public Button resumeButton, optional_btn, exitButton;
	public Transform MenuPanel, DarkPanel;
	public Text scoreText;

	public Sprite restartSprite, infoSprite;

	bool paused = true;
	bool ended = false;
	bool firsttime = true;

	void OnPauseGame () {
		ShowPanel ();
		paused = true;
		optional_btn.GetComponent<Image> ().sprite = restartSprite;
	}

	void ShowPanel () {

		scoreText.text = StickManupulate.getScore () + "";
		MenuPanel.gameObject.SetActive (true);

	}

	void OnRestartGame () {
		paused = ended = false;
		MenuPanel.gameObject.SetActive (false);
	}

	void OnResumeGame () {
		if (ended) {
			restart ();
		} else {
			MenuPanel.gameObject.SetActive (false);
			resumeButton.GetComponentInChildren<Text> ().text = "";
			paused = false;
		}
	}

	void OnEndGame () {
		ShowPanel ();
		resumeButton.GetComponentInChildren<Text> ().text = "";
		paused = ended = true;

		optional_btn.GetComponent<Image> ().sprite = infoSprite;

	}

	void ResumeMyGame () {
		firsttime = false;
		Object[] objects = FindObjectsOfType (typeof(GameObject));
		foreach (GameObject go in objects) {
			go.SendMessage ("OnResumeGame", SendMessageOptions.DontRequireReceiver);
		}
	}

	void restart () {
		Object[] objects = FindObjectsOfType (typeof(GameObject));
		foreach (GameObject go in objects) {
			go.SendMessage ("OnRestartGame", SendMessageOptions.DontRequireReceiver);
		}
	}

	void RestartOrInfoClicked () {
		if (ended || firsttime) {
			Application.OpenURL ("http://www.jobinrjohnson.in");
		} else {
			restart ();
		}
	}

	void Exit () {
		Application.Quit ();
	}

	void Start () {
		resumeButton.onClick.AddListener (ResumeMyGame);
		MenuPanel.gameObject.SetActive (true);
		optional_btn.onClick.AddListener (RestartOrInfoClicked);
		exitButton.onClick.AddListener (Exit);
	}

	void Update () {
		
	}
}
