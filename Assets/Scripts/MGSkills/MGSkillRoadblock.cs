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
        MGNotificationCenter.defaultCenter().addObserver(this, triggerFunc, SkillEnum.roadblock);
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
        Debug.Log(tag + ";" + name);
        if (isBreak) return;
        if (name == "bones(Clone)" || name == "sprint(Clone)")
        {
            Debug.Log("roadblock break");
            isBreak = true;
            this.GetComponent<Animator>().SetBool("isBreak", true);
        }
        if (tag != "Player")
            return;
        if (name != releaseSkillObjectName)
        {
			print("技能名：路障。被打中的是" + name + "，释放技能的是" + releaseSkillObjectName);
			string strrole = "role";
			int irole = strrole.Length;
			if( name.Length>=irole && name.Substring(0, irole) == strrole )
				MGGlobalDataCenter.defaultCenter().isRoadBlockHit = true;
			else
				MGGlobalDataCenter.defaultCenter().isRoadBlockDefence = true;
			print("RoadBlockHit="+MGGlobalDataCenter.defaultCenter().isRoadBlockHit+
			      ",RoadBloackDefence="+MGGlobalDataCenter.defaultCenter().isRoadBlockDefence);
            MGNotificationCenter.defaultCenter().postNotification(RoleButtonEvent.deadLatterEventId, name);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (MGGlobalDataCenter.defaultCenter().isHost == true)
        {
            MGMsgModel model = new MGMsgModel();
            model.eventId = SkillEnum.roadblock;
            model.tag = other.tag;
            model.name = other.name;
            MGNotificationCenter.defaultCenter().postNotification(SkillEnum.roadblock, other);
            mgNetWorking.sendMessageToPeer(JsonMapper.ToJson(model));
        }
        
    }
    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
