using UnityEngine;
using System.Collections;

public class FixedCannon : MonoBehaviour
{

	public Rigidbody projectile;
	//public float shotForce = 50000f;
	//public float maxShotForce = 150000f;
	
	public float shotVelocity = 10f;
	public float maxShotVelocity = 60f;

		private bool shotFired = false;

		private float lastSynchronizationTime = 0f;
		private float syncDelay = 0f;
		private float syncTime = 0f;

		private float syncEndShotVelocity = 0f;
		private float syncStartShotVelocity = 0f;

		private LineRenderer lineRenderer;

		public float fireRate = 1F;
    	private float nextFire = 0.0F;
		private PlayerControl playerControl;

		// Use this for initialization
		void Start ()
		{
			playerControl = transform.parent.GetComponent<PlayerControl>();
		}

		void Awake ()
		{
			nextFire = Time.time + 1f;	
			lastSynchronizationTime = Time.time;
			networkView.observed = this;
		}

		void FireShot ()
	{	
			if (! playerControl.isPlayerFrozen ()) {
				//fireShotDone = false;
				Rigidbody shot = Instantiate (projectile, this.transform.position, this.transform.rotation) as Rigidbody;
				//shot.AddForce (transform.TransformDirection(transform.forward) * shotForce * Time.deltaTime * -1);
				shot.GetComponent<Exploder> ().setShooter (this.gameObject.transform.parent.gameObject);	
				shot.rigidbody.velocity = ((transform.position - transform.parent.position) * shotVelocity);
				Debug.Log("==============");
				shotFired = true;
				shotVelocity = 10f;
				nextFire = Time.time + fireRate;
				//fireShotDone = true;
			}
		}

		void OnSerializeNetworkView (BitStream stream, NetworkMessageInfo info)
		{
				bool isShooting = false;
				float syncedShotVelocity = 0f;
				if (stream.isWriting) {
						isShooting = shotFired;
						syncedShotVelocity = shotVelocity;
						stream.Serialize (ref isShooting);
						stream.Serialize(ref syncedShotVelocity);
						shotFired = false;
				} else {
						syncTime = 0f;
						syncDelay = Time.time - lastSynchronizationTime;
						lastSynchronizationTime = Time.time;

			
						stream.Serialize (ref isShooting);
						stream.Serialize (ref syncedShotVelocity);

						if(shotVelocity <  syncedShotVelocity){
							shotVelocity = syncedShotVelocity;
						}

						if (isShooting) {
							FireShot ();
							isShooting = false;
						}
				}
		}

		// Update is called once per frame
		void Update ()
		{
				if (networkView.isMine) {
					if (Input.GetButton ("Fire1") && (shotVelocity < maxShotVelocity)) {
						shotVelocity += 0.1f;
					}
					if (Input.GetButtonUp ("Fire1") && Time.time > nextFire) {
						FireShot ();		
					}
				} else {
					
				}
		}

	void FixedUpdate()
	{
			UpdateTrajectory (this.transform.position, (transform.position - transform.parent.position) * shotVelocity, Physics.gravity);
	}


	void UpdateTrajectory(Vector3 initialPosition, Vector3 initialVelocity, Vector3 gravity)
	{
		lineRenderer = GetComponent<LineRenderer>();
		if (networkView.isMine) {
						Material playerMat = new Material (Shader.Find ("Sprites/Default"));
						playerMat.color = Color.green;
						lineRenderer.material = playerMat;
				} else {
			Material playerMat = new Material(Shader.Find("Sprites/Default"));
			playerMat.color = Color.red;
			lineRenderer.material = playerMat;
				}

		int numSteps = 20; // for example
		float timeDelta = 1.0f / initialVelocity.magnitude; // for example
	
		lineRenderer.SetVertexCount(numSteps);
		
		Vector3 position = initialPosition;
		Vector3 velocity = initialVelocity;
		for (int i = 0; i < numSteps; ++i)
		{
			lineRenderer.SetPosition(i, position);
			
			position += velocity * timeDelta + 0.5f * gravity * timeDelta * timeDelta;
			velocity += gravity * timeDelta;
		}
	}

}
