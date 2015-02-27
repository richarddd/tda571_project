using UnityEngine;
using System.Collections;

public class FreezePartSys : MonoBehaviour {

	
	GameObject frozenPlayer;
	
	// Use this for initialization
	void Start () {
		StartCoroutine (Life ());
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = frozenPlayer.transform.position;
	}
	
	public void setFrozenPlayer(GameObject player){
		frozenPlayer = player;
	}

	IEnumerator Life() {
		
		yield return new WaitForSeconds (5);
		Destroy (this.gameObject);
	}
}
