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
	float [] playerZPos = null;
	float maxX =0;
	float minX=0;

	float maxZ =0;
	float minZ=0;

	GameObject playerWithMaxX;
	GameObject playerWithMinX;
	GameObject playerWithMaxZ;
	GameObject playerWithMinZ;

	Vector3 center =Vector3.zero;
	Vector3 oldCenter=Vector3.zero;

	int maxXIndex;
	int minXIndex;
	int maxZIndex;
	int minZIndex;

	Transform player1X;
	Transform player2X;
	Transform player1Z;
	Transform player2Z;
	// Use this for initialization
	void Start () {
		mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		aspectRatio = Screen.width / Screen.height;

	}
	
	// Update is called once per frame
	void Update () {
			
			activePlayers = GameObject.FindGameObjectsWithTag ("Player");

			if (activePlayers != null) {
				//Calculates how much the camera should zoom in the x-axis
				calculateMaxAndMinX();
						
				if(maxXIndex != minXIndex){
					playerWithMaxX = activePlayers[maxXIndex];
					playerWithMinX =activePlayers[minXIndex];
					minX = 0;
					maxX = 0;
				}else if (activePlayers.Length ==1){
					playerWithMinX =activePlayers[minXIndex];
					calculateSinglePlayerCamera ();
				}

			if(maxZIndex != minZIndex){
				playerWithMaxZ = activePlayers[maxZIndex];
				playerWithMinZ =activePlayers[minZIndex];
				minZ = 0;
				maxZ = 0;
			}
//			Debug.Log(activePlayers.Length);
				if (activePlayers.Length > 1 && networkView.isMine) {
					calculateCameraPos ();
				} 
			}	
		}

	void calculateMaxAndMinX(){
		playerXPos = new float[activePlayers.Length];
		playerZPos = new float[activePlayers.Length];
		maxXIndex=0;
		minXIndex=0;
		maxZIndex=0;
		minZIndex=0;

		for (int i=0; i <activePlayers.Length; i++){
			float playerX =activePlayers[i].transform.position.x;
			playerXPos[i]=activePlayers[i].transform.position.x;

			float playerZ = activePlayers[i].transform.position.z;
			playerZPos[i]=activePlayers[i].transform.position.z;

			if (playerX >= maxX) {
				maxX = playerX;
				maxXIndex=i;
			}
			if (playerX <= minX) {
				minX = playerX;
				minXIndex=i;
			}
			//BROKEN
			if (playerZ >= maxZ) {
				maxZ = playerZ;
				maxZIndex=i;
			}
			if (playerZ <= minZ) {
				minZ = playerZ;
				minZIndex=i;
			}
		}	
	}

	void calculateCameraPos()
	{
		player1X = playerWithMaxX.transform;
		player2X = playerWithMinX.transform;
		player1Z = playerWithMaxZ.transform;
		player2Z = playerWithMinZ.transform;


		calculateDistanceBetweenPlayers ();

		center.x = player1X.position.x + (0.5f * (player2X.position.x - player1X.position.x));
		center.z = player1Z.position.z + (0.5f * (player2Z.position.z - player1Z.position.z));
		Vector3 temp= mainCamera.transform.position; 
		center.z -= 2f;
		temp.x= center.x;
		temp.z += center.z- oldCenter.z;

		
		mainCamera.transform.position = temp;
		oldCenter = center;

		distanceFromMiddlePoint = (mainCamera.transform.position - center).magnitude;
		Camera.main.fieldOfView = 2.0f * Mathf.Rad2Deg * Mathf.Atan((0.5f * distanceBetweenPlayers) / (distanceFromMiddlePoint * aspectRatio));
		mainCamera.camera.fieldOfView += 15f;


	}

	void calculateDistanceBetweenPlayers(){
		distanceBetweenPlayers = 0;
		if ((player2Z.position - player1Z.position).magnitude>distanceBetweenPlayers){
			distanceBetweenPlayers = (player2Z.position - player1Z.position).magnitude;
		
		}if((player2Z.position - player1X.position).magnitude>distanceBetweenPlayers){
			distanceBetweenPlayers = (player2Z.position - player1X.position).magnitude;
		
		}if((player2X.position - player1Z.position).magnitude>distanceBetweenPlayers){
			distanceBetweenPlayers = (player1X.position - player1Z.position).magnitude;

		}if((player2X.position - player1X.position).magnitude>distanceBetweenPlayers){
			distanceBetweenPlayers = (player2X.position - player1X.position).magnitude;

		}

	}

	void calculateSinglePlayerCamera(){
		//This is for following camera

		center = playerWithMinX.transform.position;

		Vector3 temp= mainCamera.transform.position; 
		temp.x= center.x;
		temp.z += center.z- oldCenter.z;

		mainCamera.transform.position = temp;
		oldCenter = center;

	}
}
