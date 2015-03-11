using UnityEngine;
using System.Collections;

public class RestartButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void Restart(){
		Network.Disconnect ();
		int levelNumber = Application.loadedLevel + 1;
		if (levelNumber < Application.levelCount) {
				
			Application.LoadLevel(levelNumber);
		}
		else{
			Application.LoadLevel(0);
		}

	}
}
