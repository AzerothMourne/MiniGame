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
        MGGlobalDataCenter.defaultCenter().isMingYueGuide = MGFoundtion.isMingYueGuide();
        MGGlobalDataCenter.defaultCenter().isTianYaGuide = MGFoundtion.isTianYaGuide();
        Debug.Log("MGGlobalDataCenter.defaultCenter().isMingYueGuide=" + MGGlobalDataCenter.defaultCenter().isMingYueGuide);
        Debug.Log("MGGlobalDataCenter.defaultCenter().isTianYaGuide=" + MGGlobalDataCenter.defaultCenter().isTianYaGuide);
	}
	
}
