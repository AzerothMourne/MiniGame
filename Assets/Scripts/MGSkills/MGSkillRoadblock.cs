using UnityEngine;
using System.Collections;
using LitJson;
public class MGSkillRoadblock : MGSkillsBase{
    public string releaseSkillObjcName;
    public int speed;

    void Awake()
    {
        releaseSkillObjcName = null;
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

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "bones(Clone)" || other.name == "sprint(Clone)")
        {
            Debug.Log("roadblock break");
            this.GetComponent<Animator>().SetBool("isBreak",true);
        }
        if (other.tag != "Player")
            return;
        if (other.name != releaseSkillObjcName && releaseSkillObjcName != null)
        {
            MGMsgModel skillModel = new MGMsgModel();
            skillModel.eventId = SkillEffectEnum.roadblock;
            skillModel.gameobjectName = other.name;
            //发送给自己
            MGNotificationCenter.defaultCenter().postNotification(SkillEffectEnum.roadblock, skillModel);
            print("技能名：路障。被打中的是" + other.name + "，释放技能的是" + releaseSkillObjcName);
            MGNotificationCenter.defaultCenter().postNotification(RoleButtonEvent.deadLatterEventId, other.name);
        }
    }
    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (releaseSkillObjcName == null)
        {
            releaseSkillObjcName = other.name;
        }
    }
}
