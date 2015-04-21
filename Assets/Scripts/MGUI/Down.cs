using UnityEngine;
using System.Collections;

public class Down : MonoBehaviour {
	public Sprite upWardSprite;
    public GameObject upButton;

	private float timer;
	private bool isClick;

	// Use this for initialization
	void Start () {
		timer = 0.0f;
		isClick = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (isClick) {
			if(timer <= 0.25f){
				transform.localScale = new Vector3((transform.localScale.x-0.01f),(transform.localScale.y-0.01f),(transform.localScale.z-0.01f));
				timer+=Time.deltaTime;
			}
			else if(timer<=0.5f) {
				transform.localScale=new Vector3((transform.localScale.x+0.01f),(transform.localScale.y+0.01f),(transform.localScale.z+0.01f));
				timer+=Time.deltaTime;
			}
			else if(timer>0.5f){
				isClick=false;timer=0.0f;
				transform.localScale=new Vector3(1,1,1);
			}
		}
	}

	public void OnMouseDown () {
		isClick = true;

        //print("Down OnMouseDown time : " + MGGlobalDataCenter.timestamp());
        if(MGGlobalDataCenter.defaultCenter().isHost==true)
            MGNotificationCenter.defaultCenter().postNotification("downToLine", null);
        else
            MGNotificationCenter.defaultCenter().postNotification("1downToLine", null);

		//按向下后调出向上按钮
        upButton.GetComponent<SpriteRenderer>().sprite = upWardSprite;
	}
}
