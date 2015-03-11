using UnityEngine;
using System.Collections;


public class NetworkManager : Singleton<NetworkManager>
{
		private const string typeName = "TDA5710";
		
	
		private bool isRefreshingHostList = false;
		private HostData[] hostList;
	
		public GameObject playerPrefab;
		public GameObject spawnPowerups;

		private string gameName;


		void Start(){
			gameName =  Preferences.GetGameName();
		}

	
	
		void OnGUI ()
		{


				if (!Network.isClient && !Network.isServer) {
						if (GUI.Button (new Rect (100, 100, 250, 100), "Start Server"))
								StartServer ();
			
						if (GUI.Button (new Rect (100, 250, 250, 100), "Refresh Hosts"))
								RefreshHostList ();

						gameName = GUI.TextField (new Rect (10, 10, 200, 20), gameName, 25);

			
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
				Preferences.SaveGameName(gameName);
				Network.InitializeServer (6, 25000, !Network.HavePublicAddress ());
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
				GameObject player = (GameObject)Network.Instantiate (playerPrefab, new Vector3(Random.Range(-5,5),5,Random.Range(-5,5)), Quaternion.identity, 0);
		}
		
		private void SpawnPowerupsManager ()
		{

			Instantiate (spawnPowerups, new Vector3(0,0,0) , Quaternion.identity);

		}
}
