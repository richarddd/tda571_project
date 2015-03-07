using UnityEngine;
using System.Collections;

public class SmoothFollow : MonoBehaviour
{

		private const float SmoothTime = 0.1F;
		private const float MaxZoom = 2.0F;
		private const float MinZoom = 10.0F;

		public const float MaxX = 60f;
		public const float MinX = 0.5f;

		private GameObject target;
		private float xSpeed = 100f;
		private float ySpeed = 100f;
		private Vector3 posSmooth = Vector3.zero;
		private Vector3 posVelocity = Vector3.zero;
		private Quaternion rotation;
		private float rotationY;
		private float rotationX; 

		public float radius = 3;


		public void setTarget (GameObject target)
		{
				this.target = target;
		}

		void LateUpdate ()
		{
				if (target != null) {



						float mouseX = Input.GetAxis ("Mouse X") * xSpeed * 0.05f;
						float mouseY = Input.GetAxis ("Mouse Y") * ySpeed * -0.05f;

						float mouseScroll = Input.GetAxis ("Mouse ScrollWheel");

			

						//rotation around y axis
						rotationY += mouseX;
						//rotation around x axis
						rotationX += mouseY;

						if (rotationX >= MaxX) {
								rotationX = MaxX;
						}
						if (rotationX <= MinX) {
								rotationX = MinX;
						}

						if (radius <= MaxZoom) {
								radius = MaxZoom;
						}
						if (radius >= MinZoom) {
								radius = MinZoom;
						}



						radius = Mathf.Lerp (radius, radius + mouseScroll, Time.deltaTime * SmoothTime * 50);
		

						rotation = Quaternion.Slerp (rotation, Quaternion.Euler (rotationX, rotationY, 0), Time.time * SmoothTime);

						posSmooth = Vector3.SmoothDamp (posSmooth, target.transform.position, ref posVelocity, Time.deltaTime * SmoothTime); 

						transform.rotation = rotation;
						transform.position = rotation * new Vector3 (0, 0, -radius) + posSmooth;
						//transform.position = rotation * (target.transform.position.normalized*radius) +posSmooth;

				}
		}
}

