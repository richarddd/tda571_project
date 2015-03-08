using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Killing : MonoBehaviour {
	private GameObject canvas;
	public GameObject ParticleEffect;
	private int noOfPlayers=0;
	// Use this for initialization
	void Start () {
		canvas =GameObject.FindGameObjectWithTag("Canvas");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	[RPC]
	void PlayerJoined(){
			noOfPlayers++;
			Debug.Log ("Number of players " +noOfPlayers);	
	

	}

	[RPC]
	void PlayerLeft(){
		noOfPlayers--;
		Debug.Log ("Number of players " +noOfPlayers);	
		
		
	}

	void OnTriggerEnter(Collider colInfo){
		GameObject playerToKill = colInfo.gameObject;
		if (colInfo.tag == "Player" && colInfo.transform.parent.networkView.isMine) {
			Network.Instantiate(ParticleEffect, colInfo.transform.position, colInfo.transform.rotation,0);
			audio.Play();
			Kill (playerToKill);
		}
	}

	public void Kill (GameObject colInfo){
		//Debug.Log (colInfo.name);
		int lifeLeft = colInfo.gameObject.transform.parent.GetComponent<PlayerLife>().GetNumberOfLifes();
		canvas.transform.GetChild(lifeLeft).gameObject.SetActive(false);
		lifeLeft--;
			
			//If the player have life left
		if (lifeLeft > 0){
				
			canvas.transform.GetChild(0).gameObject.SetActive(true);
			colInfo.gameObject.transform.parent.GetComponent<PlayerLife>().setNumberOfLifes(lifeLeft);
				
			for (int i=0; i<colInfo.gameObject.transform.parent.childCount; i++){
				//colInfo.gameObject.transform.parent.GetChild(i).gameObject.SetActive(false);
				colInfo.gameObject.transform.parent.gameObject.SetActive(false);
			}

			canvas.transform.GetChild(5).gameObject.SetActive(true);
			canvas.transform.GetChild(6).gameObject.SetActive(true);

			colInfo.gameObject.transform.parent.GetComponent<PlayerHealthBar>().setCurrentHealth(100);	
				
			StartCoroutine(Respawn(colInfo.gameObject));
				
		}
			
			//If the player lost the last life
		else{
			canvas.transform.GetChild(4).gameObject.SetActive(true);
				//Destroy(colInfo.gameObject);
			for (int i=0; i<colInfo.gameObject.transform.parent.childCount; i++){
				//Destroy(colInfo.gameObject.transform.parent.GetChild(i).gameObject);
				Destroy(colInfo.gameObject.transform.parent.gameObject);
			}

			canvas.transform.GetChild (9).gameObject.SetActive (true);
			canvas.transform.GetChild (10).gameObject.SetActive (true);
				
			noOfPlayers--;
				
			if (noOfPlayers<2){
					//GameObject.FindGameObjectWithTag("Player").SendMessage("GameOver");
				GameObject.FindGameObjectWithTag("Player").gameObject.transform.parent.networkView.RPC ("GameOver",RPCMode.OthersBuffered);
			}
		}
		
	}

	//Used for respawning
	IEnumerator Respawn(GameObject colInfo) {

		for (int i =5; i>0; i--) {
			canvas.transform.GetChild(6).GetComponent<Text>().text = i.ToString();
			yield return new WaitForSeconds (1);

		}
		colInfo.rigidbody.velocity = Vector3.zero;
		colInfo.transform.position = new Vector3(0,5,0);
		colInfo.renderer.material.color = Color.white;

		//Gui stuff
		canvas.transform.GetChild(5).gameObject.SetActive(false);
		canvas.transform.GetChild(6).gameObject.SetActive(false);
		canvas.transform.GetChild(0).gameObject.SetActive(false);

		for (int i=0; i<colInfo.gameObject.transform.parent.childCount; i++){
			//colInfo.gameObject.transform.parent.GetChild(i).gameObject.SetActive(true);
			colInfo.gameObject.transform.parent.gameObject.SetActive(true);

		}
	}
}
