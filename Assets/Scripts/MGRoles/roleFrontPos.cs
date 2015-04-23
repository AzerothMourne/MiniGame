using UnityEngine;
using System.Collections;

public class roleFrontPos : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log ("****************roleFrontPos = "+transform.position);
		MGGlobalDataCenter.defaultCenter ().roleFrontPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
