using UnityEngine;
using System.Collections;

public class Exploder : MonoBehaviour {

	public float radius = 5.0f;
	public float power = 10.0f;
	private GameObject shooter;
	public GameObject explotion;
	
	void OnCollisionEnter(Collision collision){
		Vector3 explosionPos = transform.position;
		Collider[] colliders = Physics.OverlapSphere (explosionPos, radius);

		if (collision.gameObject != shooter) {
			foreach (Collider hit in colliders) {
				if (hit && hit.rigidbody) {
					hit.rigidbody.AddExplosionForce (power, explosionPos, radius, 3.0f);
					Destroy (gameObject, 0.05f);
					SpawnExplotion();

				}
			}
			if (collision.gameObject.tag == "Player") {

				PlayerHealthBar hitPlayer = collision.gameObject.transform.parent.GetComponent<PlayerHealthBar>();
				hitPlayer.decrementHealth(10);		
			}
		}

	}

	private void SpawnExplotion(){


			Detonator dTemp = (Detonator)explotion.GetComponent("Detonator");
            float offsetSize = dTemp.size/3;
            GameObject exp = (GameObject) Instantiate(explotion, transform.position, Quaternion.identity);
            dTemp = (Detonator)exp.GetComponent("Detonator");
            dTemp.detail = 1f;
            Destroy(exp, 10);

	}

	public void setShooter(GameObject owner){
		shooter = owner;
	}
}
