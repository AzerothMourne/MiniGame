using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;
using System;
public static class EventEnum{
	public static string jumpFormerEventId="jump";
	public static string jumpLatterEventId="1jump";
	public static string downToLineFormerEventId="downToLine";
	public static string dowmToLineLatterEventId="1downToLine";
	public static string upwardToLineFormerEventId="upwardToLine";
	public static string upwardToLineLatterEventId="1upwardToLine";
    //技能事件
    public static string dart = "EventEnum_dart";
    public static string blink = "EventEnum_blink";
    public static string roadblock = "EventEnum_roadblock";
    public static string bones = "EventEnum_bones";
    public static string sprint = "EventEnum_sprint";
    public static string beatback = "EventEnum_beatback";
}
public class Jump : MonoBehaviour {

	public float forceMove ;
	public bool isGround;
	public float jumpVelocity ;
	public float jumpSecond ;
	public float jumpCount ;
    public MGSkillsBase drat,roadblock,blink,bones,sprint,beatback;
	public UIInput log;
	public int isReceiveFlag;
	
    private MGNetWorking mgNetWorking;

	//记录控制的当前角色动画，由于用的次数多，直接提取出来
	private Animator jumpAnim;
    private RoleAnimController roleAnimaController;
	// Use this for initialization
	void Start () {
        isGround = false;
		isReceiveFlag = 0;
        roleAnimaController = this.GetComponent<RoleAnimController>();
        mgNetWorking = GameObject.Find("NetWork").GetComponent<MGNetWorking>();

		//初始化动画
		jumpAnim = this.GetComponent<Animator> ();

		//获取角色的名字，role则是前面的角色，role1则是后面的角色
		//前面角色的动作
		if (this.gameObject.name == "role") {
			//print ("yes role");
            //注册动作事件
            //rolePlayer = GameObject.Find("roleFront");
			MGGlobalDataCenter.defaultCenter().role=this.gameObject;
			MGNotificationCenter.defaultCenter ().addObserver (this, jump, EventEnum.jumpFormerEventId);
			MGNotificationCenter.defaultCenter ().addObserver (this, downToLine, EventEnum.downToLineFormerEventId);
			MGNotificationCenter.defaultCenter ().addObserver (this, upwardToLine, EventEnum.upwardToLineFormerEventId);
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
			MGGlobalDataCenter.defaultCenter().roleLater=this.gameObject;
			MGNotificationCenter.defaultCenter().addObserver(this, jump, EventEnum.jumpLatterEventId);
			MGNotificationCenter.defaultCenter().addObserver(this, downToLine, EventEnum.dowmToLineLatterEventId);
			MGNotificationCenter.defaultCenter().addObserver(this, upwardToLine, EventEnum.upwardToLineLatterEventId);
            //注册技能事件
            MGNotificationCenter.defaultCenter().addObserver(this, useSkillsBlink, EventEnum.blink);
            MGNotificationCenter.defaultCenter().addObserver(this, useSkillsBones, EventEnum.bones);
            MGNotificationCenter.defaultCenter().addObserver(this, useSkillsSprint, EventEnum.sprint);
		}
	}
    private string objcToJson(string msg)
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
            Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            MGSkillBeatback skillObjc = null;
            if (Network.peerType != NetworkPeerType.Disconnected)
            {
                skillObjc = mgNetWorking.Instantiate(sprint, pos, new Quaternion(), 0) as MGSkillBeatback;
            }
            else
            {
                skillObjc = beatback.createSkillSprite(pos) as MGSkillBeatback;
            }
            if (skillObjc)
            {
                skillObjc.releaseSkillObjectName = this.gameObject.name;
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
                skillObjc.transform.parent = this.transform;
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
                skillObjc.releaseSkillObjcName = this.gameObject.name;
            }
        }
    }
    public void useSkillsBones(MGNotification notification)
    {
        if (notification.objc == null)
        {
            Vector3 pos = new Vector3(transform.position.x + 3 * renderer.bounds.size.x / 16, transform.position.y + (transform.localScale.y > 0 ? 1 : -1) * renderer.bounds.size.y / 2, transform.position.z);
            MGSkillBones skillObjc = bones as MGSkillBones;
            if (Network.peerType != NetworkPeerType.Disconnected)
            {
                skillObjc = mgNetWorking.Instantiate(bones, pos, new Quaternion(), 0) as MGSkillBones;
            }
            else
            {
                skillObjc = bones.createSkillSprite(pos) as MGSkillBones;
            }
            if (skillObjc)
            {
                skillObjc.releaseSkillObjcName = this.gameObject.name;
                skillObjc.transform.parent=this.transform;
            }
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
        print("useSkillsRoadblock");
        if (notification.objc == null && isGround)
        {
            Vector3 pos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            int isDown = transform.localScale.y > 0 ? 0 : 1;
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
        MGNotificationCenter.defaultCenter().postNotification(buttonEventId(RoleButtonEvent.upFormerEventId), null);
        if (transform.localScale.y < 0)
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
 		if (transform.localScale.y > 0) {
            MGNotificationCenter.defaultCenter().postNotification(buttonEventId(RoleButtonEvent.downFormerEventId), null);
            if (isGround)
            {
                rigidbody2D.gravityScale = 0f;
                collider2D.isTrigger = true;
            }
            else
            {
				rigidbody2D.gravityScale = 10f;
                collider2D.isTrigger = false;
			}
        }
		if(notification.objc==null){
			mgNetWorking.sendMessageToPeer (objcToJson(EventEnum.downToLineFormerEventId));
		}
    }

	public void upwardToLine(MGNotification notification)
	{

        rigidbody2D.gravityScale = 0f;
        collider2D.isTrigger = true;
		if(notification.objc==null){
			mgNetWorking.sendMessageToPeer (objcToJson(EventEnum.upwardToLineFormerEventId));
		}
	}
	// Update is called once per frame
	void Update () {
        
		//当下落过程中按了down按钮，则会在落地后下翻
        if (roleAnimaController.isPressDown && isGround)
        {
            downToLine(null);
		} 
	}
    
	//判断角色是否在地面上
    public void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.name == "road" || collision.gameObject.name == "roadSecond")
        {
            Debug.Log("OnCollisionEnter2D");
            isGround = true;
        }
	}
}
