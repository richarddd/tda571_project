using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{

		//public float maxSpeed;
	public float forceModifier = 500.0f;
	public GameObject harmfulSphere;
	public GameObject freezePartSysPrefab;
	private float lastSynchronizationTime = 0f;
	private float syncDelay = 0f;
	private float syncTime = 0f;
	private Vector3 syncStartPosition = Vector3.zero;
	private Vector3 syncVelocity = Vector3.zero;
	private Vector3 syncEndPosition = Vector3.zero;
	private Quaternion syncEndRotation = Quaternion.identity;
	private Quaternion syncStartRotation = Quaternion.identity;

	private bool playerIsFrozen = false;
	public float frozenTimeInterval = 5;
	private float timePassed = 0f;
	
	void OnCollisionEnter (Collision collision)
	{
			if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Player") {
			
				ContactPoint cp = collision.contacts [0];
				Vector3 oldVelocity = rigidbody.velocity;
				rigidbody.velocity = oldVelocity + cp.normal * collision.relativeVelocity.magnitude * 1.0f;
			}
	}

	void OnTriggerEnter(Collider collider){
		if (collider.gameObject.tag == "Powerup") {
			powerUpFreeze(collider);
			//powerUpHarmPlayers(collider);
		}
	} 

	void powerUpHarmPlayers (Collider collider){
		Network.Instantiate(harmfulSphere, collider.transform.position, collider.transform.rotation,0);
	}

	void powerUpFreeze(Collider collider){
		playerIsFrozen = true;
		GameObject freezPowerUp = (GameObject) Network.Instantiate(freezePartSysPrefab, collider.transform.position, collider.transform.rotation,0);
		freezPowerUp.GetComponent<FreezePartSys> ().setFrozenPlayer (this.gameObject);
	}

		void OnSerializeNetworkView (BitStream stream, NetworkMessageInfo info)
		{
				Vector3 syncPosition = Vector3.zero;
				Quaternion syncRotation = Quaternion.identity;
				if (stream.isWriting) {
						syncRotation = rigidbody.rotation;
						stream.Serialize (ref syncRotation);

						syncPosition = rigidbody.position;
						stream.Serialize (ref syncPosition);
			
						syncVelocity = rigidbody.velocity;
						stream.Serialize (ref syncVelocity);
				} else {
						stream.Serialize (ref syncRotation);
						stream.Serialize (ref syncPosition);
						stream.Serialize (ref syncVelocity);
						
			
						syncTime = 0f;
						syncDelay = Time.time - lastSynchronizationTime;
						lastSynchronizationTime = Time.time;
			
			
						syncEndPosition = syncPosition + syncVelocity * syncDelay;
						syncStartPosition = rigidbody.position;
						syncEndRotation = syncRotation;
						syncStartRotation = rigidbody.rotation;
				}
		}

		void Awake ()
		{
				//networkview = gameObject.GetComponent<NetworkView> ();
				//rigidbody = gameObject.GetComponent<Rigidbody> ();
				networkView.observed = this;
				lastSynchronizationTime = Time.time;

				if (networkView.isMine) {
						//Camera.main.GetComponent<SmoothFollow> ().setTarget (gameObject);
				}
		}
		void Update(){
			if (playerIsFrozen) {
				timePassed += Time.deltaTime;
				if(timePassed > frozenTimeInterval){
					timePassed = 0f;
					playerIsFrozen = false;
				}
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

		private void InputMovement()
	{
		float moveSideways = Input.GetAxis("Horizontal");
		float moveForward = Input.GetAxis ("Vertical");

		if (!playerIsFrozen) {

			// test if the player is actually standing over the terrain
			RaycastHit rayHit = new RaycastHit();
			if (Physics.Raycast (transform.position, -transform.up, out rayHit))
			{
				if(rayHit.collider.gameObject.name == "Terrain")
				{
					AdjustPhysicsByTerrain ();
				}
			}

			// nudge the force position up by the diameter of the sphere to position it at the top,
			// adding a rolling force to the top of sphere gives a more realistic result.
			Vector3 forcePosition = transform.position + new Vector3 (0.0f, 0.5f, 0.0f);
			Vector3 forceDirection = new Vector3 (Camera.main.transform.right.x * moveSideways, 0.0f, Camera.main.transform.forward.z * moveForward);

			// normalize the direction so we get constant force in all directions
			forceDirection.Normalize ();

			// add a combined force with the calculated direction and position
			rigidbody.AddForceAtPosition (forceDirection * forceModifier * Time.deltaTime, forcePosition);
			Debug.DrawRay (forcePosition, forceDirection * forceModifier * Time.deltaTime);
		}

		// enable this to visualize the force position in real-time
		
	}


		void OnCollisionStay (Collision collisionInfo)
		{
				//rigidbody.velocity = rigidbody.velocity * 0.95f;

		}
		
		
		private void SyncedMovement ()
		{
				syncTime += Time.deltaTime;
				rigidbody.position = Vector3.Lerp (syncStartPosition, syncEndPosition, syncTime / syncDelay);
				rigidbody.velocity = syncVelocity;
				rigidbody.rotation = Quaternion.Slerp(syncStartRotation, syncEndRotation, syncTime / syncDelay);
		}

		void AdjustPhysicsByTerrain ()
	{
		// adjust force modifier and angular drag according to texture index
		TextureDetector td = GetComponent<TextureDetector> ();
		int texture = td.GetMainTexture (transform.position);
		switch (texture) {
		case 0:
			//normal terrain, set normal friction
			rigidbody.angularDrag = 5.0f;
			forceModifier = 500.0f;
			break;
		case 1:
			//dry terrain, increase friction
			rigidbody.angularDrag = 12.0f;
			forceModifier = 300.0f;
			break;
		case 2:
			//lava, increase friction and do some damage
			rigidbody.angularDrag = 24.0f;
			forceModifier = 150.0f;
			break;
		default:
			rigidbody.angularDrag = 1.0f;
			forceModifier = 500.0f;
			break;
		}
	}
	


}
