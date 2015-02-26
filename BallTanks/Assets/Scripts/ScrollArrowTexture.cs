using UnityEngine;
using System.Collections;

public class ScrollArrowTexture : MonoBehaviour {
	public float scrollSpeed = 0.1F;
	
	void Update() {
		float offset = Time.time * scrollSpeed;
		renderer.material.SetTextureOffset("_MainTex", new Vector2(0, offset));
	}
}
