using UnityEngine;
using System.Collections;

public class Exploder : MonoBehaviour {

	public float radius = 5.0f;
	public float power = 500.0f;
	private GameObject shooter;
	public GameObject projectileExplosion;
	
	void OnCollisionEnter(Collision collision){

		if (collision.gameObject != shooter) {
			Vector3 explosionPos = transform.position;
			Collider[] colliders = Physics.OverlapSphere (explosionPos, radius);
			Instantiate(projectileExplosion, new Vector3(explosionPos.x, explosionPos.y, explosionPos.z), Quaternion.identity);

			foreach (Collider hit in colliders) {
				if (hit && hit.rigidbody && hit.tag.Equals ("Player")) {
					hit.rigidbody.AddExplosionForce (power, explosionPos, radius, 3.0f);

					PlayerHealthBar hitPlayer = hit.gameObject.transform.parent.GetComponent<PlayerHealthBar>();
					int damage = ((int) (10/radius)) * (((int) radius) - ((int) Vector3.Distance(explosionPos, hit.transform.position)));
					if(damage > 0)
					{
						hitPlayer.decrementHealth(damage);
					}

					}
				}
			Destroy (gameObject);
			}
		}

	public void setShooter(GameObject owner){
		shooter = owner;
	}
	}
