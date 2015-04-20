using UnityEngine;
using System.Collections;
using LitJson;

public class MGskillDrat : MGSkillsBase{
	public string releaseSkillObjcName;
	public int speed;
	void Awake()
	{
		releaseSkillObjcName = null;
	}
	// Use this for initialization
	void Start()
	{

	}
	public override void createSkillSprite(Vector3 pos)
	{
		base.createSkillSprite(pos);
		GameObject.Instantiate(this, pos, Quaternion.Euler(0, 0, -1));
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
		if (other.name == "road" || other.name == "roadSecond")
			return;
		if (other.name != releaseSkillObjcName && releaseSkillObjcName != null)
		{
            MGSkillModel skillModel = new MGSkillModel();
            skillModel.eventId = SkillEnum.dart;
            skillModel.gameobjectName = other.name;
            //发送给对面
            //mgNetWorking.sendMessageToPeer(objcToJson(EventEnum.jumpFormerEventId));
            //发送给自己
            MGNotificationCenter.defaultCenter().postNotification(SkillEnum.dart,skillModel);
			print("技能名：飞镖。被打中的是" + other.name + "，释放技能的是" + releaseSkillObjcName+";gameobjc:"+other.gameObject);
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
