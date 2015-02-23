using UnityEngine;
using System.Collections;

public class PowerupCollisionManager : MonoBehaviour {

	void OnTriggerEnter(Collider collider){
		if (collider.gameObject.tag == "Player" || collider.gameObject.tag == "Wall" )
		Debug.Log ("Powerup Is triggerdng");
	}

	void OnCollisionEnter(Collision collider){
		if (collider.gameObject.tag == "Player" || collider.gameObject.tag == "Wall" )
			Debug.Log ("Powerup Is collding");
	}
}
