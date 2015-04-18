﻿using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;
using System;
public static class EventEnum{
	public static string dartFormerEventId="useSkillsDart";
	public static string dartLatterEventId="1useSkillsDart";
	public static string jumpFormerEventId="jump";
	public static string jumpLatterEventId="1jump";
	public static string downToLineFormerEventId="downToLine";
	public static string dowmToLineLatterEventId="1downToLine";
	public static string upwardToLineFormerEventId="upwardToLine";
	public static string upwardToLineLatterEventId="1upwardToLine";


}
public class Jump : MonoBehaviour {

	public float forceMove ;
	private bool isGround ;
	public float jumpVelocity ;
	public float jumpSecond ;
	public float jumpCount ;
	public int isDown;
    public MGskillDrat drat;
    private int groundLayerMask;
	public UIInput log;
	public int isReceiveFlag;
	public bool isPressDown;
    private MGNetWorking mgNetWorking;
	// Use this for initialization
	void Start () {
        groundLayerMask = LayerMask.GetMask("Default");
        isDown = 0;
        isGround = false;
		isReceiveFlag = 0;
		isPressDown = false;
        mgNetWorking = GameObject.Find("Main Camera").GetComponent<MGNetWorking>();

		//获取角色的名字，role则是前面的角色，role1则是后面的角色

		//前面角色的动作
		if (this.gameObject.name == "role") {
			print ("yes role");
			MGNotificationCenter.defaultCenter ().addObserver (this, useSkillsDart, EventEnum.dartFormerEventId);
			MGNotificationCenter.defaultCenter ().addObserver (this, jump, EventEnum.jumpFormerEventId);
			MGNotificationCenter.defaultCenter ().addObserver (this, downToLine, EventEnum.downToLineFormerEventId);
			MGNotificationCenter.defaultCenter ().addObserver (this, upwardToLine, EventEnum.upwardToLineFormerEventId);
		} 
		//后面的角色动作
		else if(this.gameObject.name == "role1"){
			print ("yes role1");
			MGNotificationCenter.defaultCenter().addObserver(this, useSkillsDart, EventEnum.dartLatterEventId);
			MGNotificationCenter.defaultCenter().addObserver(this, jump, EventEnum.jumpLatterEventId);
			MGNotificationCenter.defaultCenter().addObserver(this, downToLine, EventEnum.dowmToLineLatterEventId);
			MGNotificationCenter.defaultCenter().addObserver(this, upwardToLine, EventEnum.upwardToLineLatterEventId);
		}
	}
    private string objcToJson(string msg)
    {
        //log.label.text+="jump send:" + MGGlobalDataCenter.timestamp ()+"\r\n";
        MGMsgModel msgModel = new MGMsgModel();
        if (MGGlobalDataCenter.defaultCenter().isHost == true)
            msgModel.eventId = msg;
        else msgModel.eventId = "1"+msg;
        //print("eventId : "+msgModel.eventId);
        msgModel.timestamp = MGGlobalDataCenter.timestamp();
        return JsonMapper.ToJson(msgModel);
    }
    public void useSkillsDart(MGNotification notification)
    {
		if (notification.objc == null) {
            GameObject role1 = this.gameObject;
            Vector3 pos=new Vector3(role1.transform.position.x, role1.transform.position.y + (isDown==0?1:-1)*role1.renderer.bounds.size.y / 2, role1.transform.position.z);
            if (Network.peerType != NetworkPeerType.Disconnected)
            {
                mgNetWorking.Instantiate(drat, pos, new Quaternion(), 0);
            }
            else
            {
                drat.createSkillSprite(pos);
            }
		}
    }
    public void jump(MGNotification notification)
    {
        print("is ground " + isGround);
        if (isDown == 1)
        {
            upwardToLine(notification);
            return;
        }
		if (isGround){
            isGround = false;
			Vector2 velocity = rigidbody2D.velocity;
			velocity.y = jumpVelocity;
			rigidbody2D.velocity = velocity;
			jumpCount = 1;
			//如果没有发送给对方，则发送消息
			if (notification.objc == null) {
				mgNetWorking.sendMessageToPeer (objcToJson(EventEnum.jumpFormerEventId));
			}
		}
		//如果不在地面上，且一段跳了，则二段跳
		else if(!isGround && jumpCount == 1) {
			Vector2 velocity = rigidbody2D.velocity;
			if (velocity.y < -1.0f) velocity.y = jumpSecond + 3;
			else velocity.y = jumpSecond;
			rigidbody2D.velocity = velocity;
			jumpCount = 2;
            if (notification.objc == null)
            {
                mgNetWorking.sendMessageToPeer(objcToJson(EventEnum.jumpFormerEventId));
            }
		}
	}

    public void downToLine(MGNotification notification)
    {
        //角色会根据下按钮，翻转到线下
        print("is ground " + isGround);
 		if (isDown == 0) {
			if(isGround){
            	rigidbody2D.gravityScale = 0;
           		transform.localScale = new Vector3(1, -1, 1);
                isDown = 1;
			} else if (!isGround) {
				rigidbody2D.gravityScale = 10;
				isPressDown =true;
			}
        }

		if(notification.objc==null){
			mgNetWorking.sendMessageToPeer (objcToJson(EventEnum.downToLineFormerEventId));
		}
    }


	public void upwardToLine(MGNotification notification)
	{
        
		if (isDown == 1)
        {
            transform.localScale = new Vector3(1, 1, 1);
            rigidbody2D.gravityScale = 5;
			isDown = 0;
            isGround = true;
        }
		if(notification.objc==null){
			mgNetWorking.sendMessageToPeer (objcToJson(EventEnum.upwardToLineFormerEventId));
		}
	}

	// Update is called once per frame
	void Update () {

		//判断键盘输入左右键， 用来说明位移
		float h = Input.GetAxis ("Horizontal");
		//float h = 0.1f;
		if (h > 0.05f) {
			//rigidbody2D.AddForce (Vector2.right * forceMove);
			if(rigidbody2D.gravityScale==0)
				rigidbody2D.AddForce (Vector2.right * (forceMove-40));
			else rigidbody2D.AddForce (Vector2.right * forceMove);
		} else if (h < -0.05f) {
			if(rigidbody2D.gravityScale==0)
				rigidbody2D.AddForce (-Vector2.right * (forceMove-40));
			else rigidbody2D.AddForce (-Vector2.right * forceMove);
		}


		//角色会根据左右键反转
		if (h > 0.05f) {
			transform.localScale = new Vector3(1,rigidbody2D.gravityScale==0?-1:1,1);
		} else if (h < -0.05f) {
			transform.localScale = new Vector3(-1,rigidbody2D.gravityScale==0?-1:1,1);
		}
        
		//当下落过程中按了down按钮，则会在落地后下翻
		if (isPressDown && isGround) {
			rigidbody2D.gravityScale = 0;
			transform.localScale = new Vector3(1, -1, 1);
			isPressDown = false;
            isDown = 1;
		}
      
	}
    
	//判断角色是否在地面上
    public void OnCollisionEnter2D(Collision2D collision)
    {
        //print("OnCollisionEnter2D:" + collision.gameObject.name);
        if (collision.gameObject.name == "road" || collision.gameObject.name == "roadSecond")
        {
            isGround = true;
        }
	}

}
