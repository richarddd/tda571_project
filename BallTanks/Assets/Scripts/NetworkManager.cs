﻿using UnityEngine;
using System.Collections;


public class NetworkManager : Singleton<NetworkManager>
{
		private const string typeName = "TDA5710";
		private const string gameName = "ROOM99";
	
		private bool isRefreshingHostList = false;
		private HostData[] hostList;
	
		public GameObject playerPrefab;
		public GameObject mainCamera;
		public GameObject spawnPowerups;

	
	
		void OnGUI ()
		{
				if (!Network.isClient && !Network.isServer) {
						if (GUI.Button (new Rect (100, 100, 250, 100), "Start Server"))
								StartServer ();
			
						if (GUI.Button (new Rect (100, 250, 250, 100), "Refresh Hosts"))
								RefreshHostList ();

						//if (GUI.InputField(new Rect(100, 250, 250, 100), "Refresh Hosts"))
						//RefreshHostList();
			
						if (hostList != null) {
								for (int i = 0; i < hostList.Length; i++) {
										if (GUI.Button (new Rect (400, 100 + (110 * i), 300, 100), hostList [i].gameName))
												JoinServer (hostList [i]);
								}
						}
				}
		}
	
		private void StartServer ()
		{
				Network.InitializeServer (4, 25000, !Network.HavePublicAddress ());
				MasterServer.RegisterHost (typeName, gameName);

		}
	
		void OnServerInitialized ()
		{
				SpawnPlayer ();
				SpawnPowerupsManager ();
		}
	
	
		void Update ()
		{
				if (isRefreshingHostList && MasterServer.PollHostList ().Length > 0) {
						isRefreshingHostList = false;
						hostList = MasterServer.PollHostList ();
				}
		}
	
		private void RefreshHostList ()
		{
				if (!isRefreshingHostList) {
						isRefreshingHostList = true;
						MasterServer.RequestHostList (typeName);
				}
		}
	
	
		private void JoinServer (HostData hostData)
		{
				Network.Connect (hostData);
		}
	
		void OnConnectedToServer ()
		{

				SpawnPlayer ();
		}
	

		private void SpawnPlayer ()
		{

				GameObject.FindGameObjectWithTag ("Killzone").networkView.RPC ("PlayerJoined", RPCMode.AllBuffered);
				
				GameObject player = (GameObject)Network.Instantiate (playerPrefab, Vector3.up * 5, Quaternion.identity, 0);
				//player.networkView.viewID;
		}
		
		private void SpawnPowerupsManager ()
		{
			Network.Instantiate (spawnPowerups, new Vector3(0,0,0) , Quaternion.identity, 0);

		}
}
