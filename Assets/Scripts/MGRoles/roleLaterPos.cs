using UnityEngine;
using System.Collections;

public class roleLaterPos : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log ("****************roleLaterPos = "+transform.position);
		MGGlobalDataCenter.defaultCenter ().roleLaterPos = transform.position;
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
