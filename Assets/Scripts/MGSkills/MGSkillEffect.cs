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
    private string shortBonesName;
    public bones blinkSkillBones;
    private float timer, blinkSkillBonesTimer;
    private MGNotification sprintSwitch,beatbackSwitch;
    private GameObject tempObjcet;
    public int speedSwitch;
	// Use this for initialization
	void Start () {
        timer = 0;
        blinkSkillBonesTimer = 1;
        sprintSwitch = null;
        tempObjcet = null;
        shortBonesName = "bones_short";
        MGNotificationCenter.defaultCenter().addObserver(this, dartEffect, SkillEffectEnum.dart);
        MGNotificationCenter.defaultCenter().addObserver(this, blinkEffect, SkillEffectEnum.blink);
        MGNotificationCenter.defaultCenter().addObserver(this, sprintEffect, SkillEffectEnum.sprint);
        MGNotificationCenter.defaultCenter().addObserver(this, beatbackEffect, SkillEffectEnum.beatback);
	}
	
	// Update is called once per frame
	void Update () {
        //dartEffect(dartSwitch);
        //blinkEffect(blinkSwitch);只要一下就闪到前面 所以不需要再Update里慢慢移动
        sprintEffect(sprintSwitch);
        beatbackEffect(beatbackSwitch);
        if (blinkSkillBonesTimer < 0.2f)
        {
            blinkSkillBonesTimer += Time.deltaTime;
            if (blinkSkillBonesTimer >= 0.2f)
            {
                UILabel label = GameObject.Find("log").GetComponent<UIInput>().label;
                label.text += "\r\nDestroy:" + shortBonesName;
                DestroyImmediate(GameObject.Find(shortBonesName),true);
                //Destroy(GameObject.Find(shortBonesName));
            }
        }
	}
    void beatbackEffect(MGNotification notification)
    {
        if (notification != null)
        {

            beatbackSwitch = notification;
            MGMsgModel skillModel = (MGMsgModel)notification.objc;
            GameObject objc = GameObject.Find(skillModel.gameobjectName);
            float dis = MGGlobalDataCenter.defaultCenter().roleFrontPos.x - MGGlobalDataCenter.defaultCenter().roleLaterPos.x;
            if (objc)
            {
                objc.transform.Translate(Vector3.left * MGSkillBeatbackInfo.SkillEffectSpeed * dis * Time.deltaTime / MGSkillBeatbackInfo.durationTime);
            }
            timer += Time.deltaTime;
            if (timer > MGSkillBeatbackInfo.durationTime * Time.timeScale)
            {
                timer = 0;
                beatbackSwitch = null;
                MGSkillBeatback beatbackObject = GameObject.Find("beatback(Clone)").GetComponent<MGSkillBeatback>();
                beatbackObject.DestroySelf();
            }
        }
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
                if (tempObjcet == null)
                {
                    tempObjcet = GameObject.Find("sprint(Clone)");
                }
                Vector3 pos = new Vector3(objc.transform.position.x + 3 * objc.renderer.bounds.size.x / 16,
                    objc.transform.position.y + (objc.transform.localScale.y > 0 ? 1 : -1) * objc.renderer.bounds.size.y / 2,
                    objc.transform.position.z);
                tempObjcet.transform.position = pos;//同步sprint技能的gameobject位置和role1的位置
                //Debug.Log("sprint.position=" + tempObjcet.transform.position);
                float dis = MGGlobalDataCenter.defaultCenter().roleFrontPos.x - MGGlobalDataCenter.defaultCenter().roleLaterPos.x;
                objc.transform.Translate( speedSwitch*Vector3.right * MGSkillSprintInfo.SkillEffectSpeed * dis * Time.smoothDeltaTime / MGSkillSprintInfo.durationTime);
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
                Destroy(tempObjcet);
            }
        }
    }
    void blinkEffect(MGNotification notification)
    {
        if (notification != null)
        {
            blinkSkillBonesTimer = 0;
            MGMsgModel skillModel = (MGMsgModel)notification.objc;
            Debug.Log("skillModel:" + skillModel + ",eventId:" + skillModel.eventId + ",gameobjectName:" + skillModel.gameobjectName);
            GameObject objc = GameObject.Find(skillModel.gameobjectName);
			if (objc)
            {
                Debug.Log("blinkObjc:" + objc + "name:" + objc.name);
				float dis=MGGlobalDataCenter.defaultCenter().roleFrontPos.x-MGGlobalDataCenter.defaultCenter().roleLaterPos.x;
                objc.transform.Translate(Vector3.right * MGSkillBlinkInfo.SkillEffectSpeed * dis);
            }

            MGNotificationCenter.defaultCenter().postNotification(EventEnum.bones, shortBonesName);//发送bones技能的事件

            timer += Time.deltaTime;
            UILabel label = GameObject.Find("log").GetComponent<UIInput>().label;
            Vector3 pos1 = GameObject.Find("role1").transform.position;
            Vector3 pos = GameObject.Find("role").transform.position;
            label.text += "\r\nrole.x=" + pos.x + ";role1.x=" + pos1.x + ";shortBonesName:" + shortBonesName;
            if (timer > MGSkillBlinkInfo.durationTime)
            {
				
                timer = 0;
            }
        }
    }
    void dartEffect(MGNotification notification)
    {
        if (notification!=null)
        {
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
            }
        }
    }
    void OnDestroy()
    {
        MGNotificationCenter.defaultCenter().removeObserver(this);
    }
}
