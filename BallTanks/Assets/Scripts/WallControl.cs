using UnityEngine;
using System.Collections;

public class WallControl : MonoBehaviour {

	public GameObject explosion;

	void OnCollisionEnter(Collision collision){
		if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Projectile") {
			Network.Destroy(gameObject);
			Instantiate(explosion, transform.position, transform.rotation);
		}

		
	}

}
