using UnityEngine;
using System.Collections;

public class Dart : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnMouseDown () {
		print ("Dart OnMouseDown ");
        MGNotificationCenter.defaultCenter().postNotification("useSkillsDart", null);
	}
}
