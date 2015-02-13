using UnityEngine;
using System.Collections;

public class Shooter : MonoBehaviour
{

		public Rigidbody projectile;
		public float shotForce = 1000f;

		private bool shotFired = false;

		void OnSerializeNetworkView (BitStream stream, NetworkMessageInfo info)
		{
				bool isShooting = false;
				if (stream.isWriting) {
						isShooting = shotFired;
						stream.Serialize (ref isShooting);
						
				} else {
						stream.Serialize (ref isShooting);
						if (isShooting) {
								FireShot ();
								//shotFired = false;
						}
				}
		}

		void FireShot ()
		{
				Rigidbody shot = Instantiate (projectile, transform.position, transform.rotation) as Rigidbody;
				shot.AddForce (transform.up * shotForce * Time.deltaTime * -1);

		}

		void Update ()
		{

				if (networkView.isMine) {
		
						if (Input.GetButtonUp ("Fire1")) {
								shotFired = true;
								FireShot ();		
						}
				}
		}
}
