using UnityEngine;
using System.Collections;

public class creATEHostScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
    public void OnMouseDown()
    {
        print("createHost");
        MGGlobalDataCenter.defaultCenter().isHost = 1;
		MGNetWorking.createHost();
		print("createHost is : "+ MGGlobalDataCenter.defaultCenter().isHost);
    }
	// Update is called once per frame
	void Update () {
	
	}
}
