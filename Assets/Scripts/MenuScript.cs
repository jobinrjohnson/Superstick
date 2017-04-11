using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;

public class MenuScript : MonoBehaviour {

	public Button resumeButton, optional_btn, exitButton;
	public Transform MenuPanel;
	public Text scoreText, extraText;

	public Sprite restartSprite, infoSprite;

	bool paused = true;
	bool ended = false;
	bool firsttime = true;
	public static int HIGH_SCORE = 0;
	private InterstitialAd interstitial;


	private void RequestInterstitial () {
		#if UNITY_ANDROID
		string adUnitId = "ca-app-pub-6479565236772083/3187905053";
		#elif UNITY_IPHONE
		string adUnitId = "INSERT_IOS_INTERSTITIAL_AD_UNIT_ID_HERE";
		#else
		string adUnitId = "unexpected_platform";
		#endif

		interstitial = new InterstitialAd (adUnitId);
		AdRequest request = new AdRequest.Builder ().Build ();
		interstitial.LoadAd (request);
	}


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
		if (interstitial.IsLoaded ()) {
			interstitial.Show ();
          	AdRequest request = new AdRequest.Builder ().Build ();
			interstitial.LoadAd (request);
		}

		if (HIGH_SCORE < StickManupulate.getScore ()) {
			PlayerPrefs.SetInt ("highscore", StickManupulate.getScore ());
			HIGH_SCORE = StickManupulate.getScore ();
			extraText.text = "New High Scores";
		} else {
			extraText.text = "High Score : " + HIGH_SCORE;
		}

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
		RequestInterstitial ();
		resumeButton.onClick.AddListener (ResumeMyGame);
		MenuPanel.gameObject.SetActive (true);
		optional_btn.onClick.AddListener (RestartOrInfoClicked);
		exitButton.onClick.AddListener (Exit);

		HIGH_SCORE =	PlayerPrefs.GetInt ("highscore");
		scoreText.text = HIGH_SCORE + "";
		extraText.text = "High Score";
	}

	void Update () {
		
	}
}
