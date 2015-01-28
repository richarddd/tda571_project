using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;
	public Vector3 offset;
	public float speed;
	Vector3 point;

	// Use this for initialization
	void Start () {
		offset = transform.position;
	}

	void Update () {
		Vector3 pos = transform.position;
		Vector3 dir = Input.mousePosition - pos;
		float angle = Mathf.Atan2 (dir.x -Screen.width/2, dir.y) * Mathf.Rad2Deg;
		 
		point = player.transform.position;
		Debug.Log (point);
		transform.RotateAround (point, new Vector3 (0.0f, 1.0f, 0.0f), speed*Time.deltaTime*angle);

	}

}

