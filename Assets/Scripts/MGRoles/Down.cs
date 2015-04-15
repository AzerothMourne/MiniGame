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
		print ("Down OnMouseDown ");
        MGNotificationCenter.defaultCenter().postNotification("downToLine", null);
	}
}
