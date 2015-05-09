using UnityEngine;
using System.Collections;

public class roleWatchController : MonoBehaviour
{

	// Use this for initialization
	void Start () {
        if (this.gameObject.name == "role")
        {
            MGGlobalDataCenter.defaultCenter().roleFrontPos = transform.position;
        }
        else if (this.gameObject.name == "role1")
        {
            MGGlobalDataCenter.defaultCenter().roleLaterPos = transform.position;
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (transform.position.y < MGGlobalDataCenter.defaultCenter().roadOrignY - this.GetComponent<SpriteRenderer>().bounds.size.y - 0.5f)
        {
            transform.position = new Vector3(transform.position.x, MGGlobalDataCenter.defaultCenter().roadOrignY - this.GetComponent<SpriteRenderer>().bounds.size.y, transform.position.z);
            transform.localScale = new Vector3(1, -1, 1);
            transform.rigidbody2D.gravityScale = 0f;
        }
        transform.rotation = Quaternion.Euler(0, 0, 0);
	}
}
