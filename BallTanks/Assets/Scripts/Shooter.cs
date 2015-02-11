using UnityEngine;
using System.Collections;

public class Shooter : MonoBehaviour {

	public Rigidbody projectile;
	public float shotForce = 1000f;
	public float moveSpeed = 10f;

	void Update () {
		
		if (Input.GetButtonUp ("Fire1")) {
			Rigidbody shot = Instantiate (projectile, transform.position + (new Vector3(0.15f,0,1)), transform.rotation) as Rigidbody;
			shot.AddForce (transform.up * -1 * shotForce);
		}
	}
}
