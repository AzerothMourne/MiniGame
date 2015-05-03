using UnityEngine;
using System.Collections;

public class flyOut : MonoBehaviour {
    public float y;
    public int flag;
	// Use this for initialization
	void Start () {
        y = 0;
        flag = 0;
	}
	
	// Update is called once per frame
	void Update () {
        y += 0.05f;
            this.transform.Translate(new Vector3(Random.Range(1f, 2f), Mathf.Sin(y), 0) * Random.Range(2f, 3f) * Time.deltaTime);
           //this.transform.Translate(new Vector3(Random.Range(1f, 2f), Random.Range(0.1f, 0.2f) * Time.deltaTime));
        
        if (this.transform.position.x > MGFoundtion.pixelToWroldPoint(MGGlobalDataCenter.defaultCenter().pixelWidth, 0).x)
        {
            Destroy(this.gameObject);
        }
	}
}
