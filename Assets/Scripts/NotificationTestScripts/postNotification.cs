using UnityEngine;
using System.Collections;

public class postNotification : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
    public void onClick()
    {
        object objc=new object();
        MGNotificationCenter.defaultCenter().postNotification("testNotification", objc);
    }
	// Update is called once per frame
	void Update () {
	
	}
}
