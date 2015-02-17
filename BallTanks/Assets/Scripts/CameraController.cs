using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour {

	GameObject mainCamera;
	float distanceBetweenPlayers;
	float distanceFromMiddlePoint;
	float aspectRatio;
	GameObject[] activePlayers =null;
	float [] playerXPos = null;
	float maxX =0;
	float minX=0;
	GameObject playerWithMaxX;
	GameObject playerWithMinX;

	Vector3 center =Vector3.zero;
	Vector3 oldCenter=Vector3.zero;

	int maxIndex;
	int minIndex;
	// Use this for initialization
	void Start () {
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		aspectRatio = Screen.width / Screen.height;

	}
	
	// Update is called once per frame
	void Update () {
			
			activePlayers = GameObject.FindGameObjectsWithTag ("Player");

			if (activePlayers != null) {
				
				calculateMaxAndMin();
						
				if(maxIndex != minIndex){
					playerWithMaxX = activePlayers[maxIndex];
					playerWithMinX =activePlayers[minIndex];
					minX = 0;
					maxX = 0;
				}else if (activePlayers.Length ==1){
					playerWithMinX =activePlayers[minIndex];
					calculateSinglePlayerCamera ();
				}

				if (activePlayers.Length > 1 && networkView.isMine) {
					calculateCameraPos ();
				} 
			}	
		}

	void calculateMaxAndMin(){
		playerXPos = new float[activePlayers.Length];
		maxIndex=0;
		minIndex=0;

		for (int i=0; i <activePlayers.Length; i++){
			float player =activePlayers[i].transform.position.x;
			playerXPos[i]=activePlayers[i].transform.position.x;
			if (player >= maxX) {
				maxX = player;
				maxIndex=i;
			}
			if (player <= minX) {
				minX = player;
				minIndex=i;
			}
		}	
	}

	void calculateCameraPos()
	{
		Transform player1 = playerWithMaxX.transform;
		Transform player2 = playerWithMinX.transform;
		distanceBetweenPlayers = (player2.position - player1.position).magnitude;
		center = player1.position + 0.5f * (player2.position - player1.position);
		//Debug.Log (center);
		//Vector3 tempCenter = center / 2;
		Vector3 temp= mainCamera.transform.position; 
		//Debug.Log (center);
		temp.x= center.x;
		temp.z += center.z- oldCenter.z;
		
		mainCamera.transform.position = temp;
		oldCenter = center;

		distanceFromMiddlePoint = (mainCamera.transform.position - center).magnitude;
		Camera.main.fieldOfView = 2.0f * Mathf.Rad2Deg * Mathf.Atan((0.5f * distanceBetweenPlayers) / (distanceFromMiddlePoint * aspectRatio));
		mainCamera.camera.fieldOfView += 15f;


	}

	void calculateSinglePlayerCamera(){
		//This is for following camera

		center = playerWithMinX.transform.position;

		Vector3 temp= mainCamera.transform.position; 
		temp.x= center.x;
		temp.z += center.z- oldCenter.z;

		mainCamera.transform.position = temp;
		oldCenter = center;


		/*
		//This is for zooming TODO might not be needed here
		if (playerWithMaxX.renderer.isVisible) {
			if(Vector3.Distance(mainCamera.transform.position,center)>startDistance){
				mainCamera.transform.Translate(Vector3.forward * 2 * Time.deltaTime);
				Debug.Log (Vector3.Distance(mainCamera.transform.position,center) + " " +startDistance);
			}
			//Debug.Log ("forward");
		} else if (!playerWithMaxX.renderer.isVisible){
			mainCamera.transform.Translate(Vector3.back * 2 * Time.deltaTime);
			Debug.Log ("back");
		} */
	}
}
