using UnityEngine;
using System.Collections;

public class Shooter : MonoBehaviour
{

		

		void OnSerializeNetworkView (BitStream stream, NetworkMessageInfo info)
		{
				
				if (stream.isWriting) {
						
						
				} else {
						
				}
		}

		

		void Update ()
		{

				if (networkView.isMine) {
		
						
				}
		}
}
