using UnityEngine;
using System.Collections;

public static class SkillEffectEnum
{
    public static string dart = "SkillEffectEnum_dart";
    public static string roadblock = "SkillEffectEnum_roadblock";
    public static string bones = "SkillEffectEnum_bones";
    public static string blink = "SkillEffectEnum_blink";
}
//主要用于技能对别的gameobject产生效果处理,MGSkill+技能名 是技能自己的效果处理
public class MGSkillEffect : MonoBehaviour {
    private float timer;
    private MGNotification dartSwitch,blinkSwitch;
	// Use this for initialization
	void Start () {
        timer = 0;
        dartSwitch = null;
        blinkSwitch = null;
        MGNotificationCenter.defaultCenter().addObserver(this, dartEffect, SkillEffectEnum.dart);
        MGNotificationCenter.defaultCenter().addObserver(this, blinkEffect, SkillEffectEnum.blink);
	}
	
	// Update is called once per frame
	void Update () {
        dartEffect(dartSwitch);
        //blinkEffect(blinkSwitch);只要一下就闪到前面 所以不需要再Update里慢慢移动
	}
    void blinkEffect(MGNotification notification)
    {
        if (notification != null)
        {
           
            blinkSwitch = notification;
            MGMsgModel skillModel = (MGMsgModel)notification.objc;
            Debug.Log("skillModel:" + skillModel + ",eventId:" + skillModel.eventId + ",gameobjectName:" + skillModel.gameobjectName);
            GameObject objc = GameObject.Find(skillModel.gameobjectName);
			if (objc)
            {
                Debug.Log("blinkObjc:" + objc + "name:" + objc.name);
				float dis=MGGlobalDataCenter.defaultCenter().roleFrontPos.x-MGGlobalDataCenter.defaultCenter().roleLaterPos.x;
                objc.transform.Translate(Vector3.right * MGSkillBlinkInfo.SkillEffectSpeed*dis);
            }
            timer += Time.deltaTime;
            if (timer > MGSkillBlinkInfo.durationTime)
            {
				UILabel label=GameObject.Find("Control - Simple Text Box").GetComponent<UIInput>().label;
				Vector3 pos1=GameObject.Find("role1").transform.position;
				Vector3 pos=GameObject.Find("role").transform.position;
				label.text+="\r\nrole.x="+pos.x+";role1.x="+pos1.x;
                timer = 0;
                blinkSwitch = null;

            }
        }
    }
    void dartEffect(MGNotification notification)
    {
        if (notification!=null)
        {
            dartSwitch = notification;
            MGMsgModel skillModel = (MGMsgModel)notification.objc;
            GameObject objc = GameObject.Find(skillModel.gameobjectName);
			if(MGGlobalDataCenter.defaultCenter().isHost=false){
				timer = 0;
				blinkSwitch = null;
				return;
			}
            if (objc)
            {
                objc.transform.Translate(-Vector3.right * MGSkillDartInfo.SkillEffectSpeed * Time.deltaTime);
            }
            timer += Time.deltaTime;
            if (timer > MGSkillDartInfo.durationTime)
            {
                timer = 0;
                dartSwitch = null;
            }
        }
    }
    void OnDestroy()
    {
        MGNotificationCenter.defaultCenter().removeObserver(this);
    }
}
