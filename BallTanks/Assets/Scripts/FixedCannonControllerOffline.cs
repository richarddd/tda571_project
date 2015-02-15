using UnityEngine;
using System.Collections;

public class FixedCannonControllerOffline : MonoBehaviour
{

		public Rigidbody projectile;
		public float shotForce = 50000f;

		// Use this for initialization
		void Start ()
		{
				
		}

		void FireShot ()
		{
			Rigidbody shot = Instantiate (projectile, this.transform.position, this.transform.rotation) as Rigidbody;
			shot.AddForce (transform.TransformDirection(transform.forward) * shotForce * Time.deltaTime * -1);
		}

		// Update is called once per frame
		void Update ()
		{
			if (Input.GetButtonUp ("Fire1")) {
				FireShot ();
			}
		}

}
