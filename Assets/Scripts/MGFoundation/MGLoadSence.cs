using UnityEngine;
using System.Collections;
using System;
public static class LoadSenceEnum
{
    public static string LoadLevel = "LoadLevel";
}
public class MGLoadSence : MonoBehaviour {
    private static MGLoadSence instance;
    private MGLoadSence()
    {
        MGNotificationCenter.defaultCenter().addObserver(this, loadLevelWay,LoadSenceEnum.LoadLevel);
	}
    public static MGLoadSence shareInstance()
    {
        if (instance == null)
        {
            instance = new MGLoadSence();
        }
        return instance;
    }
    void loadLevelWay(MGNotification notification)
    {
        if(notification.objc!=null && notification.objc is string)
            Application.LoadLevel(notification.objc as string);
    }
}
