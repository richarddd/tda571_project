using UnityEngine;
using System.Collections;

public class Shooter : MonoBehaviour
{

		public Rigidbody projectile;
		public float shotForce = 1000f;
		public float moveSpeed = 10f;

		private Transform barrelTransform;


		void Start ()
		{
				barrelTransform = transform.FindChild ("Barrel").transform;
		}

		void Update ()
		{
		
				if (Input.GetButtonUp ("Fire1")) {
						
						Rigidbody shot = Instantiate (projectile, transform.position + new Vector3 (0, 0.1f, 0), Quaternion.identity) as Rigidbody;
						shot.AddForce (barrelTransform.up * shotForce * Time.deltaTime * 90 * -1);

				}
		}
}
