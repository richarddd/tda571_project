using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{

		//public float maxSpeed;

		private float lastSynchronizationTime = 0f;
		private float syncDelay = 0f;
		private float syncTime = 0f;
		private Vector3 syncStartPosition = Vector3.zero;
		private Vector3 syncVelocity = Vector3.zero;
		private Vector3 syncEndPosition = Vector3.zero;

	
		void OnCollisionEnter (Collision collision)
		{
		
				//If the ball collides with a wall, reverse the movement
				if (collision.gameObject.tag == "Wall") {
						ContactPoint cp = collision.contacts [0];
						Vector3 oldVelocity = rigidbody.velocity;
						rigidbody.velocity = oldVelocity + cp.normal * 1.0f * oldVelocity.magnitude;
				}
		
		}
		// Use this for initialization
		void Start ()
		{
	
		}

		void OnSerializeNetworkView (BitStream stream, NetworkMessageInfo info)
		{
				Vector3 syncPosition = Vector3.zero;
				if (stream.isWriting) {
						syncPosition = rigidbody.position;
						stream.Serialize (ref syncPosition);
			
						syncVelocity = rigidbody.velocity;
						stream.Serialize (ref syncVelocity);
				} else {
						stream.Serialize (ref syncPosition);
						stream.Serialize (ref syncVelocity);
			
						syncTime = 0f;
						syncDelay = Time.time - lastSynchronizationTime;
						lastSynchronizationTime = Time.time;
			
			
						syncEndPosition = syncPosition + syncVelocity * syncDelay;
						syncStartPosition = rigidbody.position;
				}
		}

		void Awake ()
		{
				//networkview = gameObject.GetComponent<NetworkView> ();
				//rigidbody = gameObject.GetComponent<Rigidbody> ();
				networkView.observed = this;
				lastSynchronizationTime = Time.time;

				if (networkView.isMine) {
						Camera.main.GetComponent<SmoothFollow> ().setTarget (gameObject);
				}
		}
	
		void FixedUpdate ()
		{
				if (networkView.isMine) {

						InputMovement ();
						//InputColorChange();
				} else {
						SyncedMovement ();
				}
		}

		private void InputMovement ()
		{
				float moveHorizontal = Input.GetAxis ("Horizontal");
				float moveVertical = Input.GetAxis ("Vertical");

				rigidbody.AddForce (Camera.main.transform.forward * moveVertical * 1000f * Time.deltaTime);
				rigidbody.AddForce (Camera.main.transform.right * moveHorizontal * 1000f * Time.deltaTime);


		}

		void OnCollisionStay (Collision collisionInfo)
		{
				rigidbody.velocity = rigidbody.velocity * 0.95f;

		}
		
		private void SyncedMovement ()
		{
				syncTime += Time.deltaTime;
				rigidbody.position = Vector3.Lerp (syncStartPosition, syncEndPosition, syncTime / syncDelay);
				rigidbody.velocity = syncVelocity;
		}
	


}
