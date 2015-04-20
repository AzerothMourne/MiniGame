using UnityEngine;
using System.Collections;

public static class SkillEffectEnum
{
    public static string dart = "SkillEffectEnum_dart";
    public static string roadblock = "SkillEffectEnum_roadblock";
    public static string bones = "SkillEffectEnum_bones";
    public static string blink = "SkillEffectEnum_blink";
}
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
                objc.transform.Translate(Vector3.right * MGSkillBlinkInfo.SkillEffectSpeed * Time.deltaTime);
            }
            timer += Time.deltaTime;
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
