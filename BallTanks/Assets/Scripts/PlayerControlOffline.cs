using UnityEngine;
using System.Collections;

public class PlayerControlOffline : MonoBehaviour {

	//public float maxSpeed;
	public float forceModifier = 500.0f;


	// Use this for initialization
	void Start () {
	
	}

	void FixedUpdate()
	{
		InputMovement();
	}

	private void InputMovement()
	{
		float moveSideways = Input.GetAxis("Horizontal");
		float moveForward = Input.GetAxis ("Vertical");

		// test if the player is actually standing over the terrain
		RaycastHit rayHit = new RaycastHit();
		if (Physics.Raycast (transform.position, -transform.up, out rayHit))
		{
			if(rayHit.collider.gameObject.name != "Terrain")
			{
				return;
			}
			else
			{
				AdjustPhysicsByTerrain ();
			}
		}

		// nudge the force position up by the diameter of the sphere to position it at the top,
		// adding a rolling force to the top of sphere gives a more realistic result.
		Vector3 forcePosition = transform.position + new Vector3 (0.0f, 0.5f, 0.0f);
		Vector3 forceDirection = new Vector3 (Camera.main.transform.right.x * moveSideways, 0.0f, Camera.main.transform.forward.z * moveForward);

		// normalize the direction so we get constant force in all directions
		forceDirection.Normalize ();

		// add a combined force with the calculated direction and position
		rigidbody.AddForceAtPosition (forceDirection * forceModifier * Time.deltaTime, forcePosition);

		// enable this to visualize the force position in real-time
		Debug.DrawRay (forcePosition, forceDirection * forceModifier * Time.deltaTime);
	}

	void AdjustPhysicsByTerrain ()
	{
		// adjust force modifier and angular drag according to texture index
		TextureDetector td = GetComponent<TextureDetector> ();
		int texture = td.GetMainTexture (transform.position);
		switch (texture) {
		case 0:
			//normal terrain, set normal friction
			rigidbody.angularDrag = 5.0f;
			forceModifier = 500.0f;
			break;
		case 1:
			//dry terrain, increase friction
			rigidbody.angularDrag = 12.0f;
			forceModifier = 300.0f;
			break;
		case 2:
			//lava, increase friction and do some damage
			rigidbody.angularDrag = 24.0f;
			forceModifier = 150.0f;
			break;
		default:
			rigidbody.angularDrag = 1.0f;
			forceModifier = 500.0f;
			break;
		}
	}
}
