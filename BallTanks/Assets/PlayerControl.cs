using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	public float speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {
		float moveHorizental = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizental, 0.0f, moveVertical);

		rigidbody.AddForce(movement * speed);
	}
}
