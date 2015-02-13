using UnityEngine;
using System.Collections;

public class CannonController : MonoBehaviour
{
	
		private Transform playerTransform;
		public Transform barrelTransform;
		private float lastSynchronizationTime = 0f;
		private float syncDelay = 0f;
		private float syncTime = 0f;


		private float syncStartRotationY = 0;
		private float syncEndRotationY = 0;
		private float syncStartBarrelAngle = 0;
		private float syncEndBarrelAngle = 0;

		private float offsetY = 0.45f;
		private Vector3 offset = Vector3.zero;

		private const float BarrelMaxAngle = 60f;
		private const float BarrelMinAngle = 120f;

		private const float RotationSmoothing = 0.01f;
		private const float BarrelSmoothing = 0.1f;
	



		// Use this for initialization
		void Start ()
		{
				playerTransform = transform.parent.Find ("Ball").transform;
				offset = new Vector3 (0, offsetY, 0);
				barrelTransform = transform.FindChild ("Barrel").transform;
				
		}

		void Awake ()
		{
				lastSynchronizationTime = Time.time;
				networkView.observed = this;
		}

		void OnSerializeNetworkView (BitStream stream, NetworkMessageInfo info)
		{
				float syncRotationY = 0;
				float syncBarrelAngle = 0;
				if (stream.isWriting) {
						syncRotationY = transform.rotation.eulerAngles.y;
						syncBarrelAngle = barrelTransform.rotation.eulerAngles.z;
						stream.Serialize (ref syncRotationY);
						stream.Serialize (ref syncBarrelAngle);
				} else {
						stream.Serialize (ref syncRotationY);
						stream.Serialize (ref syncBarrelAngle);
						syncTime = 0f;
						syncDelay = Time.time - lastSynchronizationTime;
						lastSynchronizationTime = Time.time;
						
						syncEndBarrelAngle = syncBarrelAngle + BarrelSmoothing * syncDelay;
						syncStartBarrelAngle = barrelTransform.rotation.eulerAngles.z;
				
						syncEndRotationY = syncRotationY + RotationSmoothing * syncDelay;
						syncStartRotationY = transform.rotation.eulerAngles.y;
				}
		}



		// Update is called once per frame
		void Update ()
		{
				this.transform.position = playerTransform.position + offset;
				

				if (networkView.isMine) {
						
						float targetAngle = Camera.main.transform.eulerAngles.y - 90;
						Vector3 rotationY = new Vector3 (0, targetAngle, 0);
						float cameraAngle = Camera.main.transform.rotation.eulerAngles.x;
						float cameraAnglePercent = Mathf.Abs (cameraAngle) / (Mathf.Abs (SmoothFollow.MinX) + Mathf.Abs (SmoothFollow.MaxX));
						float barrelAngle = BarrelMinAngle + ((BarrelMaxAngle - BarrelMinAngle) * cameraAnglePercent);
						
						
						transform.eulerAngles = rotationY;
						barrelTransform.rotation = Quaternion.Euler (rotationY + new Vector3 (0, 0, barrelAngle));

				} else {
						syncTime += Time.deltaTime;
						Debug.Log (syncEndRotationY);
						
						transform.eulerAngles = new Vector3 (0, Mathf.Lerp (syncStartRotationY, syncEndRotationY, syncTime / syncDelay), 0);



						barrelTransform.rotation = Quaternion.Euler (transform.eulerAngles + new Vector3 (0, 0, Mathf.Lerp (syncStartBarrelAngle, syncEndBarrelAngle, syncTime / syncDelay)));
				}
		}

		IEnumerator RotateByAngle (Vector3 byAngles, float inTime)
		{
				Quaternion fromAngle = transform.rotation;
				Quaternion toAngle = Quaternion.Euler (transform.eulerAngles + byAngles);
				for (float t = 0f; t < 1f; t += Time.deltaTime/inTime) {
						transform.rotation = Quaternion.Lerp (fromAngle, toAngle, t);
						yield return null;
				}
		}

		private void SmoothRotateToAngle (float to)
		{
				Vector3 toVector = new Vector3 (0, to, 0);
			
				if (Vector3.Distance (transform.eulerAngles, toVector) > 0.1f) {
						transform.eulerAngles = Vector3.Lerp (transform.rotation.eulerAngles, toVector, Time.deltaTime);
				} else {
						transform.eulerAngles = toVector;
				}
		}
}
