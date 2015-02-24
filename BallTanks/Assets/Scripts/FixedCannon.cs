﻿using UnityEngine;
using System.Collections;

public class FixedCannon : MonoBehaviour
{

	public Rigidbody projectile;
	//public float shotForce = 50000f;
	//public float maxShotForce = 150000f;
	
	public float shotVelocity = 10f;
	public float maxShotVelocity = 60f;

		private bool shotFired = false;

		private float lastSynchronizationTime = 0f;
		private float syncDelay = 0f;
		private float syncTime = 0f;

		private float syncEndShotVelocity = 0f;
		private float syncStartShotVelocity = 0f;

		private LineRenderer lineRenderer;

		// Use this for initialization
		void Start ()
		{
				
		}

		void Awake ()
		{
				lastSynchronizationTime = Time.time;
				networkView.observed = this;
				lineRenderer = GetComponent<LineRenderer>();
				if(!networkView.isMine){
					lineRenderer.enabled = false;
				}
		}

		void FireShot ()
		{
			Rigidbody shot = Instantiate (projectile, this.transform.position, this.transform.rotation) as Rigidbody;
		//shot.AddForce (transform.TransformDirection(transform.forward) * shotForce * Time.deltaTime * -1);
			shot.rigidbody.velocity = ((transform.position - transform.parent.position) * shotVelocity);
		}

		void OnSerializeNetworkView (BitStream stream, NetworkMessageInfo info)
		{
				bool isShooting = false;
				float syncedShotVelocity = 0f;
				if (stream.isWriting) {
						isShooting = shotFired;
						syncedShotVelocity = shotVelocity;
						stream.Serialize (ref isShooting);
						stream.Serialize(ref syncedShotVelocity);
						shotFired = false;
				} else {
						syncTime = 0f;
						syncDelay = Time.time - lastSynchronizationTime;
						lastSynchronizationTime = Time.time;

			
						stream.Serialize (ref isShooting);
						stream.Serialize (ref syncedShotVelocity);

						shotVelocity = syncedShotVelocity;

						//syncEndShotVelocity = syncedShotVelocity; //syncedShotVelocity *  * syncDelay;
						//syncStartShotVelocity = rigidbody.position;
			
						if (isShooting) {
								FireShot ();
								isShooting = false;
								shotFired = false;
						}
				}
		}

		// Update is called once per frame
		void Update ()
		{
				if (networkView.isMine) {
					if (Input.GetButton ("Fire1") && (shotVelocity < maxShotVelocity)) {
						shotVelocity += 0.1f;
					}
					if (Input.GetButtonUp ("Fire1")) {
						FireShot ();
						shotFired = true;
						shotVelocity = 10f;
					}
				} else {
					
				}
		}

	void FixedUpdate()
	{
		if(networkView.isMine){
			UpdateTrajectory (this.transform.position, (transform.position - transform.parent.position) * shotVelocity, Physics.gravity);
		}
	}


	void UpdateTrajectory(Vector3 initialPosition, Vector3 initialVelocity, Vector3 gravity)
	{
		int numSteps = 20; // for example
		float timeDelta = 1.0f / initialVelocity.magnitude; // for example
	
		lineRenderer.SetVertexCount(numSteps);
		
		Vector3 position = initialPosition;
		Vector3 velocity = initialVelocity;
		for (int i = 0; i < numSteps; ++i)
		{
			lineRenderer.SetPosition(i, position);
			
			position += velocity * timeDelta + 0.5f * gravity * timeDelta * timeDelta;
			velocity += gravity * timeDelta;
		}
	}

}
