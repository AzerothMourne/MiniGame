using UnityEngine;
using System.Collections;



public class bgnear : MonoBehaviour {


	public float speed;  
	private float movespeed;
	public float orginPositionX;  
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		movespeed = speed * Time.deltaTime;  
		transform.Translate(Vector3.left * movespeed, Space.World); //向左移动  
		if (transform.localPosition.x < MGGlobalDataCenter.defaultCenter().screenLiftX - transform.GetComponent<SpriteRenderer>().bounds.size.x/2)  
		{
            transform.localPosition = new Vector3(orginPositionX, transform.localPosition.y, transform.localPosition.z);  
		}  
	}
}
