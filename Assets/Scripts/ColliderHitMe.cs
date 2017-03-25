using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderHitMe : MonoBehaviour {

	GameObject[] bases;

	public Transform mbase;

	// Use this for initialization
	void Start () {
		bases = new GameObject[3];
		for (int i = 0; i < mbase.childCount; i++) {
			bases [i] = mbase.GetChild (i).gameObject;
		}
		
	}

	
	// Update is called once per frame
	void Update () {

		float worldScreenWidth = (float)((Camera.main.orthographicSize * 2.0) / Screen.height * Screen.width * .5) * .5f;
		float time = Time.deltaTime * .5f;
		for (int i = 0; i < mbase.childCount; i++) {
			bases [i].transform.localPosition = new Vector3 (
				0, 
				(bases [i].transform.localPosition.y + time) < worldScreenWidth ? (bases [i].transform.localPosition.y + time) : (mbase.childCount - 1) * -1.5f, 
				0f
			);
		}
	}



	//public Transform explosionPrefab;

	void OnCollisionEnter(Collision collision) {
		ContactPoint contact = collision.contacts[0];
		Quaternion rot = Quaternion.FromToRotation (Vector3.up, contact.normal);
		Vector3 pos = contact.point;
		//Instantiate(explosionPrefab, pos, rot);
		Destroy(gameObject);
	}


}
