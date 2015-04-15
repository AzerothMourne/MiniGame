using UnityEngine;
using System.Collections;

public class Up : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnMouseDown () {
		print ("Up OnMouseDown ");
        MGNotificationCenter.defaultCenter().postNotification("firstJump", null);
	}
}
