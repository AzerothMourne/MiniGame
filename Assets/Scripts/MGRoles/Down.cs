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
		print ("Down OnMouseDown "+MGGlobalDataCenter.timestamp());
        if(MGGlobalDataCenter.defaultCenter().isHost==1)
            MGNotificationCenter.defaultCenter().postNotification("downToLine", null);
        else
            MGNotificationCenter.defaultCenter().postNotification("1downToLine", null);
	}
}
