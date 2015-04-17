using UnityEngine;
using System.Collections;
using LitJson;
public class Up : MonoBehaviour {

	public UIInput log;
	// Use this for initialization
	void Start () {
		log.label.text = "";

	}
	
	// Update is called once per frame
	void Update () {

	}

	public void OnMouseDown () {

		//log.label.text += "up down:" + MGGlobalDataCenter.timestamp ()+"\r\n";
        if (MGGlobalDataCenter.defaultCenter().isHost == true)
            MGNotificationCenter.defaultCenter().postNotification("firstJump", null);
        else
            MGNotificationCenter.defaultCenter().postNotification("1firstJump", null);
	}
}
