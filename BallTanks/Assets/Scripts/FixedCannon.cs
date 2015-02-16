using UnityEngine;
using System.Collections;

public class FixedCannon : MonoBehaviour
{

		public Rigidbody projectile;
		public float shotForce = 50f;

		private bool shotFired = false;

		private float lastSynchronizationTime = 0f;
		private float syncDelay = 0f;
		private float syncTime = 0f;

		// Use this for initialization
		void Start ()
		{
				
		}

		void Awake ()
		{
				lastSynchronizationTime = Time.time;
				networkView.observed = this;
		}

		void FireShot ()
		{
				Rigidbody shot = Instantiate (projectile, this.transform.position, this.transform.rotation) as Rigidbody;
				shot.AddForce (transform.forward * shotForce * 1000f * Time.deltaTime * -1);
		}

		void OnSerializeNetworkView (BitStream stream, NetworkMessageInfo info)
		{
				bool isShooting = false;
				if (stream.isWriting) {
						isShooting = shotFired;
						stream.Serialize (ref isShooting);
						shotFired = false;
				} else {
						syncTime = 0f;
						syncDelay = Time.time - lastSynchronizationTime;
						lastSynchronizationTime = Time.time;

			
						stream.Serialize (ref isShooting);
			
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
						if (Input.GetButtonUp ("Fire1")) {
								FireShot ();
						}
				} else {
					
				}
		}

}
