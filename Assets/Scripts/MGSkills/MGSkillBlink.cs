using UnityEngine;
using System.Collections;

public class MGSkillBlink : MGSkillsBase
{
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
        isBlinked = false;
    }
    public override void createSkillSprite(Vector3 pos)
    {
        base.createSkillSprite(pos);
        GameObject.Instantiate(this, pos, Quaternion.Euler(0, 0, -1));
    }
    public override void playSkillAnimation()
    {
        base.playSkillAnimation();
        if (isBlinked == false)//通知技能效果类执行相应的效果
        {
            isBlinked = true;
            MGSkillModel skillModel = new MGSkillModel();
            skillModel.eventId = SkillEnum.dart;
            skillModel.gameobjectName = releaseSkillObjcName;
            //发送给对面
            //mgNetWorking.sendMessageToPeer(objcToJson(EventEnum.jumpFormerEventId));
            //发送给自己
            MGNotificationCenter.defaultCenter().postNotification(SkillEnum.blink, skillModel);
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
    
}
