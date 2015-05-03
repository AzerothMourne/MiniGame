﻿using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;
using System;
public static class EventEnum{
	public static string jumpFormerEventId="jump";
	public static string jumpLatterEventId="1jump";
	public static string downToLineFormerEventId="downToLine";
	public static string dowmToLineLatterEventId="1downToLine";
    public static string gameoverEventId = "gameoverEventId";
    //技能事件
    public static string dart = "EventEnum_dart";
    public static string blink = "EventEnum_blink";
    public static string roadblock = "EventEnum_roadblock";
    public static string bones = "EventEnum_bones";
    public static string sprint = "EventEnum_sprint";
    public static string beatback = "EventEnum_beatback";
}
public class Jump : MonoBehaviour {

	public bool isGround;
	public float jumpVelocity ;
	public float jumpSecond ;
	public float jumpCount ;
    public MGSkillsBase drat,roadblock,blink,bones,sprint,beatback;
	public UIInput log;
    private bool isGameOver, isCollisionOver;
    public MGNetWorking mgNetWorking;
    public float roleSpeed;
	
	//记录控制的当前角色动画，由于用的次数多，直接提取出来
    private RoleAnimController roleAnimaController;
	// Use this for initialization
	void Start () {
        isCollisionOver = false;
        isGameOver = false;
        jumpCount = 0;
        isGround = false;
        roleAnimaController = this.GetComponent<RoleAnimController>();
        mgNetWorking = GameObject.Find("NetWork").GetComponent<MGNetWorking>();

		//获取角色的名字，role则是前面的角色，role1则是后面的角色
		//前面角色的动作
		if (this.gameObject.name == "role") {
			//print ("yes role");
            //注册动作事件
            //rolePlayer = GameObject.Find("roleFront");
            roleSpeed = 0;
			MGGlobalDataCenter.defaultCenter().role=this.gameObject;
			MGNotificationCenter.defaultCenter ().addObserver (this, jump, EventEnum.jumpFormerEventId);
			MGNotificationCenter.defaultCenter ().addObserver (this, downToLine, EventEnum.downToLineFormerEventId);
            //注册技能事件
            MGNotificationCenter.defaultCenter().addObserver(this, useSkillsDart, EventEnum.dart);
            MGNotificationCenter.defaultCenter().addObserver(this, useSkillsRoadblock, EventEnum.roadblock);
            MGNotificationCenter.defaultCenter().addObserver(this, useSkillsBeatback, EventEnum.beatback);
		} 
		//后面的角色动作
		else if(this.gameObject.name == "role1"){
			//print ("yes role1");
            //注册动作事件
            //rolePlayer = GameObject.Find("roleLater");
            roleSpeed = 1f / 30f;
			MGGlobalDataCenter.defaultCenter().roleLater=this.gameObject;
			MGNotificationCenter.defaultCenter().addObserver(this, jump, EventEnum.jumpLatterEventId);
			MGNotificationCenter.defaultCenter().addObserver(this, downToLine, EventEnum.dowmToLineLatterEventId);
            //注册技能事件
            MGNotificationCenter.defaultCenter().addObserver(this, useSkillsBlink, EventEnum.blink);
            MGNotificationCenter.defaultCenter().addObserver(this, useSkillsBones, EventEnum.bones);
            MGNotificationCenter.defaultCenter().addObserver(this, useSkillsSprint, EventEnum.sprint);
			//@test
			useSkillsBones(new MGNotification("123", null, null));
		}
	}
    public string objcToJson(string msg)
    {
        //log.label.text+="jump send:" + MGGlobalDataCenter.timestamp ()+"\r\n";
        MGMsgModel msgModel = new MGMsgModel();
        if (MGGlobalDataCenter.defaultCenter().isHost == true)
            msgModel.eventId = msg;
        else msgModel.eventId = "1"+msg;
        return JsonMapper.ToJson(msgModel);
    }
    public void useSkillsBeatback(MGNotification notification)
    {
        if (notification.objc == null)
        {
            Vector3 pos = new Vector3(11f, 0, 0);
            MGSkillBeatback skillObjc = null;
            if (Network.peerType != NetworkPeerType.Disconnected)
            {
                skillObjc = mgNetWorking.Instantiate(beatback, pos, new Quaternion(), 0) as MGSkillBeatback;
            }
            else
            {
                skillObjc = beatback.createSkillSprite(pos) as MGSkillBeatback;
            }
            if (skillObjc)
            {
                skillObjc.releaseSkillObjectName = this.gameObject.name;
                skillObjc.transform.parent = this.transform;
            }
        }
    }
    public void useSkillsSprint(MGNotification notification)
    {
        if (notification.objc == null)
        {
            Vector3 pos = new Vector3(transform.position.x + 3 * renderer.bounds.size.x / 16, transform.position.y + (transform.localScale.y > 0  ? 1 : -1) * renderer.bounds.size.y / 2, transform.position.z);
            MGSkillSprint skillObjc = null;
            if (Network.peerType != NetworkPeerType.Disconnected)
            {
                skillObjc = mgNetWorking.Instantiate(sprint, pos, new Quaternion(), 0) as MGSkillSprint;
            }
            else
            {
                skillObjc = sprint.createSkillSprite(pos) as MGSkillSprint;
            }
            if (skillObjc)
            {
                skillObjc.releaseSkillObjectName = this.gameObject.name;
            }
        }
    }
    public void useSkillsBlink(MGNotification notification)
    {
        if (notification.objc == null)
        {
            Vector3 pos = new Vector3(transform.position.x, transform.position.y , transform.position.z);
            MGSkillBlink skillObjc = null;
            if (Network.peerType != NetworkPeerType.Disconnected)
            {
                skillObjc = mgNetWorking.Instantiate(blink, pos, new Quaternion(), 0) as MGSkillBlink;
            }
            else
            {
                skillObjc = blink.createSkillSprite(pos) as MGSkillBlink;
            }
            if (skillObjc)
            {
                skillObjc.releaseSkillObjectName = this.gameObject.name;
            }
        }
    }
    public void useSkillsBones(MGNotification notification)
    {
        Vector3 pos = new Vector3(transform.position.x + 3 * renderer.bounds.size.x / 16, transform.position.y + (transform.localScale.y > 0 ? 1 : -1) * renderer.bounds.size.y / 2, transform.position.z);
        MGSkillBones skillObjc = bones as MGSkillBones;
        if (notification.objc == null)//需要在对方客户端同步的
        {
            if (Network.peerType != NetworkPeerType.Disconnected)
            {
                skillObjc = mgNetWorking.Instantiate(bones, pos, new Quaternion(), 0) as MGSkillBones;
            }
            else
            {
                skillObjc = bones.createSkillSprite(pos) as MGSkillBones;
            } 
        }
        else//不需要再对方客户端同步的,短时间的bones
        {
            skillObjc = bones.createSkillSprite(pos) as MGSkillBones;
            skillObjc.name = notification.objc as string;
        }
        if (skillObjc)
        {
            skillObjc.releaseSkillObjcName = this.gameObject.name;
            skillObjc.transform.parent = this.transform;
        }
    }
    public void useSkillsDart(MGNotification notification)
    {
		if (notification.objc == null) {
            Vector3 pos = new Vector3(transform.position.x, transform.position.y + (transform.localScale.y > 0 ? 1 : -1) * renderer.bounds.size.y / 2, transform.position.z);
            MGskillDrat skillObjc = bones as MGskillDrat;
            if (Network.peerType != NetworkPeerType.Disconnected)
            {
                mgNetWorking.Instantiate(drat, pos, new Quaternion(), 0);
            }
            else
            {
                skillObjc = drat.createSkillSprite(pos) as MGskillDrat;
            }
            if (skillObjc)
            {
                skillObjc.releaseSkillObjectName = this.gameObject.name;
            }
		}
    }
    public void useSkillsRoadblock(MGNotification notification)
    {
        if (notification.objc == null)
        {
			int isDown = transform.localScale.y > 0 ? 0 : 1;
			Debug.Log(MGGlobalDataCenter.defaultCenter().roadOrignY);
			Vector3 pos = new Vector3(transform.position.x,MGGlobalDataCenter.defaultCenter().roadOrignY-(isDown==1?0.175f:0f), transform.position.z);
            if (Network.peerType != NetworkPeerType.Disconnected)
            {
                mgNetWorking.Instantiate(roadblock, pos, Quaternion.Euler(0, isDown*180, isDown*180), 0);
            }
            else
            {
                roadblock.createSkillSprite(pos, Quaternion.Euler(0, isDown * 180, isDown * 180));
            }
        }
    }
    public string buttonEventId(string eventId)
    {
        string ans = eventId;
        if (this.gameObject.name == "role1")
        {
            ans = "1" + ans;
        }
        return ans;
    }
    public void jump(MGNotification notification)
    {
        MGNotificationCenter.defaultCenter().postNotification(buttonEventId(RoleButtonEvent.upFormerEventId), notification);
        //if (roleAnimaController.isRoll || roleAnimaController.isPressDown) return;
        if (transform.localScale.y < 0 || roleAnimaController.isRoll)
        {
			Debug.Log("jump");
            if (notification.objc == null)
            {
                mgNetWorking.sendMessageToPeer(objcToJson(EventEnum.jumpFormerEventId));
            }
            return;
        }
        if (roleAnimaController.isFirstJump && !roleAnimaController.isSecondJump && jumpCount==0)
        {
            Debug.Log("一段跳");
            isGround = false;
			Vector3 velocity = rigidbody2D.velocity;
			velocity.y = jumpVelocity;
			rigidbody2D.velocity = velocity;
			jumpCount = 1;
			//如果没有发送给对方，则发送消息
			if (notification.objc == null) {
				mgNetWorking.sendMessageToPeer (objcToJson(EventEnum.jumpFormerEventId));
			}
		}
		//如果不在地面上，且一段跳了，则二段跳
        else if (roleAnimaController.isFirstJump && roleAnimaController.isSecondJump && jumpCount==1)
        {
            Debug.Log("二段跳");
			Vector3 velocity = rigidbody2D.velocity;
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
        MGNotificationCenter.defaultCenter().postNotification(buttonEventId(RoleButtonEvent.downFormerEventId), null);
		if(notification.objc==null){
			mgNetWorking.sendMessageToPeer (objcToJson(EventEnum.downToLineFormerEventId));
		}
    }
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (MGGlobalDataCenter.defaultCenter().isHost == true)
                MGNotificationCenter.defaultCenter().postNotification("downToLine", null);
            else
                MGNotificationCenter.defaultCenter().postNotification("1downToLine", null);
        }
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			if (MGGlobalDataCenter.defaultCenter().isHost == true)
				MGNotificationCenter.defaultCenter().postNotification("jump", null);
			else
				MGNotificationCenter.defaultCenter().postNotification("1jump", null);
		}
        if (roleSpeed != 0)
        {
            float dis = MGGlobalDataCenter.defaultCenter().roleFrontPos.x - MGGlobalDataCenter.defaultCenter().roleLaterPos.x;
            transform.Translate(Vector3.right * dis * roleSpeed * Time.deltaTime);
        }
        if (!isGameOver)
        {
            gameOver();
        }
	}
    public void gameOver()
    {
        
        Vector3 roleLaterPos = MGGlobalDataCenter.defaultCenter().roleLater.transform.position;
        Vector3 roleFrontPos = MGGlobalDataCenter.defaultCenter().role.transform.position;
        if (roleFrontPos.x - roleLaterPos.x < 1.0f || isCollisionOver)//后者追上前者结束
        {
			MGGlobalDataCenter.defaultCenter ().isKillMingyue = true;
            isGameOver = true;
            //强制roleLater出现在role的后面一点点。
            MGGlobalDataCenter.defaultCenter().roleLater.transform.localScale = MGGlobalDataCenter.defaultCenter().role.transform.localScale;
            if (MGGlobalDataCenter.defaultCenter().roleLater.transform.localScale.y < 0)
            {
                MGGlobalDataCenter.defaultCenter().roleLater.rigidbody2D.gravityScale = 0f;
            }
            roleLaterPos.x = roleFrontPos.x - 1f;
            roleLaterPos.y = roleFrontPos.y = MGGlobalDataCenter.defaultCenter().roadOrignY;
            MGGlobalDataCenter.defaultCenter().roleLater.transform.position = roleLaterPos;
            MGGlobalDataCenter.defaultCenter().role.transform.position = roleFrontPos;

            if (this.gameObject.name == "role1")
                MGNotificationCenter.defaultCenter().postNotification(RoleButtonEvent.killLatterEventId, this.gameObject.name);
        }
        if (this.gameObject.name == "role1" && transform.position.x + 1.4 < MGGlobalDataCenter.defaultCenter().screenLiftX)//后者出屏幕结束，还有一个后者死掉结束在RoleAnimController里
        {
            isGameOver = true;
            //切换场景
            Debug.Log("role1 out of screen");
            MGGlobalDataCenter.defaultCenter().overSenceUIName = "victoryFrontGameUI";
            Application.LoadLevel("overSence");
            MGMsgModel gameoverModel = new MGMsgModel();
            gameoverModel.eventId = EventEnum.gameoverEventId;
            gameoverModel.gameobjectName = MGGlobalDataCenter.defaultCenter().overSenceUIName;
            mgNetWorking.sendMessageToPeer(JsonMapper.ToJson(gameoverModel));
        }
    }
	//判断角色是否在地面上
    public void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.name == "road" || collision.gameObject.name == "roadSecond")
        {
            if (MGGlobalDataCenter.defaultCenter().roadOrignY == -1000)
                MGGlobalDataCenter.defaultCenter().roadOrignY = transform.position.y;
            isGround = true;
            jumpCount = 0;
        }
        if (collision.gameObject.name == "role" || collision.gameObject.name == "role1")
        {
            isCollisionOver = true;
            GameObject sprint= GameObject.Find("sprint(Clone)");
            if (sprint != null)
            {
                Destroy(sprint);
            }
            if (collision.gameObject.name == "role")
            {
				Debug.Log("set role trigger");
                if (roleAnimaController.downOrUp)
                    rigidbody2D.gravityScale = 0.5f;
                else
                    rigidbody2D.gravityScale = 0f;
                rigidbody2D.velocity = Vector3.zero;
                collider2D.isTrigger = true;
            }
            else if (collision.gameObject.name == "role1")
            {
                GameObject.Find("MGSkillEffect").GetComponent<MGSkillEffect>().speedSwitch = 0;
            }
            gameOver();
        }
	}
    void OnDestroy()
    {
        MGNotificationCenter.defaultCenter().removeObserver(this);
    }
}
