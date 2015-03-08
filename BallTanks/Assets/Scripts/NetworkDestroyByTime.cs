using UnityEngine;
using System.Collections;

public class NetworkDestroyByTime : MonoBehaviour {

	public float lifetime;


	public float timer = 0;
    void Awake() {
        timer = Time.time;
    }
    void Update() {
        if (Time.time - timer > lifetime && networkView.isMine)
            Network.Destroy(gameObject);
        
    }
}
