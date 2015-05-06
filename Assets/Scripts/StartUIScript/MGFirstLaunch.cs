using UnityEngine;
using System.Collections;

public class MGFirstLaunch : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        if (MGFoundtion.isFirstLaunch())
        {
            Debug.Log("isFirstLaunch=true");
            MGGlobalDataCenter.defaultCenter().isFirstLaunch = true;
            MGFoundtion.setFirstLaunchFlag();
        }
        else
        {
            Debug.Log("isFirstLaunch=false");
        }
	}
	
}
