using UnityEngine;
using System.Collections;

public static class SkillEffectEnum
{
    public static string dart = "SkillEffectEnum_dart";
    public static string roadblock = "SkillEffectEnum_roadblock";
    public static string bones = "SkillEffectEnum_bones";
    public static string blink = "SkillEffectEnum_blink";
    public static string sprint = "SkillEffectEnum_sprint";
    public static string beatback = "SkillEffectEnum_beatback";
}
//主要用于技能对别的gameobject产生效果处理,MGSkill+技能名 是技能自己的效果处理
public class MGSkillEffect : MonoBehaviour {
    private float timer;
    private MGNotification dartSwitch,blinkSwitch,sprintSwitch;
	// Use this for initialization
	void Start () {
        timer = 0;
        dartSwitch = null;
        blinkSwitch = null;
        sprintSwitch = null;
        MGNotificationCenter.defaultCenter().addObserver(this, dartEffect, SkillEffectEnum.dart);
        MGNotificationCenter.defaultCenter().addObserver(this, blinkEffect, SkillEffectEnum.blink);
        MGNotificationCenter.defaultCenter().addObserver(this, sprintEffect, SkillEffectEnum.sprint);
	}
	
	// Update is called once per frame
	void Update () {
        //dartEffect(dartSwitch);
        //blinkEffect(blinkSwitch);只要一下就闪到前面 所以不需要再Update里慢慢移动
        sprintEffect(sprintSwitch);
	}
    void sprintEffect(MGNotification notification)
    {
        if (notification != null)
        {

            sprintSwitch = notification;
            MGMsgModel skillModel = (MGMsgModel)notification.objc;
            GameObject objc = GameObject.Find(skillModel.gameobjectName);
            if (objc)
            {
                float dis = MGGlobalDataCenter.defaultCenter().roleFrontPos.x - MGGlobalDataCenter.defaultCenter().roleLaterPos.x;
                objc.transform.Translate(Vector3.right * MGSkillSprintInfo.SkillEffectSpeed * dis * Time.smoothDeltaTime / MGSkillSprintInfo.durationTime);
            }
            timer += Time.deltaTime;
            if (timer > MGSkillSprintInfo.durationTime)
            {
                //修改背景的移动速度
                GameObject[] backgroundList = GameObject.FindGameObjectsWithTag("Background");
                for (int i = 0; i < backgroundList.Length; ++i)
                {
                    backgroundList[i].GetComponent<bgnear>().speed -= 20;
                }
                backgroundList = GameObject.FindGameObjectsWithTag("Road");
                for (int i = 0; i < backgroundList.Length; ++i)
                {
                    backgroundList[i].GetComponent<bgnear>().speed -= 20;
                }

                objc.GetComponent<RoleAnimController>().isSprint = false;
                objc.GetComponent<Animator>().SetBool("sprint", false);
                objc.GetComponent<SpriteRenderer>().material = new Material(Shader.Find("Sprites/Default"));
                UILabel label = GameObject.Find("log").GetComponent<UIInput>().label;
                Vector3 pos1 = GameObject.Find("role1").transform.position;
                Vector3 pos = GameObject.Find("role").transform.position;
                label.text += "\r\nrole.x=" + pos.x + ";role1.x=" + pos1.x;
                timer = 0;
                sprintSwitch = null;

            }
        }
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
                objc.transform.Translate(Vector3.right * MGSkillBlinkInfo.SkillEffectSpeed * dis);
            }
            timer += Time.deltaTime;
            UILabel label = GameObject.Find("log").GetComponent<UIInput>().label;
            Vector3 pos1 = GameObject.Find("role1").transform.position;
            Vector3 pos = GameObject.Find("role").transform.position;
            label.text += "\r\nrole.x=" + pos.x + ";role1.x=" + pos1.x;
            if (timer > MGSkillBlinkInfo.durationTime)
            {
				
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
            float dis = MGGlobalDataCenter.defaultCenter().roleFrontPos.x - MGGlobalDataCenter.defaultCenter().roleLaterPos.x;
            if (objc)
            {
                objc.transform.Translate(Vector3.left * MGSkillDartInfo.SkillEffectSpeed * dis );
            }
            timer += Time.deltaTime;
            if (timer >= MGSkillDartInfo.durationTime)
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
