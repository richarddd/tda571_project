using UnityEngine;
using System.Collections;

public class PlayerControlOffline : MonoBehaviour {

	//public float maxSpeed;

	// Use this for initialization
	void Start () {
	
	}

	void FixedUpdate()
	{
		InputMovement();
	}

	private void InputMovement()
	{
		float moveHorizontal = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		// nudge the force position up by the diameter of the sphere to position it at the top.
		// adding a rolling force to the top of sphere gives a more realistic result.
		Vector3 forcePosition = new Vector3 (transform.position.x, transform.position.y + 0.5f, transform.position.z);

		rigidbody.AddForceAtPosition (Camera.main.transform.forward * moveVertical*500f*Time.deltaTime, forcePosition);
		rigidbody.AddForceAtPosition (Camera.main.transform.right * moveHorizontal*500f*Time.deltaTime, forcePosition);

		// enable this to visualize the force position in real-time
		Debug.DrawRay (forcePosition, Camera.main.transform.right);
	}

	// removed this function in order to rely on the rigidObject's angularDrag parameter.
//	void OnCollisionStay(Collision collisionInfo) {
//		rigidbody.velocity = rigidbody.velocity;// * 0.95f;
//
//	}

}
