using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StickManupulate : MonoBehaviour {


	public struct objects {
		public GameObject baseObject;
		public GameObject left;
		public GameObject right;
		public Transform rayHere;
	};

	private static float MY_SCORE = 0;
	public static bool DOES_IT_HIT = false;
	public static int noOfClicksDown = 0;
	public static bool HIT_INTD = false;

	public static float addScore (float score) {
		MY_SCORE += score;
		return MY_SCORE;
	}	
	public static int getScore () {
		return Mathf.CeilToInt (MY_SCORE);
	}

	public Transform mainStack;
	public objects[] stack;
	public Text scoreText;
	public Button pauseButton;
	public Transform backgroundCube;

	private float translate = 30;

	private float worldScreenHeight;
	private float worldScreenWidth;

	private float screenFactor;
	private float baseWidth;

	private int totalScore = 0;

	private objects currentObject;
	private objects previousObject;


	private int controlSign = 1;
	private int objectPointer = 0;
	private int stackPositionNext = 0;



	void OnEndGame(){
		paused = true;

	}

	void OnRestartGame(){
		paused = false;
		Start ();

		for (int i = 0; i < mainStack.childCount; i++) {
			stack [i].baseObject.transform.localPosition = new Vector3 (0, 2 * i, 0);
			stack [i].baseObject.transform.localRotation = Quaternion.Euler (0, 0, 0);
			float translate = 45f;
			stack [i].left.transform.localRotation = Quaternion.Euler (0, 0, translate);
			stack [i].right.transform.localRotation = Quaternion.Euler (0, 0, 90 + translate);
			float cornerAngle = Mathf.Deg2Rad * (translate);
			stack [i].right.transform.localScale = new Vector3 (.1f, baseWidth * Mathf.Cos (cornerAngle) + .04f, .1f);
			stack [i].left.transform.localScale = new Vector3 (.1f, baseWidth * Mathf.Sin (cornerAngle) + .04f, .1f);
			stack [i].baseObject.transform.localRotation = Quaternion.Euler (0f, 0f, 0f);
		}

	}

	void PauseGame () {
		Object[] objects = FindObjectsOfType (typeof(GameObject));
		foreach (GameObject go in objects) {
			go.SendMessage ("OnPauseGame", SendMessageOptions.DontRequireReceiver);
		}
	}

	void ResumeGame () {
		Object[] objects = FindObjectsOfType (typeof(GameObject));
		foreach (GameObject go in objects) {
			go.SendMessage ("OnResumeGame", SendMessageOptions.DontRequireReceiver);
		}
	}

	bool paused = true;

	void OnPauseGame () {
		paused = true;
		MeshRenderer render = gameObject.GetComponentInChildren<MeshRenderer>();
		render.enabled = false;
	}

	void OnResumeGame () {
		paused = false;
	}

	void Start () {

		MY_SCORE = noOfClicksDown = objectPointer = stackPositionNext = 0;
		DOES_IT_HIT = HIT_INTD = false;

		Button btn = pauseButton.GetComponent<Button> ();

		stack = new objects[mainStack.childCount];
		for (int i = 0; i < mainStack.childCount; i++) {
			stack [i] = new objects ();
			stack [i].baseObject = mainStack.GetChild (i).gameObject;
			for (int j = 0; j < stack [i].baseObject.transform.childCount; j++) {
				if (stack [i].baseObject.transform.GetChild (j).tag == "Left") {
					stack [i].left = stack [i].baseObject.transform.GetChild (j).gameObject;
					stack [i].rayHere = stack [i].left.transform.GetChild (0);
				} else if (stack [i].baseObject.transform.GetChild (j).tag == "Right") {
					stack [i].right = stack [i].baseObject.transform.GetChild (j).gameObject;
				}
			}

			Color c = Random.ColorHSV ();
			Destroy (stack [i].baseObject.GetComponent<Rigidbody> ());
			stack [i].left.GetComponent<Renderer> ().material.color = c;
			stack [i].right.GetComponent<Renderer> ().material.color = c;
		}
		currentObject = stack [objectPointer];

		mainStack.localPosition = new Vector3 (0, 0, 0);
	}



	private void updateBases () {
		worldScreenHeight = (float)(Camera.main.orthographicSize * 2.0);
		worldScreenWidth = (float)(worldScreenHeight / Screen.height * Screen.width * .5);
		screenFactor = worldScreenHeight / Screen.height;
		worldScreenHeight *= .5f;

		Vector3 ls = currentObject.left.transform.localPosition;
		ls.x = -1 * worldScreenWidth / 2 - 1.8f;
		currentObject.left.transform.localPosition = ls;

		ls = currentObject.right.transform.localPosition;
		ls.x = worldScreenWidth / 2 + 1.8f;
		currentObject.right.transform.localPosition = ls;

		baseWidth = currentObject.right.transform.localPosition.x - currentObject.left.transform.localPosition.x;

		backgroundCube.localScale = new Vector3 (worldScreenWidth * 2, worldScreenHeight * 2, 1);
	}

	private void incrementQueue () {

		Destroy (previousObject.baseObject.GetComponent<Rigidbody> ());
		float translate = 45f;
		previousObject.left.transform.localRotation = Quaternion.Euler (0, 0, translate);
		previousObject.right.transform.localRotation = Quaternion.Euler (0, 0, 90 + translate);
		float cornerAngle = Mathf.Deg2Rad * (translate);
		previousObject.right.transform.localScale = new Vector3 (.1f, baseWidth * Mathf.Cos (cornerAngle) + .04f, .1f);
		previousObject.left.transform.localScale = new Vector3 (.1f, baseWidth * Mathf.Sin (cornerAngle) + .04f, .1f);

		previousObject.baseObject.transform.localRotation = Quaternion.Euler (0f, 0f, 0f);
		previousObject.baseObject.transform.localPosition = new Vector3 (0f, noOfClicksDown * 2f + (stack.Length - 1) * 2, 0f);
		DOES_IT_HIT = false;
		HIT_INTD = false;


		Color c = Random.ColorHSV ();
		previousObject.left.GetComponent<Renderer> ().material.color = c;
		previousObject.right.GetComponent<Renderer> ().material.color = c;

	}


	void Update () {
		if (!paused) {
			scoreText.text = Mathf.CeilToInt (MY_SCORE) + "";

			if (DOES_IT_HIT && previousObject.baseObject != null) {
				incrementQueue ();
			}

			Vector3 stackposition = mainStack.transform.localPosition;
			if (stackposition.y > stackPositionNext) {
				stackposition.y -= Time.deltaTime * 5;
				mainStack.transform.localPosition = stackposition;
			}

			currentObject = stack [objectPointer];
			updateBases ();

			translate += Time.deltaTime * 5 * controlSign * (totalScore == 0 ? 1 : totalScore);

			if (translate < 30) {
				controlSign = 1;
			} else if (translate > 60) {
				controlSign = -1;
			}

			currentObject.left.transform.localRotation = Quaternion.Euler (0, 0, translate);
			currentObject.right.transform.localRotation = Quaternion.Euler (0, 0, 90 + translate);

			float cornerAngle = Mathf.Deg2Rad * (translate);

			currentObject.right.transform.localScale = new Vector3 (.1f, baseWidth * Mathf.Cos (cornerAngle) + .04f, .1f);
			currentObject.left.transform.localScale = new Vector3 (.1f, baseWidth * Mathf.Sin (cornerAngle) + .04f, .1f);
		}

		if (Input.GetMouseButtonDown (0) && !HIT_INTD) {

			bool isMouseOnPause = (Input.mousePosition.x <= pauseButton.transform.position.x + 30f && Input.mousePosition.x >= pauseButton.transform.position.x - 30f) &&
			                      (Input.mousePosition.y <= pauseButton.transform.position.y + 30f && Input.mousePosition.y >= pauseButton.transform.position.y - 30f);

			if (isMouseOnPause) {
				if (paused) {
					ResumeGame ();
				} else {
					PauseGame ();
				}
				return;
			}

			if (paused) {
				return;
			}
			controlSign *= -1;
			noOfClicksDown++;
			stackPositionNext -= 2;
			previousObject = currentObject;
			objectPointer = objectPointer + 1 >= stack.Length ? 0 : objectPointer + 1;
			DOES_IT_HIT = false;
			HIT_INTD = true;
			currentObject.baseObject.AddComponent<Rigidbody> ();
			translate = 45;

		}

	}

}
