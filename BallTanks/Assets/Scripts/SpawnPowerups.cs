using UnityEngine;
using System.Collections;

public class SpawnPowerups: MonoBehaviour {
	
	public GameObject powerup;
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
			Vector3 spawnPosition = new Vector3 (Random.Range (-spawnPositionValues.x, spawnPositionValues.x), spawnPositionValues.y, Random.Range (-spawnPositionValues.z, spawnPositionValues.z));
			Quaternion spawnRotation = Quaternion.identity;
			Instantiate (powerup, spawnPosition, spawnRotation);
		}
	}
}
