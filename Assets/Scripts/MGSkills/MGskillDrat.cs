using UnityEngine;
using System.Collections;
using LitJson;

public class MGskillDrat : MGSkillsBase{
	public string releaseSkillObjcName;
	public int speed;
    private MGNetWorking mgNetWorking;
	private long timestamp;
	void Awake()
	{
		releaseSkillObjcName = null;
	}
	// Use this for initialization
	void Start()
	{
        mgNetWorking = GameObject.Find("Main Camera").GetComponent<MGNetWorking>();
		timestamp = MGGlobalDataCenter.timestamp ();
	}
    public override Object createSkillSprite(Vector3 pos)
	{
		base.createSkillSprite(pos);
		return GameObject.Instantiate(this, pos, Quaternion.Euler(0, 0, 0));
	}
	public override void playSkillAnimation()
	{
		base.playSkillAnimation();
		if (posY == -10000)
		{
			posY = transform.position.y;
		}
		transform.Translate(-Vector3.right * speed * Time.deltaTime);
		transform.position = new Vector3(transform.position.x,posY,transform.position.z);
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
	
	void OnTriggerEnter2D(Collider2D other)
	{
        if (other.tag != "Player")
			return;
		if (other.name != releaseSkillObjcName && releaseSkillObjcName != null)
		{
            MGMsgModel skillModel = new MGMsgModel();
            skillModel.eventId = SkillEffectEnum.dart;
            skillModel.gameobjectName = other.name;
            //发送给对面,产生技能效果
            //mgNetWorking.sendMessageToPeer(JsonMapper.ToJson(skillModel));
            //发送给自己
            MGNotificationCenter.defaultCenter().postNotification(SkillEffectEnum.dart, skillModel);
			print("技能名：飞镖。被打中的是" + other.name + "，释放技能的是" + releaseSkillObjcName+";gameobjc:"+other.gameObject);
			Debug.Log("***dart fly time:"+(MGGlobalDataCenter.timestamp()-timestamp).ToString());
			UILabel label=GameObject.Find("Control - Simple Text Box").GetComponent<UIInput>().label;
			label.text+="\r\n***dart fly time:"+(MGGlobalDataCenter.timestamp()-timestamp).ToString();
			Destroy(this.gameObject);
		}
	}
	void OnTriggerExit2D(Collider2D other)
	{
		if (releaseSkillObjcName == null)
		{
			releaseSkillObjcName = other.name;
		}
	}
}
