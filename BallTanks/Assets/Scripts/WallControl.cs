using UnityEngine;
using System.Collections;

public class WallControl : MonoBehaviour {

	public GameObject explosion;

	void OnCollisionEnter(Collision collision){
		if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Projectile") {
			Network.Destroy(gameObject);
			Network.Instantiate(explosion, transform.position, transform.rotation,0);
		}
	}

	void OnTriggerEnter(Collider collider)
	{
		if(collider.gameObject.tag == "Killzone")
		{
			Network.Destroy(gameObject);
		}
	}
}
