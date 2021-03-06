﻿using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	float forceModifier = 500.0f;
	public GameObject harmfulSphere;
	public GameObject freezePartSysPrefab;
	public GameObject freezePowerupBreakingAudio;

	private Color myColor;
	private float lastSynchronizationTime = 0f;
	private float syncDelay = 0f;
	private float syncTime = 0f;
	private Vector3 syncStartPosition = Vector3.zero;
	private Vector3 syncVelocity = Vector3.zero;
	private Vector3 syncEndPosition = Vector3.zero;
	private Quaternion syncEndRotation = Quaternion.identity;
	private Quaternion syncStartRotation = Quaternion.identity;

	private bool powerupIsActive = false;
	private bool playerIsFrozen = false;
	float powerupAffectingTime = 5;
	private float timePassed = 0f;
	private int currentPowerupNumber;

	private const float lavaDamageInterval = 0.02f;
	private float lavaTimePassed = 0.0f;

	void OnCollisionEnter (Collision collision)
	{
			if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Player") {
			
				ContactPoint cp = collision.contacts [0];
				Vector3 oldVelocity = rigidbody.velocity;
				rigidbody.velocity = oldVelocity + cp.normal * collision.relativeVelocity.magnitude * 1.0f;
				if (collision.gameObject.tag == "Player"){
						audio.Play ();
				}

			}
	}

	void OnTriggerEnter(Collider collider){
		if (!powerupIsActive) {
			if (collider.gameObject.tag == "PowerupFreeze") {
				powerUpFreeze (collider);
				currentPowerupNumber = 1;
			}
			else if (collider.gameObject.tag == "PowerupHarmfulSphere") {
				powerUpHarmPlayers (collider);
				currentPowerupNumber = 2;
			}
			else if (collider.gameObject.tag == "PowerupGrow") {
				powerUpGrow ();
				currentPowerupNumber = 3;
			}
			else if (collider.gameObject.tag == "PowerupShrink") {
				powerUpShrink ();
				currentPowerupNumber = 4;
			}
			powerupIsActive = true;
		}
	} 

	void reversePowerup(){
		switch (currentPowerupNumber) {
		case 1:
			powerUpUnfreeze();
			break;
		case 2:
			break;
		case 3:
			powerUpShrink();
			break;
		case 4:
			powerUpGrow ();
			break;
		}
	}

	void powerUpShrink(){
		transform.localScale += new Vector3(-0.5f, -0.5f, -0.5f);
	}

	void powerUpHarmPlayers (Collider collider){
		Instantiate(harmfulSphere, collider.transform.position, collider.transform.rotation);
	}

	void powerUpFreeze(Collider collider){
			playerIsFrozen = true;
			GameObject freezPowerUp = (GameObject) Instantiate(freezePartSysPrefab, collider.transform.position, collider.transform.rotation);
			freezPowerUp.GetComponent<FreezePartSys> ().setFrozenPlayer (this.gameObject);
			myColor = renderer.material.GetColor ("_Color");
			renderer.material.color = new Color (0.6f, 0.6f, 1.0f, 0.6f);
		
	}

	void powerUpGrow(){
		transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
	}

	void powerUpUnfreeze(){
		playerIsFrozen = false;
		Instantiate(freezePowerupBreakingAudio, transform.position, Quaternion.identity);
		renderer.material.color = myColor;

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
				networkView.observed = this;
				lastSynchronizationTime = Time.time;
		}
		void Update(){

			if (powerupIsActive) {
			timePassed += Time.deltaTime;
				if(isItTime(powerupAffectingTime,timePassed)){
					timePassed = 0f;
					reversePowerup();
					powerupIsActive = false;
				}
			}


			// test if the player is actually standing over the terrain
			RaycastHit rayHit = new RaycastHit ();
			if (Physics.Raycast (transform.position, new Vector3(0,-1,0), out rayHit)) {
				if (rayHit.collider.gameObject.name == "Terrain") {
					try {
						AdjustPhysicsByTerrain ();
					} catch (System.Exception ex) {
						
					}			
				}
			}
			
		}

		bool isItTime(float threshold, float timePassed){
			if (timePassed > threshold) {
					return true;	
			} else {
				return false;
			}
		}
	
		void FixedUpdate ()
		{
				if (networkView.isMine) {

						InputMovement ();
				} else {
						SyncedMovement ();
				}
		}

		private void InputMovement()
		{
			float moveSideways = Input.GetAxis("Horizontal");
			float moveForward = Input.GetAxis ("Vertical");

			if (!playerIsFrozen) {
				// nudge the force position up by the diameter of the sphere to position it at the top,
				// adding a rolling force to the top of sphere gives a more realistic result.
				Vector3 forcePosition = transform.position + new Vector3 (0.0f, 0.5f, 0.0f);
				Vector3 forceDirection = new Vector3 (Camera.main.transform.right.x * moveSideways, 0.0f, Camera.main.transform.forward.z * moveForward);

				// normalize the direction so we get constant force in all directions
				forceDirection.Normalize ();

				// add a combined force with the calculated direction and position
				rigidbody.AddForceAtPosition (forceDirection * forceModifier * Time.deltaTime, forcePosition);
				// enable this to visualize the force position in real-time
				Debug.DrawRay (forcePosition, forceDirection * forceModifier * Time.deltaTime);
			}
			
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
			case 4:
				//lava, increase friction and do some damage
				rigidbody.angularDrag = 8.0f;
				forceModifier = 350.0f;
				PlayerHealthBar playerHealthBar = GetComponentInParent<PlayerHealthBar>();
				lavaTimePassed += Time.deltaTime;
				if (lavaTimePassed >= lavaDamageInterval) {
					lavaTimePassed = 0.0f;
					playerHealthBar.decrementHealth(1);
				}
				break;
			default:
				rigidbody.angularDrag = 5.0f;
				forceModifier = 500.0f;
				lavaTimePassed = 0.0f;
				break;
			}
		}
	
	public bool isPlayerFrozen(){
		return playerIsFrozen;
	}

}
