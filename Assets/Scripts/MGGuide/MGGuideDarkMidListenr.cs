using UnityEngine;
using System.Collections;

public class MGGuideDarkMidListenr : MonoBehaviour {
    public bool isEnable;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnMouseDown()
    {
        if (!isEnable) return;
        if(MGGlobalDataCenter.defaultCenter().isLaterRoler)
            GameObject.Find("NetWork").GetComponent<MGGuideManager>().roleLaterGuideClick();
        else if (MGGlobalDataCenter.defaultCenter().isFrontRoler)
            GameObject.Find("NetWork").GetComponent<MGGuideManager>().roleFrontGuideClick();
    }
}
