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

	private Vector3[] localPositions;

	public Transform mainStack;
	public objects[] stack;

	public Text scoreText;

	private float translate = 20;

	private float worldScreenHeight;
	private float worldScreenWidth;

	private float screenFactor;
	private float baseWidth;

	private int totalScore = 0;

	private objects currentObject;


	void Start ()
	{

		stack = new objects[3];
		localPositions = new Vector3[3];


		for (int i = 0; i < mainStack.childCount; i++) {
			stack [i] = new objects ();
			stack [i].baseObject = mainStack.GetChild (i).gameObject;
			for (int j = 0; j < stack [i].baseObject.transform.childCount; j++) {
				localPositions [i] = stack [i].baseObject.transform.localPosition;
				if (stack [i].baseObject.transform.GetChild (j).tag == "Left") {
					stack [i].left = stack [i].baseObject.transform.GetChild (j).gameObject;
					stack [i].rayHere = stack [i].left.transform.GetChild (0);
				} else if (stack [i].baseObject.transform.GetChild (j).tag == "Right") {
					stack [i].right = stack [i].baseObject.transform.GetChild (j).gameObject;
				}
			}
		}

		currentObject = stack[objectPointer];

	}

	private int controlSign = 1;

	private int objectPointer = 1;


	private void updateBases(){


		worldScreenHeight = (float)(Camera.main.orthographicSize * 2.0);
		worldScreenWidth = (float)(worldScreenHeight / Screen.height * Screen.width * .5);
		screenFactor = worldScreenHeight / Screen.height;
		worldScreenHeight *= .5f;

		//Vector3 ls = currentObject.tbase.localScale;
		//ls.y = worldScreenWidth;
		//tbase.localScale = ls;

		Vector3 ls = currentObject.left.transform.localPosition;
		ls.x = -1 * worldScreenWidth / 2 - 1.8f;
		currentObject.left.transform.localPosition = ls;

		ls = currentObject.right.transform.localPosition;
		ls.x = worldScreenWidth / 2 + 1.8f;
		currentObject.right.transform.localPosition = ls;

		baseWidth = currentObject.right.transform.localPosition.x - currentObject.left.transform.localPosition.x;
	}

	private IEnumerator incrementQueue(){

		yield return new WaitForSeconds(3);
		Destroy(currentObject.baseObject.gameObject.GetComponent<Rigidbody>());
		currentObject.baseObject.transform.localRotation =  Quaternion.Euler (0f, 0f, 0f);
		objectPointer = objectPointer - 1 < 0 ? stack.Length - 1 : objectPointer - 1;

		Vector3 Object0Lp = stack [0].baseObject.transform.localPosition;;

		for (int i = 0; i < stack.Length; i++) {
			if (i == stack.Length - 1) {
				stack [i].baseObject.transform.localPosition = Object0Lp;
				break;
			}
			stack [i].baseObject.transform.localPosition = localPositions [i + 1];
		}

	}


	void Update ()
	{


		currentObject = stack[objectPointer];
		updateBases ();

		translate += Time.deltaTime * 5 * controlSign*(totalScore==0?1:totalScore);

		if (translate < 20) {
			controlSign = 1;
		}else if(translate > 70) {
			controlSign = -1;
		}

		currentObject.left.transform.localRotation = Quaternion.Euler (0, 0, translate);
		currentObject.right.transform.localRotation = Quaternion.Euler (0, 0, 90 + translate);

		float cornerAngle = Mathf.Deg2Rad * (translate);

		currentObject.right.transform.localScale = new Vector3 (.1f, baseWidth * Mathf.Cos (cornerAngle) + .04f, .1f);
		currentObject.left.transform.localScale = new Vector3 (.1f, baseWidth * Mathf.Sin (cornerAngle) + .04f, .1f);


		if (Input.GetMouseButtonDown (0)) {
			Vector3 mousePositionInWorldPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			if (mousePositionInWorldPoint.x <= currentObject.rayHere.position.x + 0.2f && mousePositionInWorldPoint.x >= currentObject.rayHere.position.x - 0.2f) {

				currentObject.baseObject.AddComponent<Rigidbody> ();
				StartCoroutine(incrementQueue());
				scoreText.text = (++totalScore) + "";

			}

		}

	}
}
