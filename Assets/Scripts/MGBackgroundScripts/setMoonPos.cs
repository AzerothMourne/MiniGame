using UnityEngine;
using System.Collections;

public class setMoonPos : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.position = new Vector3 (transform.position.x, GameObject.Find("Main Camera").GetComponent<Camera>().orthographicSize, transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
