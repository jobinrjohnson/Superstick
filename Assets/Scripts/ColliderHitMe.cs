using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderhitMe : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	void OnCollisionEnter(Collision collision) {

		if (StickManupulate.DOES_IT_HIT) {
			return;
		}

		ContactPoint contact = collision.contacts[0];
		Quaternion rot = Quaternion.FromToRotation (Vector3.up, contact.normal);
		Vector3 pos = contact.point;
		float normalisedScore = 2f - Mathf.Abs (pos.x);
		StickManupulate.addScore (normalisedScore);

		StickManupulate.DOES_IT_HIT = true;

		//print (pos);
		//Instantiate(explosionPrefab, pos, rot);
		//Destroy(gameObject);
	}

}
