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
        if (MGGlobalDataCenter.defaultCenter().isHost == 1)
            MGNotificationCenter.defaultCenter().postNotification("useSkillsDart", null);
        else
            MGNotificationCenter.defaultCenter().postNotification("1useSkillsDart", null);
	}
}
