using UnityEngine;
using System.Collections;

public class Down : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnMouseDown () {

        if(MGGlobalDataCenter.defaultCenter().isHost==true)
            MGNotificationCenter.defaultCenter().postNotification("downToLine", null);
        else
            MGNotificationCenter.defaultCenter().postNotification("1downToLine", null);
	}
}
