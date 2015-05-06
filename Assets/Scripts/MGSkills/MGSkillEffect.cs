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
    private float sprintTimer,beatbackTimer,roadblockTimer, blinkSkillBonesTimer;
    private MGNotification sprintSwitch,beatbackSwitch,roadblockSwitch;
    private GameObject tempObjcet;
    public int speedSwitch;
	// Use this for initialization
	void Start () {
		sprintTimer = 0;
		beatbackTimer = 0;
		blinkSkillBonesTimer = 1;
        sprintSwitch = null;
        tempObjcet = null;
        roadblockSwitch = null;
        shortBonesName = "bones_short";
        MGNotificationCenter.defaultCenter().addObserver(this, dartEffect, SkillEffectEnum.dart);
        MGNotificationCenter.defaultCenter().addObserver(this, blinkEffect, SkillEffectEnum.blink);
        MGNotificationCenter.defaultCenter().addObserver(this, sprintEffect, SkillEffectEnum.sprint);
        MGNotificationCenter.defaultCenter().addObserver(this, beatbackEffect, SkillEffectEnum.beatback);
        MGNotificationCenter.defaultCenter().addObserver(this, roadblockEffect, SkillEffectEnum.roadblock);

	}
	
	// Update is called once per frame
	void Update () {
        //dartEffect(dartSwitch);
        //blinkEffect(blinkSwitch);只要一下就闪到前面 所以不需要再Update里慢慢移动
        sprintEffect(sprintSwitch);
        beatbackEffect(beatbackSwitch);
        roadblockEffect(roadblockSwitch);
        if (blinkSkillBonesTimer < 0.2f)
        {
            blinkSkillBonesTimer += Time.deltaTime;
            if (blinkSkillBonesTimer >= 0.2f)
            {
                UILabel label = GameObject.Find("log").GetComponent<UIInput>().label;
                label.text += "\r\nDestroy:" + shortBonesName;
				GameObject.Find("role1").GetComponent<Jump>().stateMask &= ~roleState.bone;
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
			beatbackTimer += Time.deltaTime;
			if (beatbackTimer > MGSkillBeatbackInfo.durationTime * Time.timeScale)
            {
				beatbackTimer = 0;
				beatbackSwitch = null;
                MGSkillBeatback beatbackObject = GameObject.Find("beatback(Clone)").GetComponent<MGSkillBeatback>();
                beatbackObject.DestroySelf();
            }
        }
    }
    void roadblockEffect(MGNotification notification)
    {
        if (notification != null)
        {
            roadblockSwitch = notification;
            MGMsgModel skillModel = (MGMsgModel)notification.objc;
            GameObject objc = GameObject.Find(skillModel.gameobjectName);
            float dis = MGGlobalDataCenter.defaultCenter().roleFrontPos.x - MGGlobalDataCenter.defaultCenter().roleLaterPos.x;
            if (objc)
            {
                objc.transform.Translate(Vector3.left * MGSkillRoadblockInfo.SkillEffectSpeed * dis * Time.deltaTime / MGSkillRoadblockInfo.durationTime);
            }
            roadblockTimer += Time.deltaTime;
            if (roadblockTimer > MGSkillRoadblockInfo.durationTime * Time.timeScale)
            {
                roadblockTimer = 0;
                roadblockSwitch = null;
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
				if(tempObjcet==null){
					tempObjcet=GameObject.Find("sprint(Clone)");
				}
                float dis = MGGlobalDataCenter.defaultCenter().roleFrontPos.x - MGGlobalDataCenter.defaultCenter().roleLaterPos.x;
				objc.transform.Translate( speedSwitch*Vector3.right * MGSkillSprintInfo.SkillEffectSpeed * dis * Time.deltaTime / MGSkillSprintInfo.durationTime);
				if(tempObjcet){
					int fx=objc.transform.localScale.y>0?1:-1;
					tempObjcet.transform.position=new Vector3(objc.transform.position.x+0.25f,objc.transform.position.y+fx*objc.GetComponent<SpriteRenderer>().bounds.size.y/2f,objc.transform.position.z);
				}
            }
			sprintTimer += Time.deltaTime;
			if (sprintTimer > MGSkillSprintInfo.durationTime)
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
				sprintTimer = 0;
				sprintSwitch = null;
				tempObjcet=null;
                GameObject sprint = GameObject.Find("sprint(Clone)");
                if (sprint != null)
                {
                    Destroy(sprint);
                }
				objc.GetComponent<Jump>().stateMask &= ~roleState.sprint;
            }
        }
    }
    void blinkEffect(MGNotification notification)
    {
        if (notification != null) {
			blinkSkillBonesTimer = 0;
			if(MGGlobalDataCenter.defaultCenter().isFrontRoler == false &&  MGGlobalDataCenter.defaultCenter().isSingle == false){
                MGNotificationCenter.defaultCenter().postNotification(SkillActEventEnum.bones, shortBonesName);//发送bones技能的事件
				return;
			}

			MGMsgModel skillModel = (MGMsgModel)notification.objc;
			Debug.Log ("skillModel:" + skillModel + ",eventId:" + skillModel.eventId + ",gameobjectName:" + skillModel.gameobjectName);
			GameObject objc = GameObject.Find (skillModel.gameobjectName);
			if (objc) {
				Debug.Log ("blinkObjc:" + objc + "name:" + objc.name);
				float dis = MGGlobalDataCenter.defaultCenter ().roleFrontPos.x - MGGlobalDataCenter.defaultCenter ().roleLaterPos.x;
				Vector3 roleLaterPos = MGGlobalDataCenter.defaultCenter ().roleLater.transform.position;
				Vector3 roleFrontPos = MGGlobalDataCenter.defaultCenter ().role.transform.position;
				if (roleFrontPos.x - roleLaterPos.x < MGSkillBlinkInfo.SkillEffectSpeed * dis) {
					objc.transform.Translate (Vector3.right * (roleFrontPos.x - roleLaterPos.x - 1f));
				} else {
					objc.transform.Translate (Vector3.right * MGSkillBlinkInfo.SkillEffectSpeed * dis);
				}
			}

            MGNotificationCenter.defaultCenter().postNotification(SkillActEventEnum.bones, shortBonesName);//发送bones技能的事件

			UILabel label = GameObject.Find ("log").GetComponent<UIInput> ().label;
			Vector3 pos1 = GameObject.Find ("role1").transform.position;
			Vector3 pos = GameObject.Find ("role").transform.position;
			label.text += "\r\nrole.x=" + pos.x + ";role1.x=" + pos1.x + ";shortBonesName:" + shortBonesName;

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
        }
    }
    void OnDestroy()
    {
        MGNotificationCenter.defaultCenter().removeObserver(this);
    }
}
