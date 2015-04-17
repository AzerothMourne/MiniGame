using UnityEngine;
using System.Collections;

public class creATEHostScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
    public void OnMouseDown()
    {
        print("createHost");
        MGGlobalDataCenter.defaultCenter().isHost = true;
		MGNetWorking.createHost();
    }
	// Update is called once per frame
	void Update () {
	
	}
}
