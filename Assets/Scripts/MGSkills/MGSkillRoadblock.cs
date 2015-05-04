using UnityEngine;
using System.Collections;
using LitJson;
public class MGSkillRoadblock : MGSkillsBase{
    public int speed;
    public bool isBreak;
    void Start()
    {
        releaseSkillObjectName = "role";
        isBreak = false;
        mgNetWorking = GameObject.Find("NetWork").GetComponent<MGNetWorking>();
        gameObject.name += MGGlobalDataCenter.defaultCenter().dartIndex.ToString();
        MGGlobalDataCenter.defaultCenter().dartIndex = (MGGlobalDataCenter.defaultCenter().dartIndex + 1) % (MGSkillDartInfo.skillHoldLevel * MGSkillRoadblockInfo.skillHoldLevel);
        MGNotificationCenter.defaultCenter().addObserver(this, triggerFunc, SkillEnum.roadblock + gameObject.name);
    }
    // Use this for initialization
    public override Object createSkillSprite(Vector3 pos)
    {
        base.createSkillSprite(pos);
        return GameObject.Instantiate(this, pos, Quaternion.Euler(0, 0, 0));
    }
    public override Object createSkillSprite(Vector3 pos, Quaternion rotation)
    {
        base.createSkillSprite(pos, rotation);
        return GameObject.Instantiate(this, pos, rotation);
    }
    public override void playSkillAnimation()
    {
        base.playSkillAnimation();
        transform.Translate(-Vector3.right * speed * Time.deltaTime);
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
            DestroySelf();
        }
    }
    void triggerFunc(MGNotification notification)
    {
        string tag = null, name = null;
        if (notification.objc is Collider2D)//自己要做的
        {
            tag = (notification.objc as Collider2D).tag;
            name = (notification.objc as Collider2D).name;
        }
        else if (notification.objc is MGMsgModel)//对面要做的
        {
            tag = (notification.objc as MGMsgModel).tag;
            name = (notification.objc as MGMsgModel).name;
        }
        //Debug.Log(tag + ";" + name);
        if (isBreak) return;
        if (name == "bones(Clone)" || name == "sprint(Clone)")
        {
            //Debug.Log("roadblock break");
            isBreak = true;
            this.GetComponent<Animator>().SetBool("isBreak", true);
        }
        if (tag != "Player")
            return;
        if (name != releaseSkillObjectName)
        {
			string strrole = "role";
			int irole = strrole.Length;
			if( name.Length>=irole && name.Substring(0, irole) == strrole )
				MGGlobalDataCenter.defaultCenter().isRoadBlockHit = true;
			else
				MGGlobalDataCenter.defaultCenter().isRoadBlockDefence = true;
            //print("技能名：路障。被打中的是" + name + "，释放技能的是" + releaseSkillObjectName);
            isBreak = true;
            this.GetComponent<Animator>().SetBool("isBreak", true);
            MGNotificationCenter.defaultCenter().postNotification(RoleButtonEvent.killAnimLatterEventId, name);

			GameObject objc=GameObject.Find(name);
			if(objc){
				int boneMask=objc.GetComponent<Jump>().stateMask & roleState.bone;
				int sprintMask=objc.GetComponent<Jump>().stateMask & roleState.sprint;
				if(boneMask!=0 || sprintMask!=0) return;
			}

            if (notification.objc is Collider2D)
            {
                MGMsgModel skillModel = new MGMsgModel();
                skillModel.eventId = SkillEffectEnum.roadblock;
                skillModel.gameobjectName = name;
                //发送给自己
                MGNotificationCenter.defaultCenter().postNotification(SkillEffectEnum.roadblock, skillModel);
            }

        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (MGGlobalDataCenter.defaultCenter().isHost == true)
        {
            MGMsgModel model = new MGMsgModel();
            model.eventId = SkillEnum.roadblock + gameObject.name;
            model.tag = other.tag;
            model.name = other.name;
            MGNotificationCenter.defaultCenter().postNotification(SkillEnum.roadblock + gameObject.name, other);
            mgNetWorking.sendMessageToPeer(JsonMapper.ToJson(model));
        }
        
    }
    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
