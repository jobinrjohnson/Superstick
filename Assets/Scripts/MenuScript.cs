using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

	public Button resumeButton;
	public Transform MenuPanel,DarkPanel;
	public Text scoreText;

	bool paused = true;
	bool ended = false;

	void OnPauseGame () {
		ShowPanel ();
		paused = true;
	}

	void ShowPanel(){

		scoreText.text = StickManupulate.getScore () + "";
		MenuPanel.gameObject.SetActive (true);

	}

	void OnResumeGame () {
		if (ended) {
			Scene scene = SceneManager.GetActiveScene();
			SceneManager.LoadScene(scene.name);
		} else {
			MenuPanel.gameObject.SetActive (false);
			resumeButton.GetComponentInChildren<Text> ().text = "Resume";
			paused = false;
		}
	}

	void OnEndGame(){
		ShowPanel ();
		resumeButton.GetComponentInChildren<Text>().text = "Restart";
		paused = ended = true;
	}

	void ResumeMyGame(){
		Object[] objects = FindObjectsOfType (typeof(GameObject));
		foreach (GameObject go in objects) {
			go.SendMessage ("OnResumeGame", SendMessageOptions.DontRequireReceiver);
		}
	}
		
	void Start () {
		resumeButton.onClick.AddListener(ResumeMyGame);
		MenuPanel.gameObject.SetActive (true);
	}

	void Update () {
		
	}
}
