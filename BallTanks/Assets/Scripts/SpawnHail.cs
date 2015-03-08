using UnityEngine;
using System.Collections;

public class SpawnHail : MonoBehaviour {

	public float MinInterval = 0.5f;
	public float DirectionDeviation = 1.0f;
	public GameObject[] HailPrefabs;

	private float timePassed = 0.0f;
	private float scaleFactor = 0.004f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		timePassed += Time.deltaTime;
		if (timePassed >= MinInterval)
		{
			//choose random rock from array and instantiate it at volcano
			int randomIndex = Random.Range(0, HailPrefabs.Length);
			GameObject newHail = Instantiate(HailPrefabs[randomIndex], transform.position, Random.rotation)  as GameObject;
			newHail.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
			Destroy(newHail.GetComponent<WallControl>());

			//add a rigidbody component to make the rock react to gravity and to be able to add forces to it
			Rigidbody newHailRigidbody = newHail.AddComponent<Rigidbody>();
			newHailRigidbody.mass = 0.008f;

			//get a random number to modify force direction
			float randomForceModifier = Random.Range(-DirectionDeviation, DirectionDeviation);
			Vector3 launchForce = new Vector3(2.0f + Random.Range(-1.0f, 1.0f), 0.08f, randomForceModifier);

			//get a random force position so the rock would spin in a different direction every time
			Vector3 randomPosition = Random.insideUnitSphere;
			newHailRigidbody.AddForceAtPosition(launchForce * (5.0f + Random.Range(-0.5f,0.5f)), randomPosition);

			//set convex=true on the rock's mesh collider to enable collisions with the terrain
			MeshCollider newHailMeshCollider = newHail.GetComponent<MeshCollider>();
			newHailMeshCollider.convex = true;

			timePassed = 0.0f;
		}
	}


}
