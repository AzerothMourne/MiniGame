using UnityEngine;
using System.Collections;


public class MGSkillEffect : MonoBehaviour {
    private float timer;
    private MGNotification dartSwitch,blinkSwitch;
	// Use this for initialization
	void Start () {
        timer = 0;
        dartSwitch = null;
        blinkSwitch = null;
        MGNotificationCenter.defaultCenter().addObserver(this, dartEffect,SkillEnum.dart);
        MGNotificationCenter.defaultCenter().addObserver(this, blinkEffect, SkillEnum.blink);
	}
	
	// Update is called once per frame
	void Update () {
        dartEffect(dartSwitch);
	}
    void blinkEffect(MGNotification notification)
    {
        if (notification != null)
        {
            blinkSwitch = notification;
            MGSkillModel skillModel = (MGSkillModel)notification.objc;
            GameObject objc = GameObject.Find(skillModel.gameobjectName);
            print("触发闪现效果");
            objc.transform.Translate(Vector3.right * MGSkillBlinkInfo.SkillEffectSpeed * Time.deltaTime);
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
            MGSkillModel skillModel = (MGSkillModel)notification.objc;
            GameObject objc = GameObject.Find(skillModel.gameobjectName);
            objc.transform.Translate(-Vector3.right * MGSkillDartInfo.SkillEffectSpeed * Time.deltaTime);
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
