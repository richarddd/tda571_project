using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	public float maxSpeed;
	public GameObject camera;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate() {
		float moveHorizental = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		//trying to get camera rotation smoother
		int h = (int) moveHorizental;
		int v = (int)  moveVertical;

	
		if (rigidbody.velocity.magnitude < maxSpeed) {
			rigidbody.AddForce (camera.transform.forward * v);
			rigidbody.AddForce (camera.transform.right * h);
		}





	}
}
