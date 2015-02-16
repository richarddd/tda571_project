using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour {

	GameObject mainCamera;
	List<NetworkView> listOfPlayers =new List<NetworkView>();
	GameObject[] activePlayers =null;
	float maxX =0;
	float minX=0;
	GameObject playerWithMaxX;
	GameObject playerWithMinX;

	// Use this for initialization
	void Start () {
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

	}
	
	// Update is called once per frame
	void Update () {
		if (listOfPlayers.Count != 0) {
			activePlayers = GameObject.FindGameObjectsWithTag ("Player");
		
			//Debug.Log (activePlayers);
			foreach(GameObject player in activePlayers){
//				Debug.Log( activePlayers.Length);
				if (minX<=player.transform.position.x){
					minX=player.transform.position.x;
					playerWithMinX=player;
				}
				if (maxX>=player.transform.position.x){
					maxX=player.transform.position.x;
					playerWithMaxX=player;
				}
			}
		}
		//Debug.Log (activePlayers.Length);
		if (activePlayers!=null && activePlayers.Length > 0 &&networkView.isMine) {
			calculateCameraPos ();
		}
	}

	//Remove??
	[RPC]
	public void addPlayer(NetworkViewID newPlayer){

		listOfPlayers.Add(NetworkView.Find (newPlayer));
//		Debug.Log (NetworkView.Find (newPlayer).name);

	}


	void calculateCameraPos()
	{
	}
}
