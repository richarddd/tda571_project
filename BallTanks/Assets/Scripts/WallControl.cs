using UnityEngine;
using System.Collections;

public class WallControl : MonoBehaviour {

	public GameObject explosion;

	void OnCollisionEnter(Collision collision){
		if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Projectile") {
			gameObject.SetActive (false);
			Instantiate(explosion, transform.position, transform.rotation);
		}
		
	}

}
