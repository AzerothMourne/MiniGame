using UnityEngine;
using System.Collections;
using LitJson;

public class MGSkillBlink : MGSkillsBase
{
    public int speed;
    private bool isBlinked;
    // Use this for initialization
    void Start()
    {
        isBlinked = false;
		mgNetWorking = GameObject.Find("NetWork").GetComponent<MGNetWorking>();
		MGGlobalDataCenter.defaultCenter ().isFlash = true;
    }
    public override Object createSkillSprite(Vector3 pos)
    {
        base.createSkillSprite(pos);
        return GameObject.Instantiate(this, pos, Quaternion.Euler(0, 0, 0));
    }
    public override void playSkillAnimation()
    {
        base.playSkillAnimation();
        transform.Translate(Vector3.left * speed * Time.deltaTime);
        if (isBlinked == false)//通知技能效果类执行相应的效果
        {
            isBlinked = true;
            MGMsgModel skillModel = new MGMsgModel();
            skillModel.eventId = SkillEffectEnum.blink;
            skillModel.gameobjectName = "role1";
            //发送给对面 产生技能效果
            if (MGGlobalDataCenter.defaultCenter().isHost == false || MGGlobalDataCenter.defaultCenter().isSingle)
            {
                MGNotificationCenter.defaultCenter().postNotification(SkillEffectEnum.blink, skillModel);
            	mgNetWorking.sendMessageToPeer(JsonMapper.ToJson(skillModel));
			}
        }
    }
    public override void playSkillSound()
    {
        base.playSkillSound();
    }
    // Update is called once per frame
    void Update()
    {
        playSkillAnimation();
    }
    void DestroyGameObject()
    {
        Destroy(this.gameObject);
    }
}
