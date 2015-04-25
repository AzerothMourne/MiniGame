using UnityEngine;
using System.Collections;
using System;
using LitJson;
public static class uiEvent
{
    public static string stopGame = "uiEvent_stopGame";
    public static string continueGame = "uiEvent_continueGame";
}
public class createRoleUI : MonoBehaviour {
    public GameObject dartButton,roadblockButton,beatbackButton;
    public GameObject blinkButton,bonesButton,sprintButton;
    public GameObject downButton, upButton, stopButton;
    public GameObject stopLayer, homeButton, continueButton;
	public Camera uiCamera;
    public GameObject NGUIRoot;
    private GameObject stopLayerObj, homeButtonObj, continueButtonObj;
    private int UILayerMask = 7;
    private MGNetWorking mgNetWorking;
    void Start()
    {
        mgNetWorking = GameObject.Find("NetWork").GetComponent<MGNetWorking>();
        MGNotificationCenter.defaultCenter().addObserver(this, stopNotification, uiEvent.stopGame);
        MGNotificationCenter.defaultCenter().addObserver(this, continueNotification, uiEvent.continueGame);
        createCommonUI();
        if (MGGlobalDataCenter.defaultCenter().isHost == true)
        {
            createFrontRoleUI();
        }
        else
        {
            createLaterRoleUI();
        }
    }
    public void continueNotification(MGNotification notification)
    {
        if (notification.objc != null)
        {
            continueGame(null);
        }
    }
    public void stopNotification(MGNotification notification)
    {
        if (notification.objc != null)
        {
            clickStop(null);
        }
    }
    public void clickStop(GameObject button)
    {
        if (MGGlobalDataCenter.defaultCenter().isStop == false)
        {

            if (button)
            {
                MGMsgModel uiMsg = new MGMsgModel();
                uiMsg.eventId = uiEvent.stopGame;
                mgNetWorking.sendMessageToPeer(JsonMapper.ToJson(uiMsg));
            }

            MGGlobalDataCenter.defaultCenter().isStop = true;
            Time.timeScale = 0;
            stopLayerObj = GameObject.Instantiate(stopLayer, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
            stopLayerObj.transform.parent = NGUIRoot.transform;
            stopLayerObj.layer = UILayerMask;
            stopLayerObj.transform.position = MGFoundtion.WorldPointToNGUIPoint(Vector3.zero, uiCamera);
			stopLayerObj.transform.localScale = new Vector3(MGGlobalDataCenter.defaultCenter().UIScale, MGGlobalDataCenter.defaultCenter().UIScale, 1);

            continueButtonObj = GameObject.Instantiate(continueButton, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
            continueButtonObj.transform.parent = NGUIRoot.transform;
            continueButtonObj.GetComponent<UISprite>().depth = 3;
            continueButtonObj.layer = UILayerMask;
            continueButtonObj.transform.position = MGFoundtion.WorldPointToNGUIPoint(Vector3.zero, uiCamera);
			continueButtonObj.transform.localScale = new Vector3(MGGlobalDataCenter.defaultCenter().UIScale, MGGlobalDataCenter.defaultCenter().UIScale, 1);
            UIEventListener.Get(continueButtonObj).onClick = continueGame;

            homeButtonObj = GameObject.Instantiate(homeButton, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
            homeButtonObj.transform.parent = NGUIRoot.transform;
            homeButtonObj.GetComponent<UISprite>().depth = 3;
            homeButtonObj.layer = UILayerMask;
            homeButtonObj.transform.position = MGFoundtion.WorldPointToNGUIPoint(new Vector3(MGGlobalDataCenter.defaultCenter().screenLiftX + MGGlobalDataCenter.defaultCenter().NGUI_ButtonWidth / 2+0.3f, MGGlobalDataCenter.defaultCenter().screenBottomY + MGGlobalDataCenter.defaultCenter().NGUI_ButtonWidth / 2+0.3f, 0f), uiCamera);
			homeButtonObj.transform.localScale = new Vector3(MGGlobalDataCenter.defaultCenter().UIScale, MGGlobalDataCenter.defaultCenter().UIScale, 1);
            UIEventListener.Get(homeButtonObj).onClick = homeClick;
        }
    }
    public void continueGame(GameObject button)
    {
        if (MGGlobalDataCenter.defaultCenter().isStop == true)
        {

            if (button)
            {
                MGMsgModel uiMsg = new MGMsgModel();
                uiMsg.eventId = uiEvent.continueGame;
                mgNetWorking.sendMessageToPeer(JsonMapper.ToJson(uiMsg));
            }

            MGGlobalDataCenter.defaultCenter().isStop = false;
            Time.timeScale = 1;
            DestroyObject(stopLayerObj);
            DestroyObject(homeButtonObj);
            DestroyObject(continueButtonObj);
        }
    }
    public void homeClick(GameObject button)
    {
        UILabel label = GameObject.Find("log").GetComponent<UIInput>().label;
        label.text += "\r\nhomeClick";
        MGNetWorking.disconnect();
        label.text += "\r\ndisconnect";
        Application.LoadLevel("startGameScene");
        label.text += "\r\nApplication.LoadLevel(\"startGameScene\")";
    }
    public void createCommonUI()
    {
        print("createCommonUI");
        GameObject objc = GameObject.Instantiate(downButton, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
        objc.transform.parent = NGUIRoot.transform;
        objc.layer =  UILayerMask;
        objc.transform.position = MGFoundtion.WorldPointToNGUIPoint(new Vector3(MGGlobalDataCenter.defaultCenter().screenLiftX + 1.5f*MGGlobalDataCenter.defaultCenter().NGUI_ButtonWidth, -4f, 0f), uiCamera);
		objc.transform.localScale = new Vector3(MGGlobalDataCenter.defaultCenter().UIScale, MGGlobalDataCenter.defaultCenter().UIScale, 1);

        objc = GameObject.Instantiate(upButton, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
        objc.transform.parent = NGUIRoot.transform;
        objc.layer =  UILayerMask;
        objc.transform.position = MGFoundtion.WorldPointToNGUIPoint(new Vector3(MGGlobalDataCenter.defaultCenter().screenRightX - 1.5f*MGGlobalDataCenter.defaultCenter().NGUI_ButtonWidth, -4f, 0f), uiCamera);
		objc.transform.localScale = new Vector3(MGGlobalDataCenter.defaultCenter().UIScale, MGGlobalDataCenter.defaultCenter().UIScale, 1);

        objc = GameObject.Instantiate(stopButton, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
        objc.transform.parent = NGUIRoot.transform;
        objc.layer = UILayerMask;
        objc.transform.position = MGFoundtion.WorldPointToNGUIPoint(new Vector3(MGGlobalDataCenter.defaultCenter().screenRightX - MGGlobalDataCenter.defaultCenter().NGUI_ButtonWidth / 2, MGGlobalDataCenter.defaultCenter().screenTopY - MGGlobalDataCenter.defaultCenter().NGUI_ButtonWidth / 2, 0f), uiCamera);
        objc.transform.localScale = new Vector3(MGGlobalDataCenter.defaultCenter().UIScale, MGGlobalDataCenter.defaultCenter().UIScale, 1);
        UIEventListener.Get(objc).onClick = clickStop;

    }
	public void createFrontRoleUI()
    {
        print("createFrontRoleUI");
        //飞镖按钮UI
        GameObject objc= GameObject.Instantiate(dartButton, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
        objc.transform.parent = NGUIRoot.transform;
        objc.layer =  UILayerMask;
		objc.transform.position = MGFoundtion.WorldPointToNGUIPoint(new Vector3(MGGlobalDataCenter.defaultCenter().screenRightX-MGGlobalDataCenter.defaultCenter().NGUI_ButtonWidth / 2, 2.29f, 0f),uiCamera);
		objc.transform.localScale = new Vector3(MGGlobalDataCenter.defaultCenter().UIScale, MGGlobalDataCenter.defaultCenter().UIScale, 1);
        //路障按钮UI
        objc = GameObject.Instantiate(roadblockButton, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
        objc.transform.parent = NGUIRoot.transform;
        objc.layer =  UILayerMask;
        objc.transform.position = MGFoundtion.WorldPointToNGUIPoint(new Vector3(MGGlobalDataCenter.defaultCenter().screenRightX - 3.5f * MGGlobalDataCenter.defaultCenter().NGUI_ButtonWidth, -4f, 0f), uiCamera);
		objc.transform.localScale = new Vector3(MGGlobalDataCenter.defaultCenter().UIScale, MGGlobalDataCenter.defaultCenter().UIScale, 1);
        //击退按钮
        objc = GameObject.Instantiate(beatbackButton, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
        objc.transform.parent = NGUIRoot.transform;
        objc.layer = UILayerMask;
        objc.transform.position = MGFoundtion.WorldPointToNGUIPoint(new Vector3(MGGlobalDataCenter.defaultCenter().screenLiftX + MGGlobalDataCenter.defaultCenter().NGUI_ButtonWidth / 2, 2.29f, 0f), uiCamera);
		objc.transform.localScale = new Vector3(MGGlobalDataCenter.defaultCenter().UIScale, MGGlobalDataCenter.defaultCenter().UIScale, 1);

        
    }
    public void createLaterRoleUI()
    {
        print("createLaterRoleUI");
        //闪现按钮UI
        GameObject objc = GameObject.Instantiate(blinkButton, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
        objc.transform.parent = NGUIRoot.transform;
        objc.layer =  UILayerMask;
        objc.transform.position = MGFoundtion.WorldPointToNGUIPoint(new Vector3(MGGlobalDataCenter.defaultCenter().screenRightX-MGGlobalDataCenter.defaultCenter().NGUI_ButtonWidth / 2, 2.29f, 0f), uiCamera);
		objc.transform.localScale = new Vector3(MGGlobalDataCenter.defaultCenter().UIScale, MGGlobalDataCenter.defaultCenter().UIScale, 1);
        //金钟罩按钮UI
        objc = GameObject.Instantiate(bonesButton, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
        objc.transform.parent = NGUIRoot.transform;
        objc.layer =  UILayerMask;
        objc.transform.position = MGFoundtion.WorldPointToNGUIPoint(new Vector3(MGGlobalDataCenter.defaultCenter().screenLiftX + MGGlobalDataCenter.defaultCenter().NGUI_ButtonWidth / 2, 2.29f, 0f), uiCamera);
		objc.transform.localScale = new Vector3(MGGlobalDataCenter.defaultCenter().UIScale, MGGlobalDataCenter.defaultCenter().UIScale, 1);
        
		objc = GameObject.Instantiate(sprintButton, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
        objc.transform.parent = NGUIRoot.transform;
        objc.layer =  UILayerMask;
        objc.transform.position = MGFoundtion.WorldPointToNGUIPoint(new Vector3(MGGlobalDataCenter.defaultCenter().screenRightX - 3.5f * MGGlobalDataCenter.defaultCenter().NGUI_ButtonWidth , -4f, 0f), uiCamera);
		objc.transform.localScale = new Vector3(MGGlobalDataCenter.defaultCenter().UIScale, MGGlobalDataCenter.defaultCenter().UIScale, 1);
        
    }
}
