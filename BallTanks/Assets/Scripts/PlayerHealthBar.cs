using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour {
		
	private GameObject player;
	public int currentHealth;
	GameObject killZone;
		
	private int barWidth;
	private int barHeight;
	private float barX;
	private float barY;
	private Texture2D emptyBarTex;
	private Texture2D healthBarTex;
	private Vector2 playerPos;
	private SphereCollider playerHitbox;
		
		// Use this for initialization
	void Start () {
		player = transform.gameObject;
		killZone= GameObject.FindGameObjectWithTag("Killzone");


			
		currentHealth = 100;
			
		playerHitbox = gameObject.GetComponentInChildren<SphereCollider>();
			
		//Create a red texture used for displaying background.
		calculateBarSize();
		healthBarTex = new Texture2D(barWidth, barHeight, TextureFormat.ARGB32, false);
		for (int x = 0; x <= barWidth; x++)
		{
			for (int y = 0; y <= barHeight; y++)
			{
				healthBarTex.SetPixel(x, y, Color.red);
			}
		}
		healthBarTex.Apply();
			
		//Create a red texture used for displaying background.
		emptyBarTex = new Texture2D(barWidth, barHeight, TextureFormat.ARGB32, false);
		for (int x = 0; x <= barWidth; x++)
		{
			for (int y = 0; y <= barHeight; y++)
			{
				emptyBarTex.SetPixel(x, y, Color.black);
			}
		}
		emptyBarTex.Apply();
			
	}

	void Awake ()
	{
		networkView.observed = this;
	}

	void OnSerializeNetworkView (BitStream stream, NetworkMessageInfo info)
	{
		int syncedHealth = 0;
		if(stream.isWriting){
			syncedHealth = currentHealth;
			stream.Serialize (ref syncedHealth);
		}else{
			stream.Serialize (ref syncedHealth);
			currentHealth = syncedHealth;
		}

	}
		
		// Update is called once per frame
	void Update () {
		if(networkView.isMine){
			//Debug.Log("=========");
			//Debug.Log(currentHealth);
			if(currentHealth <= 0) {
				killZone.GetComponent<Killing>().Kill(this.gameObject.transform.GetChild(0).gameObject);
				Debug.Log("dead");
					
			}
		}
	}
		
		//Draw gui components
	void OnGUI()
	{
		calculateBarSize();
			
		//Since we want the health bar above the player, we need to factor in the players height when getting the position.
		Vector3 tmp = transform.GetChild(0).position;
		tmp.y += playerHitbox.radius *1.5f;
			
		playerPos = Camera.main.WorldToScreenPoint(tmp);
		barX = playerPos.x - barWidth / 2;
		barY = Screen.height - playerPos.y;
			
			//Draw the health bar
		GUI.DrawTexture(new Rect(barX, barY, barWidth, barHeight), emptyBarTex);
		GUI.DrawTexture(new Rect(barX, barY, barWidth * currentHealth / 100, barHeight), healthBarTex);
	}
		
		
		//Return current health
	public int getCurrentHealth() {
		return currentHealth;
	}
		
		//Set currentHealth
	public void setCurrentHealth(int newHealth) {
		currentHealth = newHealth;
	}
		
		//Increment health by given value
	public void incrementHealth(int value) {
		currentHealth += value;
		StartCoroutine(blinkPlayerColor (0.1f, Color.green));
	}
		
		//Decrement health by given value
	public void decrementHealth(int value) {
		currentHealth -= value;
		StartCoroutine(blinkPlayerColor (0.1f, Color.red));
	}

	public IEnumerator blinkPlayerColor(float time, Color color)
	{
		for (int i = 0; i < 5; i++) 
		{
			gameObject.transform.GetChild (0).renderer.material.color = color;
			yield return new WaitForSeconds (time);
			gameObject.transform.GetChild (0).renderer.material.color = Color.white;
			yield return new WaitForSeconds (time);
		}
	}
		

		
	private void calculateBarSize()
	{
		barWidth = Screen.width / 16;
		barHeight = Screen.height / 80;
	}


}
