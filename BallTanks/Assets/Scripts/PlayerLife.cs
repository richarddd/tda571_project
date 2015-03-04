using UnityEngine;
using System.Collections;

public class PlayerLife : MonoBehaviour {

	private GameObject canvas;
	private int numberOfLifes;


	// Use this for initialization
	void Start () {

		numberOfLifes = 3;

		canvas =GameObject.FindGameObjectWithTag("Canvas");
		if(networkView.isMine){
			canvas.transform.GetChild(1).gameObject.SetActive(true);
			canvas.transform.GetChild(2).gameObject.SetActive(true);
			canvas.transform.GetChild(3).gameObject.SetActive(true);
		}
	}

	public int GetNumberOfLifes(){
		return numberOfLifes;
	}

	public void setNumberOfLifes (int i){
		numberOfLifes = i;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	[RPC]
	void GameOver(){
		canvas.transform.GetChild (7).gameObject.SetActive (true);
		canvas.transform.GetChild (9).gameObject.SetActive (true);
		canvas.transform.GetChild (10).gameObject.SetActive (true);

		
	}
}
