using UnityEngine;
using System.Collections;
using LitJson;

public class MGSkillBlink : MGSkillsBase
{
    private MGNetWorking mgNetWorking;
    public string releaseSkillObjcName;
    public int speed;
    private bool isBlinked;
    void Awake()
    {
        releaseSkillObjcName = null;
    }
    // Use this for initialization
    void Start()
    {
        mgNetWorking = GameObject.Find("NetWork").GetComponent<MGNetWorking>();
        isBlinked = false;
    }
    public override Object createSkillSprite(Vector3 pos)
    {
        base.createSkillSprite(pos);
        return GameObject.Instantiate(this, pos, Quaternion.Euler(0, 0, 0));
    }
    public override void playSkillAnimation()
    {
        base.playSkillAnimation();
        if (isBlinked == false)//通知技能效果类执行相应的效果
        {
            isBlinked = true;
            MGMsgModel skillModel = new MGMsgModel();
            skillModel.eventId = SkillEffectEnum.blink;
            skillModel.gameobjectName = releaseSkillObjcName;
            //发送给对面 产生技能效果
            mgNetWorking.sendMessageToPeer(JsonMapper.ToJson(skillModel));
            //发送给自己
            MGNotificationCenter.defaultCenter().postNotification(SkillEffectEnum.blink, skillModel);
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
