using UnityEngine;
using System.Collections;

public class Boost : MonoBehaviour {
	public float boostFactor = 1F;

	void OnTriggerEnter(Collider other){
		if (other.tag == "Player" && other.rigidbody != null) {
			other.rigidbody.AddForce (transform.forward * boostFactor);
		}
	}
}
