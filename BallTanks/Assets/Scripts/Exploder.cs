﻿using UnityEngine;
using System.Collections;

public class Exploder : MonoBehaviour {

	public float radius = 5.0f;
	public float power = 10.0f;
	private GameObject shooter;
	
	void OnCollisionEnter(Collision collision){
		Vector3 explosionPos = transform.position;
		Collider[] colliders = Physics.OverlapSphere (explosionPos, radius);

		if (collision.gameObject != shooter) {
			foreach (Collider hit in colliders) {
				if (hit && hit.rigidbody) {
					hit.rigidbody.AddExplosionForce (power, explosionPos, radius, 3.0f);
					Destroy (gameObject, 0.05f);
				}
			}
			if (collision.gameObject.tag == "Player") {

				PlayerHealthBar hitPlayer = collision.gameObject.transform.parent.GetComponent<PlayerHealthBar>();
				hitPlayer.decrementHealth(10);		
			}
		}

	}

	public void setShooter(GameObject owner){
		shooter = owner;
	}
}
