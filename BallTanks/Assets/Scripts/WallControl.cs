using UnityEngine;
using System.Collections;

public class WallControl : MonoBehaviour {

	public GameObject explosion;

	void OnCollisionEnter(Collision collision){
		if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Projectile") {
			Destroy(gameObject);
			Instantiate(explosion, transform.position, transform.rotation);
		}
	}

	void OnTriggerEnter(Collider collider)
	{
		if(collider.gameObject.tag == "Killzone")
		{
			Destroy(gameObject);
		}
	}
}
