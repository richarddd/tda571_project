using UnityEngine;
using System.Collections;

public class Shooter : MonoBehaviour
{

		public Rigidbody projectile;
		public float shotForce = 1000f;
		public float moveSpeed = 10f;

		private Transform barrelTransform;

		private bool shotFired = false;


		void Start ()
		{
				barrelTransform = transform.FindChild ("Barrel").transform;
		}

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
				Rigidbody shot = Instantiate (projectile, transform.position + new Vector3 (0, 0.1f, 0), Quaternion.identity) as Rigidbody;
				shot.AddForce (barrelTransform.up * shotForce * Time.deltaTime * 90 * -1);

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
