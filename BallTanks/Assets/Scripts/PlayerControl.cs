using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	public float speed;

	private float lastSynchronizationTime = 0f;
	private float syncDelay = 0f;
	private float syncTime = 0f;
	private Vector3 syncStartPosition = Vector3.zero;
	private Vector3 syncEndVelocity = Vector3.zero;
	private Vector3 syncEndPosition = Vector3.zero;
	

	// Use this for initialization
	void Start () {
	
	}

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		Vector3 syncPosition = Vector3.zero;
		Vector3 syncVelocity = Vector3.zero;
		if (stream.isWriting)
		{
			syncPosition = rigidbody.position;
			stream.Serialize(ref syncPosition);
			
			syncVelocity = rigidbody.velocity;
			stream.Serialize(ref syncVelocity);
		}
		else
		{
			stream.Serialize(ref syncPosition);
			stream.Serialize(ref syncVelocity);
			
			syncTime = 0f;
			syncDelay = Time.time - lastSynchronizationTime;
			lastSynchronizationTime = Time.time;
			
			
			syncEndPosition = syncPosition + syncVelocity * syncDelay;
			syncStartPosition = rigidbody.position;
		}
	}

	void Awake()
	{
		//networkview = gameObject.GetComponent<NetworkView> ();
		//rigidbody = gameObject.GetComponent<Rigidbody> ();
		networkView.observed = this;
		lastSynchronizationTime = Time.time;
	}
	
	void FixedUpdate()
	{
		if (networkView.isMine)
		{
			InputMovement();
			//InputColorChange();
		}
		else
		{
			SyncedMovement();
		}
	}

	private void InputMovement()
	{
		float moveHorizental = Input.GetAxis("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");


		
		Vector3 movement = new Vector3 (moveHorizental, 0.0f, moveVertical);
		rigidbody.AddForce(movement * speed);



	}

	void OnCollisionStay(Collision collisionInfo) {

		rigidbody.velocity = rigidbody.velocity * 0.95f;

//		Debug.Log ("colliding");
//
//		float moveHorizental = Input.GetAxis("Horizontal");
//		float moveVertical = Input.GetAxis ("Vertical");
//		Vector3 movement = new Vector3 (-moveHorizental, 0.0f, -moveVertical);
//		float otherSpeed = (speed * 1f);
//
//		rigidbody.AddForce(movement * otherSpeed);
	}
		
	private void SyncedMovement()
	{
		syncTime += Time.deltaTime;
		//rigidbody.position = syncStartPosition;
		//rigidbody.AddForce (syncEndVelocity);

		rigidbody.position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
	}

	
	private void InputColorChange()
	{
		if (Input.GetKeyDown(KeyCode.R))
			ChangeColorTo(new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
	}
	
	[RPC] void ChangeColorTo(Vector3 color)
	{
		renderer.material.color = new Color(color.x, color.y, color.z, 1f);
		
		if (networkView.isMine)
			networkView.RPC("ChangeColorTo", RPCMode.OthersBuffered, color);
	}
	

}
