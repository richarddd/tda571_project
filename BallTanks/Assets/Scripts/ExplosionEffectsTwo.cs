using UnityEngine;
using System.Collections;

public class ExplosionEffectsTwo : MonoBehaviour {
	
	public float explosionRadius = 5.0f;
	
	void Start () {
		transform.localScale = new Vector3 (explosionRadius, explosionRadius, explosionRadius);
		
		Color tempColor = Color.red;
		tempColor.a = 0.5f;
		renderer.material.color = tempColor;
	}
	
	void Update () {
		Color tempColor = renderer.material.color;
		tempColor.a = (tempColor.a - 0.01f);
		renderer.material.color = tempColor;
		
		if (renderer.material.color.a <= 0) {
			Destroy (gameObject);
		}
	}
}
