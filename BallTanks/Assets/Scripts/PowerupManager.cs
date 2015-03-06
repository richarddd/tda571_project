using UnityEngine;
using System.Collections;

public class PowerupManager : MonoBehaviour {

	public GameObject myAudio;
	public Vector3 spawnPositionValues;

	void Awake(){
		while (true) {
			Vector3 spawnPos = getNewPosition ();
			gameObject.transform.position = spawnPos;
			Collider[] hitColliders = Physics.OverlapSphere (spawnPos, 0.3f);

			if (hitColliders.Length < 2) {
					break;
			}
		}
	}

	void OnTriggerEnter (Collider collider) {
		if (collider.gameObject.tag == "Player") {
			Network.Instantiate(myAudio, transform.position, transform.rotation,0);
			//Network.RemoveRPCs(networkView.viewID); 
			Network.Destroy(gameObject);
		}
	}


	Vector3 getNewPosition(){
		float xPos = Random.Range (-spawnPositionValues.x, spawnPositionValues.x);
		float yPos = spawnPositionValues.y;
		float zPos = Random.Range (-spawnPositionValues.z, spawnPositionValues.z);
		
		Vector3 spawnPosition = new Vector3 (xPos,yPos,zPos);
		return spawnPosition;
	}

}
