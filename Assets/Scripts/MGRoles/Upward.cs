using UnityEngine;
using System.Collections;

public class Upward : MonoBehaviour {
	public Sprite jumpSprite;
	private float timer;
	private bool isClick;
	string currentRole;
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

		//选定出当前角色，是前者还是后者，分别判断状态发出消息
		if(MGGlobalDataCenter.defaultCenter ().isHost == 1)
			currentRole = "role";
		else currentRole = "role1";

		//将向上的按钮变为跳的按钮
		this.GetComponent<SpriteRenderer>().sprite = jumpSprite;

		//当角色在线上时，发出跳的消息；当角色在线下时，发出向上的消息
		if (GameObject.Find (currentRole).GetComponent<Jump> ().isDown == 0) {
			if (MGGlobalDataCenter.defaultCenter ().isHost == 1)
				MGNotificationCenter.defaultCenter ().postNotification ("jump", null);
			else
				MGNotificationCenter.defaultCenter ().postNotification ("1jump", null);
		} 
		else {
			if(MGGlobalDataCenter.defaultCenter().isHost==1)
				MGNotificationCenter.defaultCenter().postNotification("upwardToLine", null);
			else
				MGNotificationCenter.defaultCenter().postNotification("1upwardToLine", null);
		}
	}
}
