using UnityEngine;
using System.Collections;

public class FixedCannon : MonoBehaviour
{

	public Rigidbody projectile;
	
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

	private Material playerMaterial;
	private Material enemyMaterial;

	
	void Start ()
	{
		playerControl = transform.parent.GetComponent<PlayerControl>();
		playerMaterial = new Material (Shader.Find ("Sprites/Default"));
		playerMaterial.color = Color.green;
		enemyMaterial = new Material(Shader.Find("Sprites/Default"));
		enemyMaterial.color = Color.red;
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
			Rigidbody shot = Instantiate (projectile, this.transform.position, this.transform.rotation) as Rigidbody;
			shot.GetComponent<Exploder> ().setShooter (this.gameObject.transform.parent.gameObject);	
			shot.rigidbody.velocity = ((transform.position - transform.parent.position) * shotVelocity);
			shotFired = true;
			shotVelocity = 10f;
			nextFire = Time.time + fireRate;
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


	void Update ()
	{
		if (networkView.isMine) {
			if (Input.GetButton ("Fire1") && (shotVelocity < maxShotVelocity)) {
				shotVelocity += 0.1f;
			}
			if (Input.GetButtonUp ("Fire1") && Time.time > nextFire) {
				FireShot ();		
			}
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
			lineRenderer.material = playerMaterial;
		} else {
			lineRenderer.material = enemyMaterial;
		}

		int numSteps = 20;
		float timeDelta = 1.0f / initialVelocity.magnitude;

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
