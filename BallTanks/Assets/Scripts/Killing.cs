using UnityEngine;
using System.Collections;

public class Killing : MonoBehaviour {
	private GameObject canvas;
	// Use this for initialization
	void Start () {
		canvas =GameObject.FindGameObjectWithTag("Canvas");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider colInfo){
		if (colInfo.tag == "Player") {
			if(colInfo.networkView.isMine){
				canvas.transform.GetChild(0).gameObject.SetActive(true);
			}
			Destroy(colInfo.gameObject);

		}
	}
}
