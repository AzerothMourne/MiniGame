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
        direction = Vector3.left;
		timestamp = MGGlobalDataCenter.timestamp ();
		
		//获取播放器对象
		isPlayDart = false;
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
	
	void OnTriggerEnter2D(Collider2D other)
	{
        if (other.tag != "Player")
			return;
		if (other.name != "role")
		{
            MGMsgModel skillModel = new MGMsgModel();
            skillModel.eventId = SkillEffectEnum.dart;
            skillModel.gameobjectName = other.name;
            //发送给自己
            MGNotificationCenter.defaultCenter().postNotification(SkillEffectEnum.dart, skillModel);
            print("技能名：飞镖。被打中的是" + other.name + "，释放技能的是" + releaseSkillObjectName + ";gameobjc:" + other.gameObject);
			MGGlobalDataCenter.defaultCenter().isDartHit = true;
			Debug.Log("***dart fly time:"+(MGGlobalDataCenter.timestamp()-timestamp).ToString());
            UILabel label = GameObject.Find("log").GetComponent<UIInput>().label;
            label.text += "\r\n***dart fly time:" + (MGGlobalDataCenter.timestamp() - timestamp).ToString() + ";releaseSkillObjectName" + releaseSkillObjectName;
			Destroy(this.gameObject);
		}
	}
}
