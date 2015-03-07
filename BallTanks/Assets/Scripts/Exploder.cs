using UnityEngine;
using System.Collections;

public class Exploder : MonoBehaviour {

	public float radius = 5.0f;
	public float power = 500.0f;
	private GameObject shooter;
	public GameObject explotion;
	public GameObject projectileExplosion;
	
	void OnCollisionEnter(Collision collision){
		Vector3 explosionPos = transform.position;
		Collider[] colliders = Physics.OverlapSphere (explosionPos, radius);

		if (collision.gameObject != shooter) {

			Instantiate(projectileExplosion, new Vector3(explosionPos.x, explosionPos.y, explosionPos.z), Quaternion.identity);

			foreach (Collider hit in colliders) {
				if (hit && hit.rigidbody) {
					hit.rigidbody.AddExplosionForce (power, explosionPos, radius, 3.0f);
					Destroy (gameObject, 0.1f);
					if (hit.tag.Equals ("Player")) {
						
						PlayerHealthBar hitPlayer = hit.gameObject.transform.parent.GetComponent<PlayerHealthBar>();
						int damage = ((int) (10/radius)) * (((int) radius) - ((int) Vector3.Distance(explosionPos, hit.transform.position)));
						if(damage > 0)
						{
							hitPlayer.decrementHealth(damage);
						}
					}
				}
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
