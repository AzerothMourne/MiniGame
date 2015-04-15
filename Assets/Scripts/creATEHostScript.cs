using UnityEngine;
using System.Collections;

public class creATEHostScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
    public void OnMouseDown()
    {
        print("createHost");
        P2PBinding.createHost();
    }
	// Update is called once per frame
	void Update () {
	
	}
}
