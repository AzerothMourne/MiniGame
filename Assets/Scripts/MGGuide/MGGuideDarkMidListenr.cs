﻿using UnityEngine;
using System.Collections;

public class MGGuideDarkMidListenr : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnMouseDown()
    {
        GameObject.Find("NetWork").GetComponent<MGGuideManager>().darkMidClick();
    }
}
