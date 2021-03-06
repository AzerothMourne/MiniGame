﻿using UnityEngine;
using System.Collections;
using LitJson;

public class MGskillDrat : MGSkillsBase{
	public int speed;
    public Vector3 direction;
	private long timestamp;
    public bool isPlayDart;
	public Sprite danSprite;
	// Use this for initialization
	void Start()
	{
        gameObject.name += MGGlobalDataCenter.defaultCenter().dartIndex.ToString();
        MGGlobalDataCenter.defaultCenter().dartIndex=(MGGlobalDataCenter.defaultCenter().dartIndex+1)%(MGSkillDartInfo.skillHoldLevel*MGSkillRoadblockInfo.skillHoldLevel);
        direction = Vector3.left;
		timestamp = MGGlobalDataCenter.timestamp ();
        mgNetWorking = GameObject.Find("NetWork").GetComponent<MGNetWorking>();
		//获取播放器对象
		isPlayDart = false;
        releaseSkillObjectName = "role";
		MGGlobalDataCenter.defaultCenter ().isDartRelease = true;
        MGNotificationCenter.defaultCenter().addObserver(this, triggerFunc, SkillEnum.dart + gameObject.name);
	}
    public override Object createSkillSprite(Vector3 pos)
	{
		base.createSkillSprite(pos);
		return GameObject.Instantiate(this, pos, Quaternion.Euler(0, 0, 0));
	}
	public override void playSkillAnimation()
	{
		base.playSkillAnimation();
        transform.Translate(direction * speed * Time.deltaTime);
	}
	public override void playSkillSound()
	{
		base.playSkillSound();
	}
	// Update is called once per frame
	void Update()
	{
		playSkillAnimation();
		if (transform.position.x < MGGlobalDataCenter.defaultCenter().screenLiftX)
		{
			Destroy(this.gameObject);
		}
	}
	void danDart(){
		MGGlobalDataCenter.defaultCenter().isDartHit = true;
		MGGlobalDataCenter.defaultCenter().roleLater.GetComponent<SpriteRenderer> ().sprite = danSprite;
		
		this.GetComponent<Collider2D>().enabled = false;
		GameObject releaseRole = MGGlobalDataCenter.defaultCenter().role;
		
		float otherObjectY = this.transform.position.y;
		float releaseObjectY = releaseRole.transform.position.y + releaseRole.GetComponent<SpriteRenderer>().bounds.size.y / 2;
		int angle = Random.Range(0, 25) + 135;//随机生成135到160度的角度
		Vector3 direction = this.GetComponent<MGskillDrat>().direction;
		if (otherObjectY >= releaseObjectY)
		{
			//向上飞
			this.GetComponent<MGskillDrat>().direction = new Vector3(direction.x, Mathf.Abs(direction.x) * Mathf.Tan(Mathf.PI * (angle - 90) / 180f), direction.z);
		}
		else
		{
			//向下飞
			this.GetComponent<MGskillDrat>().direction = new Vector3(direction.x, -1 * Mathf.Abs(direction.x) * Mathf.Tan(Mathf.PI * (angle - 90) / 180f), direction.z);
		}
	}
    void triggerFunc(MGNotification notification)
    {
        Debug.Log("triggerFunc");

        if (notification.objc is Collider2D)//自己要做的
        {
            Collider2D other = notification.objc as Collider2D;
            danDart();
            GameObject objc = GameObject.Find(other.name);
            if (objc)
            {
                int boneMask = objc.GetComponent<Jump>().stateMask & roleState.bone;
                int sprintMask = objc.GetComponent<Jump>().stateMask & roleState.sprint;
                if (boneMask != 0 || sprintMask != 0) return;
            }

            MGMsgModel skillModel = new MGMsgModel();
            skillModel.eventId = SkillEffectEnum.dart;
            skillModel.gameobjectName = other.name;
            //发送给自己
            MGNotificationCenter.defaultCenter().postNotification(SkillEffectEnum.dart, skillModel);
        }
        else if(notification.objc is MGMsgModel)//对面要做的
        {
            //MGMsgModel other = notification.objc as MGMsgModel;
			danDart();
        }
        
    }
	void OnTriggerEnter2D(Collider2D other)
	{
        if (other.tag != "Player" || other.name == releaseSkillObjectName)
            return;
        if (MGGlobalDataCenter.defaultCenter().isFrontRoler == true || MGGlobalDataCenter.defaultCenter().isSingle)
        {
            Debug.Log("OnTriggerEnter2D dart");
            MGMsgModel model = new MGMsgModel();
            model.eventId = SkillEnum.dart + gameObject.name;
            model.tag = other.tag;
            model.name = other.name;
            MGNotificationCenter.defaultCenter().postNotification(SkillEnum.dart + gameObject.name, other);
            mgNetWorking.sendMessageToPeer(JsonMapper.ToJson(model));
        }
	}
    void OnDestroy()
    {
        MGNotificationCenter.defaultCenter().removeObserver(this);
    }
}
