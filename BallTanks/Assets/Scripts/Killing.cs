using UnityEngine;
using System.Collections;

public class Killing : MonoBehaviour {
	private GameObject canvas;
	// Use this for initialization
	void Start () {
		canvas =GameObject.FindGameObjectWithTag("Canvas");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider colInfo){
		if (colInfo.tag == "Player" && colInfo.networkView.isMine) {

			int lifeLeft = colInfo.gameObject.GetComponent<PlayerLife>().GetNumberOfLifes();
			canvas.transform.GetChild(lifeLeft).gameObject.SetActive(false);
			lifeLeft--;

			//If the player have life left
			if (lifeLeft > 0){

				canvas.transform.GetChild(0).gameObject.SetActive(true);
				colInfo.gameObject.GetComponent<PlayerLife>().setNumberOfLifes(lifeLeft);

				colInfo.gameObject.SetActive(false);

				StartCoroutine(Respawn(colInfo.gameObject));

			}

			//If the player lost the last life
			else{
				canvas.transform.GetChild(4).gameObject.SetActive(true);
				Destroy(colInfo.gameObject);
			}
		}
	}

	//Used for respawning
	IEnumerator Respawn(GameObject colInfo) {

		yield return new WaitForSeconds(5);

		colInfo.transform.position = Vector3.zero;
		canvas.transform.GetChild(0).gameObject.SetActive(false);
		colInfo.gameObject.SetActive(true);
	}
}
