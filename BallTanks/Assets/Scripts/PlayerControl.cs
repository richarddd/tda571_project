using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	public float speed;



	void FixedUpdate() {
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		rigidbody.AddForce(movement * speed);
	}

	void OnCollisionEnter(Collision collision){

		//If the ball collides with a wall, reverse the movement
		if (collision.gameObject.tag == "Wall") {
			ContactPoint cp = collision.contacts[0];
			Vector3 oldVelocity = rigidbody.velocity;
			rigidbody.velocity = oldVelocity + cp.normal * 1.0f * oldVelocity.magnitude;
		}

	}


}
