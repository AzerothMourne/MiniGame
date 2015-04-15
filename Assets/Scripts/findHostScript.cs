using UnityEngine;
using System.Collections;

public class findHostScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
    public void OnMouseDown()
    {
        print("finsHost");
        P2PBinding.findHost();
    }
	// Update is called once per frame
	void Update () {
	
	}
}
