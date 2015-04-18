using UnityEngine;
using System.Collections;

public class Upward : MonoBehaviour {
	public Sprite jumpSprite;
	private float timer;
	private bool isClick;
	// Use this for initialization
	void Start () {
		timer = 0.0f;
		isClick = false;
	}

	void Update(){
        
		if (isClick) {
			if(timer<=0.25f){
				transform.localScale=new Vector3((transform.localScale.x-0.01f),(transform.localScale.y-0.01f),(transform.localScale.z-0.01f));
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
	
	// Update is called once per frame
	public void OnMouseDown () {
		isClick = true;
		//将向上的按钮变为跳的按钮
		this.GetComponent<SpriteRenderer>().sprite = jumpSprite;

        if (MGGlobalDataCenter.defaultCenter().isHost == true)
            MGNotificationCenter.defaultCenter().postNotification("jump", null);
        else
            MGNotificationCenter.defaultCenter().postNotification("1jump", null);
	}
}
