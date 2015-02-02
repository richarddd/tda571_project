using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;
	public float speed;
	public float cameraSpeed;
	Vector3 point;
	Vector3 oldMousePos; 
	float dir;

	// Use this for initialization
	void Start () {
		oldMousePos = Input.mousePosition;
		dir = 0;
	}
	//TODO x+=(targett-x)/velocity
	void Update () {
		Vector3 pos = transform.position;
		Vector3 mousePos = Input.mousePosition; 

		float angle = Mathf.Rad2Deg*cameraSpeed;
		 
		point = player.transform.position;

		if ((oldMousePos.x < mousePos.x) ) { 
			dir=1f;					
		}
		else if((oldMousePos.x>mousePos.x)){
			dir=-1f;
		}
		else{
			dir=0f;
		}

		transform.RotateAround (point, new Vector3 (0.0f, 1.0f, 0.0f), dir*speed*Time.deltaTime*angle );
		oldMousePos = mousePos;

	}

}

