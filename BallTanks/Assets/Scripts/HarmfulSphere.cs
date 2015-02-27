using UnityEngine;
using System.Collections;

public class HarmfulSphere : MonoBehaviour {
	float startSize=0.5f;
	Transform sphereTrans;

	public int damage;
	// Use this for initialization
	void Start () {
		sphereTrans = this.transform;
	}

	void OnTriggerEnter(Collider colInfo){
		if (colInfo.tag =="Player"){

			colInfo.gameObject.transform.parent.GetComponent<PlayerHealthBar>().decrementHealth(damage);
		}
	}
	
	// Update is called once per frame
	void Update () {

		StartCoroutine(Grow());
	}
	IEnumerator Grow() {

		while (startSize <15) {
			sphereTrans = this.transform;
			sphereTrans.localScale +=new Vector3(0.1f, 0.1f, 0.1f);
			startSize+=0.1f;
			
			
			this.transform.localScale = sphereTrans.localScale;
			yield return new WaitForSeconds (1);
		}
		Destroy (this.gameObject);

	}
}
