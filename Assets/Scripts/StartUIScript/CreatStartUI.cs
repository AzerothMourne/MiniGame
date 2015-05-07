using UnityEngine;
using System.Collections;

public class CreatStartUI : MonoBehaviour {
	public GameObject startObj;
	public int width, height;
	public float x_k,y_k;

	
	// Use this for initialization
	void Start () {
		x_k = MGGlobalDataCenter.defaultCenter ().pixelWidth / width;
		y_k = MGGlobalDataCenter.defaultCenter ().pixelHight / height;

		startObj = GameObject.Find("start");
        startObj.transform.localScale = new Vector3(x_k * startObj.transform.localScale.x, y_k * startObj.transform.localScale.y, 1);
		Debug.Log (startObj.transform.localScale);
	}

	// Update is called once per frame
	void Update () {
	
	}


}
