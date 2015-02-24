using UnityEngine;
using System.Collections;

public class SpawnPowerups: MonoBehaviour {
	
	public GameObject powerup;
	//public GameObject emptyPowerup;

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
			Vector3 spawnPosition = new Vector3(-1,4,5);
			Quaternion spawnRotation = Quaternion.identity;
			Network.Instantiate (powerup, spawnPosition, spawnRotation, 0);
		}
	}

}
