using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StickManupulate : MonoBehaviour
{


	public struct objects
	{
		public GameObject baseObject;
		public GameObject left;
		public GameObject right;
		public Transform rayHere;
	};

	private static float MY_SCORE = 0;
	public static bool DOES_IT_HIT = false;
	public static int noOfClicksDown = 0;
	public static bool HIT_INTD = false;

	public static float addScore (float score)
	{
		MY_SCORE += score;
		return MY_SCORE;
	}

	public Transform mainStack;
	public objects[] stack;
	public Text scoreText;

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

	void Start ()
	{
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
		}
		currentObject = stack [objectPointer];
	}



	private void updateBases ()
	{
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
	}

	private void incrementQueue ()
	{

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

	}


	void Update ()
	{

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


		if (Input.GetMouseButtonDown (0) && !HIT_INTD) {
			//Vector3 mousePositionInWorldPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			//if (mousePositionInWorldPoint.x <= currentObject.rayHere.position.x + 0.2f && mousePositionInWorldPoint.x >= currentObject.rayHere.position.x - 0.2f) {
			noOfClicksDown++;
			stackPositionNext -= 2;
			previousObject = currentObject;
			objectPointer = objectPointer + 1 >= stack.Length ? 0 : objectPointer + 1;
			DOES_IT_HIT = false;
			HIT_INTD = true;
			currentObject.baseObject.AddComponent<Rigidbody> ();

			translate = 45;
			//}

		}

	}

}
