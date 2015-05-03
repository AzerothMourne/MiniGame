using UnityEngine;
using System.Collections;

public class CreatOverUI : MonoBehaviour {
	public GameObject overObj1,overObj2,overObj3,overObj4;
	public int width, height;
	public float x_k,y_k;

	// Use this for initialization
	void Start () {
		x_k = MGGlobalDataCenter.defaultCenter ().pixelWidth / width;
		y_k = MGGlobalDataCenter.defaultCenter ().pixelHight / height;
		
		overObj1 = GameObject.Find("victoryFrontGameUI");
		overObj2 = GameObject.Find("failLaterGameUI");
		overObj3 = GameObject.Find("victoryLaterGameUI");
		overObj4 = GameObject.Find("failFrontGameUI");
		try{
			overObj1.transform.localScale = new Vector3 (x_k,y_k,1);
			overObj2.transform.localScale = new Vector3 (x_k,y_k,1);
			overObj3.transform.localScale = new Vector3 (x_k,y_k,1);
			overObj4.transform.localScale = new Vector3 (x_k,y_k,1);
		}catch{
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
