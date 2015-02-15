using UnityEngine;
using System.Collections;

public class FreezePowerupManager : MonoBehaviour {

	void OnTriggerEnter (Collider collider) {
		//Todo: Send message to player that he is frozen
		if (collider.gameObject.tag == "Player") {
			Destroy (gameObject);
		}
	}
}
