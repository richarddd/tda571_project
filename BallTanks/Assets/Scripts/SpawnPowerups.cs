using UnityEngine;
using System.Collections;

public class SpawnPowerups: MonoBehaviour {
	
	public GameObject powerup;
	public GameObject emptyPowerup;
	public Vector3 spawnPositionValues;
	private float spawnWait;
	public float nextPowerupMinWait;
	public float nextPowerupMaxWait;
	
	void Start(){
		StartCoroutine (SpawnPowerUp ());
	}
	
	IEnumerator SpawnPowerUp(){
		while (true) {
			spawnWait = Random.Range (nextPowerupMinWait, nextPowerupMaxWait);
			yield return new WaitForSeconds (spawnWait);
			Vector3 spawnPosition = getSpawnPosition ();
			Quaternion spawnRotation = Quaternion.identity;

			//isColliding (spawnPosition, spawnRotation);

			Network.Instantiate (powerup, spawnPosition, spawnRotation, 0);
		}
	}

	Vector3 getSpawnPosition(){
		float xPos = Random.Range (-spawnPositionValues.x, spawnPositionValues.x);
		float yPos = spawnPositionValues.y;
		float zPos = Random.Range (-spawnPositionValues.z, spawnPositionValues.z);

		Vector3 spawnPosition = new Vector3 (xPos,yPos,zPos);
		return spawnPosition;
	}



	bool isColliding(Vector3 position, Quaternion rotation){
		Network.Instantiate (emptyPowerup, position, rotation, 0);

		//Debug.Log ("new object collides with something");
		return true;
	}
}
