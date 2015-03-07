using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {
	RectTransform canvasRectT;
	RectTransform healthBar;
	public GameObject objectToFollow;

	// Use this for initialization
	void Start () {
		canvasRectT = GameObject.FindGameObjectWithTag("Canvas").GetComponent<RectTransform>();
		//healthBar.anchoredPosition;
		//Debug.Log(healthBar.transform.position);
		healthBar =this.gameObject.GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update()
	{

		if(objectToFollow != null){
			Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, objectToFollow.transform.position);
			Debug.Log(screenPoint);
			//healthBar.anchoredPosition = screenPoint - canvasRectT.sizeDelta / 2f;
			healthBar.position = screenPoint;
		}
	}

	public void SetObjectToFollow(GameObject follow){
		objectToFollow = follow;
		//Debug.Log (objectToFollow.name);
	}
	public void SetHealthBarRectTransform(RectTransform rectTrans){
		//healthBar = rectTrans;
	}
}
