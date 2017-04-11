using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeInfoCube : MonoBehaviour {

	public Transform backgroundCube; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


		float worldScreenHeight = (float)(Camera.main.orthographicSize * 2.0);
		float worldScreenWidth = (float)(worldScreenHeight / Screen.height * Screen.width * .5);
		worldScreenHeight *= .5f;

		gameObject.transform.localScale = new Vector3 (worldScreenWidth * 2, worldScreenHeight * 2, 1);

		
	}
}
