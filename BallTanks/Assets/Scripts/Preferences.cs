using UnityEngine;
using System.Collections;

public class Preferences : MonoBehaviour{

	private const string PrefGameName = "pref_game_name";


	public static string GetGameName(){
		return PlayerPrefs.GetString(PrefGameName);
	}

	public static void SaveGameName(string name){
		PlayerPrefs.SetString(PrefGameName,name);
		PlayerPrefs.Save();
	}
}
