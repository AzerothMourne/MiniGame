using UnityEngine;
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
	// Use this for initialization
	void Start () {
        groundLayerMask = LayerMask.GetMask("Ground");
        isDown = 0;
        isGround = false;
		isReceiveFlag = 0;
		isPressDown = false;
		//forceMove = 50;
		//jumpVelocity = 25;
		//jumpSecond = 15;

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

    public void useSkillsDart(MGNotification notification)
    {
        GameObject role1 = this.gameObject;
		print("1");
        drat.createSkillSprite(new Vector3(role1.transform.position.x, role1.transform.position.y + (isDown==0?1:-1)*role1.renderer.bounds.size.y / 2, role1.transform.position.z));
		print("2");
		if (notification.objc == null) {
			MGMsgModel msgModel = new MGMsgModel ();
			if(MGGlobalDataCenter.defaultCenter().isHost == 1) 
				msgModel.eventId = "useSkillsDart";
			else msgModel.eventId = "1useSkillsDart";
			msgModel.timestamp = MGGlobalDataCenter.timestamp ();
			MGNetWorking.sendMessageToPeer (JsonMapper.ToJson (msgModel));
		}
    }
    public void jump(MGNotification notification)
    {
		//如果在地面上，则一段跳
		print ("time : " + System.DateTime.Now);
		print (" the role is : " + this.gameObject.name);
		if (isGround){
			Vector2 velocity = rigidbody2D.velocity;
			velocity.y = 0;
			print ("1:" + velocity.y);
			velocity.y = jumpVelocity;
			rigidbody2D.velocity = velocity;
			print ("1:" + rigidbody2D.velocity.y);
			jumpCount = 1;
			//如果没有发送给对方，则发送消息
			if (notification.objc == null) {
				//log.label.text+="jump send:" + MGGlobalDataCenter.timestamp ()+"\r\n";
				MGMsgModel msgModel = new MGMsgModel ();
				if(MGGlobalDataCenter.defaultCenter().isHost == 1) 
					msgModel.eventId = "jump";
				else msgModel.eventId = "1jump";
				print("eventId : "+msgModel.eventId);
				msgModel.timestamp = MGGlobalDataCenter.timestamp ();
				MGNetWorking.sendMessageToPeer (JsonMapper.ToJson (msgModel));
			} else {
				//log.label.text+="jump receive:" + MGGlobalDataCenter.timestamp ()+"\r\n";
				//isReceiveFlag=1;
				long time1 = MGGlobalDataCenter.timestamp ();
				long time2 = ((MGMsgModel)notification.objc).timestamp;
				print (time1 + ";" + time2);
				log.label.text += (Mathf.Abs (time1 - time2)).ToString () + "\r\n";
			}
		}
		//如果不在地面上，且一段跳了，则二段跳
		else if(!isGround && jumpCount == 1) {
			Vector2 velocity = rigidbody2D.velocity;
			if (velocity.y < -1.0f) velocity.y = jumpSecond + 3;
			else velocity.y = jumpSecond;
			rigidbody2D.velocity = velocity;
			jumpCount = 2;
		}
	}

    public void downToLine(MGNotification notification)
    {
        //角色会根据下按钮，翻转到线下
		print ("gravityScale : " + rigidbody2D.gravityScale);

 		if (rigidbody2D.gravityScale > 0) {
			print("downToLine isDown : "+isDown);
			if(isGround){
            	rigidbody2D.gravityScale = 0;
           		transform.localScale = new Vector3(1, -1, 1);
			} else if (!isGround) {
				print ("not on the ground");
				rigidbody2D.gravityScale = 10;

				isPressDown =true;
			}
			print ("is ground " + isGround);
			isDown = 1;
        }

		if(notification.objc==null){
			MGMsgModel msgModel = new MGMsgModel ();

			if(MGGlobalDataCenter.defaultCenter().isHost == 1) 
				msgModel.eventId = "downToLine";
			else msgModel.eventId = "1downToLine";
			msgModel.timestamp = MGGlobalDataCenter.timestamp ();
			MGNetWorking.sendMessageToPeer (JsonMapper.ToJson (msgModel));
		}
    }


	public void upwardToLine(MGNotification notification)
	{
		//角色会根据上按钮从线下到线上
		print ("up  gravityScale : " + rigidbody2D.gravityScale);
		if (rigidbody2D.gravityScale == 0)
        {
			print("upwardToLine isDown : "+isDown);
            transform.localScale = new Vector3(1, 1, 1);
            rigidbody2D.gravityScale = 5;
			isDown = 0;
        }

		if(notification.objc==null){
			MGMsgModel msgModel = new MGMsgModel ();
			
			if(MGGlobalDataCenter.defaultCenter().isHost == 1) 
				msgModel.eventId = "downToLine";
			else msgModel.eventId = "1downToLine";
			msgModel.timestamp = MGGlobalDataCenter.timestamp ();
			MGNetWorking.sendMessageToPeer (JsonMapper.ToJson (msgModel));
		}
	}

	// Update is called once per frame
	void Update () {
		if (isReceiveFlag == 1) {
			log.label.text+="Role Update :"+MGGlobalDataCenter.timestamp()+"\r\n";
			isReceiveFlag=0;
		}

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
		}
      
	}

	//判断角色是否在地面上
	public void OnCollisionEnter2D() {
		isGround = true;
	}

	public void OnCollisionExit2D() {
		isGround = false;
	}


}
