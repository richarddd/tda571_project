using UnityEngine;
using System.Collections;

public class Boost : MonoBehaviour {
	public float boostFactor = 1F;
	public GameObject particleSystem;
	Vector3 partSysPos;
	Quaternion partSysRot;
	GameObject boostPartSys;

	void OnTriggerEnter(Collider other){
		if (other.tag == "Player" && other.rigidbody != null) {
			other.rigidbody.AddForce (transform.forward * boostFactor);
			partSysPos = this.gameObject.transform.position;
			partSysPos.y += 1.56f;
			partSysPos.z -=0.72f;
			partSysRot = particleSystem.transform.rotation;

			boostPartSys =(GameObject) Network.Instantiate(particleSystem, partSysPos, partSysRot,0);
			StartCoroutine (Life ());
		}
	}

	IEnumerator Life() {
		
		yield return new WaitForSeconds (0.5f);
		Destroy (boostPartSys);
	}
}
