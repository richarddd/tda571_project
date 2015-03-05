using UnityEngine;
using System.Collections;

public class SpawnPowerups: MonoBehaviour {
	
	public GameObject powerup;
	public GameObject powerupFreeze;
	public GameObject powerupGrow;
	public GameObject powerupShrink;
	public GameObject powerupHarmfulSphere;


	private float spawnWait;
	public float nextPowerupMinWait;
	public float nextPowerupMaxWait;
	
	void Start(){
		StartCoroutine (GeneratePowerUps ());
	}
	
	IEnumerator GeneratePowerUps(){
		while (true) {
			spawnWait = Random.Range (nextPowerupMinWait, nextPowerupMaxWait);
			yield return new WaitForSeconds (spawnWait);
			decidePowerup();
		}
	}

	void decidePowerup(){
		int number = Random.Range(1, 5);
		switch (number) {
		case 1:
			Spawn(powerupFreeze);
			break;
		case 2:
			Spawn(powerupHarmfulSphere);
			break;
		case 3:
			Spawn(powerupGrow);
			break;
		case 4: 
			Spawn(powerupShrink);
			break;
			
		}
	}

	void Spawn(GameObject powerup){
		Vector3 spawnPosition = new Vector3(-1,4,5);
		Quaternion spawnRotation = Quaternion.identity;
		Network.Instantiate (powerup, spawnPosition, spawnRotation, 0);
	}


}
