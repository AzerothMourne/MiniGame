using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;
using System;
public static class RoleActEventEnum{
	public static string jumpFormerEventId="jump";
	public static string jumpLatterEventId="1jump";
	public static string downToLineFormerEventId="downToLine";
	public static string dowmToLineLatterEventId="1downToLine";
    public static string gameoverEventId = "gameoverEventId";

}
public static class roleState{
	public static int bone = 1 << 0;
	public static int sprint = 1 << 1;
    public static int wudi = 1 << 30;
}
public class Jump : MonoBehaviour {

	public bool isGround;
	public float jumpVelocity ;
	public float jumpSecond ;
	public float jumpCount ;
    public MGSkillsBase drat,roadblock,blink,bones,sprint,beatback;
	public UIInput log;
    public bool isCollisionOver;
    public MGNetWorking mgNetWorking;
    public float roleSpeed;
	public int stateMask;
	
	//记录控制的当前角色动画，由于用的次数多，直接提取出来
    private RoleAnimController roleAnimaController;
	// Use this for initialization
    void Awake()
    {
        //获取角色的名字，role则是前面的角色，role1则是后面的角色
        //前面角色的动作
        if (this.gameObject.name == "role")
        {
            //print ("yes role");
            //注册动作事件
            //rolePlayer = GameObject.Find("roleFront");
            if (MGGlobalDataCenter.defaultCenter().isSingle && MGGlobalDataCenter.defaultCenter().isLaterRoler)
            {
                this.gameObject.AddComponent<MGRoleActAIController>();
                this.gameObject.AddComponent<MGRoleFrontSkillAIController>();
            }
                
            roleSpeed = 0;
            MGGlobalDataCenter.defaultCenter().role = this.gameObject;
            MGNotificationCenter.defaultCenter().addObserver(this, jump, RoleActEventEnum.jumpFormerEventId);
            MGNotificationCenter.defaultCenter().addObserver(this, downToLine, RoleActEventEnum.downToLineFormerEventId);
            //注册技能事件
            MGNotificationCenter.defaultCenter().addObserver(this, useSkillsDart, SkillActEventEnum.dart);
            MGNotificationCenter.defaultCenter().addObserver(this, useSkillsRoadblock, SkillActEventEnum.roadblock);
            MGNotificationCenter.defaultCenter().addObserver(this, useSkillsBeatback, SkillActEventEnum.beatback);

        }
        //后面的角色动作
        else if (this.gameObject.name == "role1")
        {
            //print ("yes role1");
            //注册动作事件
            //rolePlayer = GameObject.Find("roleLater");
            if (MGGlobalDataCenter.defaultCenter().isSingle && MGGlobalDataCenter.defaultCenter().isFrontRoler)
            {
                this.gameObject.AddComponent<MGRoleActAIController>();
                this.gameObject.AddComponent<MGRoleLaterSkillAIController>();
            }
            roleSpeed = 1f / 20f;
            MGGlobalDataCenter.defaultCenter().roleLater = this.gameObject;
            MGNotificationCenter.defaultCenter().addObserver(this, jump, RoleActEventEnum.jumpLatterEventId);
            MGNotificationCenter.defaultCenter().addObserver(this, downToLine, RoleActEventEnum.dowmToLineLatterEventId);
            //注册技能事件
            MGNotificationCenter.defaultCenter().addObserver(this, useSkillsBlink, SkillActEventEnum.blink);
            MGNotificationCenter.defaultCenter().addObserver(this, useSkillsBones, SkillActEventEnum.bones);
            MGNotificationCenter.defaultCenter().addObserver(this, useSkillsSprint, SkillActEventEnum.sprint);
            //@test
            //useSkillsBones(new MGNotification("123", null, null));
        }
    }
	void Start () {
        roleAnimaController = this.GetComponent<RoleAnimController>();
        mgNetWorking = GameObject.Find("NetWork").GetComponent<MGNetWorking>();
	}
    public void initRoleJumpScript()
    {
        stateMask = 0;
        isCollisionOver = false;
        jumpCount = 0;
        isGround = false;
    }
    public string objcToJson(string msg)
    {
        //log.label.text+="jump send:" + MGGlobalDataCenter.timestamp ()+"\r\n";
        MGMsgModel msgModel = new MGMsgModel();
        if (MGGlobalDataCenter.defaultCenter().isFrontRoler == true)
            msgModel.eventId = msg;
        else msgModel.eventId = "1"+msg;
        return JsonMapper.ToJson(msgModel);
    }
    public MGSkillsBase skillsBeatback()
    {
        Vector3 pos = new Vector3(11f, 0, 0);
        MGSkillBeatback skillObjc = null;
        if (Network.peerType != NetworkPeerType.Disconnected)
        {
            skillObjc = mgNetWorking.Instantiate(beatback, pos, new Quaternion(0,0,0,0), 0) as MGSkillBeatback;
        }
        else
        {
            skillObjc = beatback.createSkillSprite(pos) as MGSkillBeatback;
        }
        if (skillObjc)
        {
            skillObjc.releaseSkillObjectName = this.gameObject.name;
        }
        return skillObjc;
    }
    public void useSkillsBeatback(MGNotification notification)
    {
        if (notification.objc == null)
        {
            skillsBeatback();
        }
    }
    public MGSkillsBase skillsSprint()
    {
        Vector3 pos = new Vector3(transform.position.x + 3 * renderer.bounds.size.x / 16, transform.position.y + (transform.localScale.y > 0 ? 1 : -1) * renderer.bounds.size.y / 2, transform.position.z);
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
        return skillObjc;
    }
    public void useSkillsSprint(MGNotification notification)
    {
        if (notification.objc == null)
        {
            skillsSprint();
        }
    }
    public MGSkillsBase skillsBlink()
    {
        int isDown = transform.localScale.y > 0 ? 0 : 1;
        Vector3 pos = new Vector3(transform.position.x, MGGlobalDataCenter.defaultCenter().roadOrignY - (isDown == 1 ? 0.175f : 0f), transform.position.z);
        MGSkillBlink skillObjc = null;
        if (Network.peerType != NetworkPeerType.Disconnected)
        {
            skillObjc = mgNetWorking.Instantiate(blink, pos, Quaternion.Euler(0, isDown * 180, isDown * 180), 0) as MGSkillBlink;
        }
        else
        {
            skillObjc = blink.createSkillSprite(pos, Quaternion.Euler(0, isDown * 180, isDown * 180)) as MGSkillBlink;
        }
        if (skillObjc)
        {
            skillObjc.releaseSkillObjectName = this.gameObject.name;
        }
        return skillObjc;
    }
    public void useSkillsBlink(MGNotification notification)
    {
        if (notification.objc == null)
        {
            skillsBlink();
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
    public MGSkillsBase skillsDart()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + (transform.localScale.y > 0 ? 1 : -1) * renderer.bounds.size.y / 2, transform.position.z);
        MGskillDrat skillObjc = bones as MGskillDrat;
        if (Network.peerType != NetworkPeerType.Disconnected)
        {
            skillObjc = mgNetWorking.Instantiate(drat, pos, new Quaternion(), 0) as MGskillDrat;
        }
        else
        {
            skillObjc = drat.createSkillSprite(pos) as MGskillDrat;
        }
        if (skillObjc)
        {
            skillObjc.releaseSkillObjectName = this.gameObject.name;
        }
        return skillObjc;
    }
    public void useSkillsDart(MGNotification notification)
    {
		if (notification.objc == null) {
            skillsDart();
		}
    }
    public MGSkillsBase skillsRoadblock()
    {
        int isDown = transform.localScale.y > 0 ? 0 : 1;
        Vector3 pos = new Vector3(transform.position.x, MGGlobalDataCenter.defaultCenter().roadOrignY - (isDown == 1 ? 0.175f : 0f), transform.position.z);
        MGSkillRoadblock skillObjc = roadblock as MGSkillRoadblock;
        if (Network.peerType != NetworkPeerType.Disconnected)
        {
            skillObjc = mgNetWorking.Instantiate(roadblock, pos, Quaternion.Euler(0, isDown * 180, isDown * 180), 0) as MGSkillRoadblock;
        }
        else
        {
            skillObjc = roadblock.createSkillSprite(pos, Quaternion.Euler(0, isDown * 180, isDown * 180)) as MGSkillRoadblock;
        }
        if (skillObjc)
        {
            skillObjc.releaseSkillObjectName = this.gameObject.name;
        }
        return skillObjc;
    }
    public void useSkillsRoadblock(MGNotification notification)
    {
        if (notification.objc == null)
        {
            skillsRoadblock();
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
                mgNetWorking.sendMessageToPeer(objcToJson(RoleActEventEnum.jumpFormerEventId));
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
                mgNetWorking.sendMessageToPeer(objcToJson(RoleActEventEnum.jumpFormerEventId));
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
                mgNetWorking.sendMessageToPeer(objcToJson(RoleActEventEnum.jumpFormerEventId));
            }
		}
	}

    public void downToLine(MGNotification notification)
    {
        //角色会根据下按钮，翻转到线下
        MGNotificationCenter.defaultCenter().postNotification(buttonEventId(RoleButtonEvent.downFormerEventId), null);
		if(notification.objc==null){
            mgNetWorking.sendMessageToPeer(objcToJson(RoleActEventEnum.downToLineFormerEventId));
		}
    }
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (MGGlobalDataCenter.defaultCenter().isFrontRoler == true)
                MGNotificationCenter.defaultCenter().postNotification("downToLine", null);
            else
                MGNotificationCenter.defaultCenter().postNotification("1downToLine", null);
        }
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			if (MGGlobalDataCenter.defaultCenter().isFrontRoler == true)
				MGNotificationCenter.defaultCenter().postNotification("jump", null);
			else
				MGNotificationCenter.defaultCenter().postNotification("1jump", null);
		}
        if (roleSpeed != 0)
        {
            float dis = MGGlobalDataCenter.defaultCenter().roleFrontPos.x - MGGlobalDataCenter.defaultCenter().roleLaterPos.x;
            transform.Translate(Vector3.right * dis * roleSpeed * Time.deltaTime);
        }
        if (!MGGlobalDataCenter.defaultCenter().isGameOver)
        {
            gameOver();
        }
        if (MGGlobalDataCenter.defaultCenter().isGameOver)
        {
            if (this.gameObject.name == "role")
            {
                if (collider2D.isTrigger && transform.position.x < MGGlobalDataCenter.defaultCenter().roleLater.transform.position.x - 2f )
                {
                    if (transform.position.y >= MGGlobalDataCenter.defaultCenter().roadOrignY)
                    {
                        transform.localScale = new Vector3(1, 1, 1);
                        transform.rotation = Quaternion.Euler(0, 0, 0);
                        rigidbody2D.gravityScale = 5;
                        collider2D.isTrigger = false;
                    }
                }
            }
        }
	}
    public void gameOver()
    {
        Vector3 roleLaterPos = Vector3.zero;
        Vector3 roleFrontPos = Vector3.zero;
        try
        {
            roleLaterPos = MGGlobalDataCenter.defaultCenter().roleLater.transform.position;
            roleFrontPos = MGGlobalDataCenter.defaultCenter().role.transform.position;
        }
        catch 
        {
            return;
        }
        
        if (roleFrontPos.x - roleLaterPos.x < 1.0f || isCollisionOver)//后者追上前者结束
        {
            MGGlobalDataCenter.defaultCenter().roleLater.GetComponent<Jump>().stateMask |= roleState.wudi;
			MGGlobalDataCenter.defaultCenter ().isKillMingyue = true;
            //MGGlobalDataCenter.defaultCenter().isGameOver = true;
            MGGlobalDataCenter.defaultCenter().roleLater.rigidbody2D.velocity = Vector3.zero;
            MGGlobalDataCenter.defaultCenter().role.rigidbody2D.velocity = Vector3.zero;
            //强制roleLater出现在role的后面一点点。

            roleLaterPos.x = roleFrontPos.x - 0.95f;
            roleLaterPos.y = roleFrontPos.y = MGGlobalDataCenter.defaultCenter().roadOrignY;
            MGGlobalDataCenter.defaultCenter().roleLater.transform.position = roleLaterPos;
            MGGlobalDataCenter.defaultCenter().roleLater.transform.localScale = new Vector3(1, 1, 1);
            MGGlobalDataCenter.defaultCenter().roleLater.transform.rotation = Quaternion.Euler(0, 0, 0);
            MGGlobalDataCenter.defaultCenter().role.transform.position = roleFrontPos;
            MGGlobalDataCenter.defaultCenter().role.transform.localScale = new Vector3(1, 1, 1);
            MGGlobalDataCenter.defaultCenter().role.transform.rotation = Quaternion.Euler(0, 0, 0);

            if (this.gameObject.name == "role1")
                MGNotificationCenter.defaultCenter().postNotification(RoleButtonEvent.killLatterEventId, this.gameObject.name);
        }
        if (this.gameObject.name == "role1" && transform.position.x + 1.4f < MGGlobalDataCenter.defaultCenter().screenLiftX)//后者出屏幕结束，还有一个后者死掉结束在RoleAnimController里
        {
            MGGlobalDataCenter.defaultCenter().isGameOver = true;
            //切换场景
            Debug.Log("role1 out of screen");
            MGGlobalDataCenter.defaultCenter().overSenceUIName = "victoryFrontGameUI";
            Application.LoadLevel("overSence");
            MGMsgModel gameoverModel = new MGMsgModel();
            gameoverModel.eventId = RoleActEventEnum.gameoverEventId;
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
            /*
            GameObject sprint= GameObject.Find("sprint(Clone)");
            if (sprint != null)
            {
                Destroy(sprint);
            }*/
            if (collision.gameObject.name == "role")
            {
				Debug.Log("set role trigger");
                if (roleAnimaController.downOrUp)
                    rigidbody2D.gravityScale = 0.5f;
                else
                    rigidbody2D.gravityScale = 0f;
                rigidbody2D.velocity = Vector3.left;
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
