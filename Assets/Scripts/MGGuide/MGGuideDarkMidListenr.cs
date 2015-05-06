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
        GameObject.Find("NetWork").GetComponent<MGGuideManager>().darkMidClick();
    }
}
