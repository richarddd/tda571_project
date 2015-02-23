using UnityEngine;
using System.Collections;

public class FreezePowerupManager : MonoBehaviour {
	public GameObject myAudio;

	void OnTriggerEnter (Collider collider) {
		if (collider.gameObject.tag == "Player") {

			Network.Instantiate(myAudio, transform.position, transform.rotation,0);
			Network.Destroy(gameObject);
		}
	}
}