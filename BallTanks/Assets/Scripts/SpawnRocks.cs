using UnityEngine;
using System.Collections;

public class SpawnRocks : MonoBehaviour {

	public float MinInterval = 2.0f;
	public float DirectionDeviation = 0.2f;
	public GameObject[] RockPrefabs;

	private float timePassed = 0.0f;
	private bool isConnected = false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		timePassed += Time.deltaTime;
		if (timePassed >= MinInterval && isConnected)
		{
			//choose random rock from array and instantiate it at volcano
			int randomIndex = Random.Range(0, RockPrefabs.Length);
			GameObject newRock = Network.Instantiate(RockPrefabs[randomIndex], transform.position, Random.rotation,0)  as GameObject;

			//add a rigidbody component to make the rock react to gravity and to be able to add forces to it
			Rigidbody newRockRigidbody = newRock.AddComponent<Rigidbody>();
			newRockRigidbody.mass = 50;

			//get a random number to modify force direction
			float randomForceModifier = Random.Range(-DirectionDeviation, DirectionDeviation);
			Vector3 launchForce = new Vector3(transform.up.x + randomForceModifier, transform.up.y + randomForceModifier, transform.up.z);

			//get a random force position so the rock would spin in a different direction every time
			Vector3 randomPosition = Random.insideUnitSphere;
			newRockRigidbody.AddForceAtPosition(launchForce * (50000.0f + Random.Range(-5000,5000)), randomPosition);

			//set convex=true on the rock's mesh collider to enable collisions with the terrain
			MeshCollider newRockMeshCollider = newRock.GetComponent<MeshCollider>();
			newRockMeshCollider.convex = true;

			timePassed = 0.0f;
		}
	}

	void OnConnectedToServer() {
		isConnected = true;
	}

}
