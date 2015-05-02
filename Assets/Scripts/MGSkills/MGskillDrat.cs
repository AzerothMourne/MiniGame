using UnityEngine;
using System.Collections;
using LitJson;

public class MGskillDrat : MGSkillsBase{
	public int speed;
    public Vector3 direction;
	private long timestamp;
    public bool isPlayDart;
	// Use this for initialization
	void Start()
	{
        gameObject.name += MGGlobalDataCenter.defaultCenter().dartIndex.ToString();
        MGGlobalDataCenter.defaultCenter().dartIndex=(MGGlobalDataCenter.defaultCenter().dartIndex+1)%MGSkillDartInfo.skillHoldLevel;
        direction = Vector3.left;
		timestamp = MGGlobalDataCenter.timestamp ();
        mgNetWorking = GameObject.Find("NetWork").GetComponent<MGNetWorking>();
		//获取播放器对象
		isPlayDart = false;
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
    void triggerFunc(MGNotification notification)
    {
        Debug.Log("triggerFunc");
        if (notification.objc is Collider2D)//自己要做的
        {
            Collider2D other = notification.objc as Collider2D;
            if (other.tag != "Player")
                return;
            if (other.name != "role")
            {
                MGMsgModel skillModel = new MGMsgModel();
                skillModel.eventId = SkillEffectEnum.dart;
                skillModel.gameobjectName = other.name;
                //发送给自己
                MGNotificationCenter.defaultCenter().postNotification(SkillEffectEnum.dart, skillModel);
                //print("技能名：飞镖。被打中的是" + other.name + "，释放技能的是" + releaseSkillObjectName);
                MGGlobalDataCenter.defaultCenter().isDartHit = true;
                //Debug.Log("***dart fly time:" + (MGGlobalDataCenter.timestamp() - timestamp).ToString());
                UILabel label = GameObject.Find("log").GetComponent<UIInput>().label;
                label.text += "\r\n***dart fly time:" + (MGGlobalDataCenter.timestamp() - timestamp).ToString() + ";releaseSkillObjectName" + releaseSkillObjectName;
                Destroy(this.gameObject);
            }
        }
        else if(notification.objc is MGMsgModel)//对面要做的
        {
            MGMsgModel other = notification.objc as MGMsgModel;
            if (other.tag != "Player")
                return;
            if (other.name != "role")
            {
                //print("技能名：飞镖。被打中的是" + other.name + "，释放技能的是" + releaseSkillObjectName);
                MGGlobalDataCenter.defaultCenter().isDartHit = true;
                //Debug.Log("***dart fly time:" + (MGGlobalDataCenter.timestamp() - timestamp).ToString());
                UILabel label = GameObject.Find("log").GetComponent<UIInput>().label;
                label.text += "\r\n***dart fly time:" + (MGGlobalDataCenter.timestamp() - timestamp).ToString() + ";releaseSkillObjectName" + releaseSkillObjectName;
                Destroy(this.gameObject);
            }
        }
        
    }
	void OnTriggerEnter2D(Collider2D other)
	{
        if (MGGlobalDataCenter.defaultCenter().isHost == true)
        {
            Debug.Log("OnTriggerEnter2D dart");
            MGMsgModel model = new MGMsgModel();
            model.eventId = SkillEnum.dart + gameObject.name;
            model.tag = other.tag;
            model.name = other.name;
            MGNotificationCenter.defaultCenter().postNotification(SkillEnum.dart+gameObject.name, other);
            mgNetWorking.sendMessageToPeer(JsonMapper.ToJson(model));
        }
	}
    void OnDestroy()
    {
        MGNotificationCenter.defaultCenter().removeObserver(this);
    }
}
