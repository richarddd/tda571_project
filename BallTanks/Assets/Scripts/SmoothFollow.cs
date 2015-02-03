using UnityEngine;
using System.Collections;

public class SmoothFollow : MonoBehaviour
{
	
	public float offsetX = 0.0f;
	public float offsetY = 1f;
	public float offsetZ = -3f;
	public GameObject target;
	private float xSpeed = 100f;
	private float ySpeed = 100f;

	private Vector3 posSmooth = Vector3.zero;
	private Vector3 posVelocity = Vector3.zero;
	private float smoothTime = 0.1F;


	Quaternion rotation;
	

	public void setTarget(GameObject target){
		this.target = target;
	}

	private float rotationY;

	void LateUpdate() {
		if (target != null) {



			float mouseX = Input.GetAxis("Mouse X") * xSpeed * 0.05f;
			float mouseY = Input.GetAxis("Mouse Y") * ySpeed * 0.05f;

			//rotation around y axis
			rotationY+=mouseX;

			rotation = Quaternion.Slerp(rotation, Quaternion.Euler(0, rotationY, 0), Time.time * smoothTime);
			posSmooth = Vector3.SmoothDamp(posSmooth,target.transform.position,ref posVelocity,smoothTime);

			transform.rotation = rotation;
			transform.position = rotation * new Vector3(offsetX,offsetY, offsetZ) + posSmooth;

		}
	}
}

