﻿using UnityEngine;
using System.Collections;
public static class SkillEnum
{
    public static string dart = "dart";
    public static string roadblock = "roadblock";
    public static string bones = "bones";
    public static string blink = "blink";
}

public class MGSkillEffect : MonoBehaviour {
    private float timer;
    private MGNotification skillNotification;
	// Use this for initialization
	void Start () {
        timer = 0;
        skillNotification = null;
        MGNotificationCenter.defaultCenter().addObserver(this, dartEffect,SkillEnum.dart);
	}
	
	// Update is called once per frame
	void Update () {
        dartEffect(skillNotification);
	}
    void dartEffect(MGNotification notification)
    {
        if (notification!=null)
        {
            skillNotification = notification;
            MGSkillModel skillModel = (MGSkillModel)notification.objc;
            GameObject objc = GameObject.Find(skillModel.gameobjectName);
            print("触发飞镖效果");
            objc.transform.Translate(-Vector3.right * MGSkillDartInfo.dartSkillEffectSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            if (timer > MGSkillDartInfo.durationTime)
            {
                timer = 0;
                skillNotification = null;
            }
        }
    }
    void OnDestroy()
    {
        MGNotificationCenter.defaultCenter().removeObserver(this);
    }
}
