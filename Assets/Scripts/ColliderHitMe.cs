using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderhitMe : MonoBehaviour
{

	void Start ()
	{
		
	}

	void Update ()
	{
		
	}

	public Transform explosionPrefab;
	private Transform explosive = null;

	IEnumerator destroyExplosive ()
	{
		yield return new WaitForSeconds (2);
		if (explosive != null) {
			Destroy (explosive.gameObject);
		}
	}

	void OnCollisionEnter (Collision collision)
	{

		if (StickManupulate.DOES_IT_HIT || gameObject.GetComponent<MeshRenderer> ().enabled == false) {
			StickManupulate.DOES_IT_HIT = true;
			return;
		}

		ContactPoint contact = collision.contacts [0];
		Quaternion rot = Quaternion.FromToRotation (Vector3.up, contact.normal);
		Vector3 pos = contact.point;

		float normalisedScore = 2f - Mathf.Abs (pos.x);
		StickManupulate.addScore (normalisedScore * 2);
		StickManupulate.DOES_IT_HIT = true;

		gameObject.GetComponent<MeshRenderer> ().enabled = false;
		Destroy (collision.gameObject.GetComponent<Rigidbody> ());
		explosive = Instantiate (explosionPrefab, pos, rot);
		StartCoroutine (destroyExplosive ());
	}

}
