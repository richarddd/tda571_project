using UnityEngine;
using System.Collections;

public class PlaySoundOnTriggerEnter : MonoBehaviour {

	public AudioClip boostSound;

void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player") {
			audio.PlayOneShot (boostSound);
		}
	}
}
