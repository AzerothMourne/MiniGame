using UnityEngine;
using System.Collections;
using System;

public class createRoleUI : MonoBehaviour {
    public GameObject dartButton,roadblockButton;
    public GameObject blinkButton,bonesButton;
    public GameObject downButton, upButton;
	public Camera uiCamera;
    void Awake()
    {
        MGGlobalDataCenter singleInstance = MGGlobalDataCenter.defaultCenter();
        Debug.Log("uiCamera.GetSides()=" + uiCamera.GetSides()[0] + uiCamera.GetSides()[1] + uiCamera.GetSides()[2] + uiCamera.GetSides()[3]);
        singleInstance.screenBottomY = uiCamera.GetSides()[3].y;
        singleInstance.screenTopY = -1 * singleInstance.screenBottomY;
        singleInstance.screenLiftX = uiCamera.GetSides()[0].x;
        singleInstance.screenRightX = -1 * singleInstance.screenLiftX;

        Vector3 pos = MGFoundtion.pixelToWroldPoint(88f, 88f);
        singleInstance.NGUI_ButtonWidth = Math.Abs( pos.x-singleInstance.screenLiftX );
        Debug.Log(pos + "********" + singleInstance.NGUI_ButtonWidth);
    }
    void Start()
    {
        
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
    public void createCommonUI()
    {
        print("createCommonUI");
        GameObject objc = GameObject.Instantiate(downButton, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
        objc.transform.parent = transform;
        objc.transform.position = MGFoundtion.WorldPointToNGUIPoint(new Vector3(MGGlobalDataCenter.defaultCenter().screenLiftX + 2*MGGlobalDataCenter.defaultCenter().NGUI_ButtonWidth, -4f, 0f), uiCamera);
        objc.transform.localScale = new Vector3(1, 1, 1);

        objc = GameObject.Instantiate(upButton, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
        objc.transform.parent = transform;
        objc.transform.position = MGFoundtion.WorldPointToNGUIPoint(new Vector3(MGGlobalDataCenter.defaultCenter().screenRightX - 2*MGGlobalDataCenter.defaultCenter().NGUI_ButtonWidth, -4f, 0f), uiCamera);
        objc.transform.localScale = new Vector3(1, 1, 1);

    }
	public void createFrontRoleUI()
    {
        print("createFrontRoleUI");
        //飞镖按钮UI
        GameObject objc= GameObject.Instantiate(dartButton, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
        objc.transform.parent = transform;
		objc.transform.position = MGFoundtion.WorldPointToNGUIPoint(new Vector3(MGGlobalDataCenter.defaultCenter().screenRightX-MGGlobalDataCenter.defaultCenter().NGUI_ButtonWidth / 2, 2.29f, 0f),uiCamera);
        objc.transform.localScale = new Vector3(1, 1, 1);
        //路障按钮UI
        objc = GameObject.Instantiate(roadblockButton, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
        objc.transform.parent = transform;
		objc.transform.position = MGFoundtion.WorldPointToNGUIPoint(new Vector3(3.6f, -4f, 0f),uiCamera);
        objc.transform.localScale = new Vector3(1, 1, 1);

        
    }
    public void createLaterRoleUI()
    {
        print("createLaterRoleUI");
        //闪现按钮UI
        GameObject objc = GameObject.Instantiate(blinkButton, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
        objc.transform.parent = transform;
        objc.transform.position = MGFoundtion.WorldPointToNGUIPoint(new Vector3(MGGlobalDataCenter.defaultCenter().screenRightX-MGGlobalDataCenter.defaultCenter().NGUI_ButtonWidth / 2, 2.29f, 0f), uiCamera);
        objc.transform.localScale = new Vector3(1, 1, 1);
        //金钟罩按钮UI
        objc = GameObject.Instantiate(bonesButton, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
        objc.transform.parent = transform;
        objc.transform.position = MGFoundtion.WorldPointToNGUIPoint(new Vector3(MGGlobalDataCenter.defaultCenter().screenLiftX + MGGlobalDataCenter.defaultCenter().NGUI_ButtonWidth / 2, 2.29f, 0f), uiCamera);
        objc.transform.localScale = new Vector3(1, 1, 1);
        /*
		objc = GameObject.Instantiate(blinkButton, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
		objc.transform.parent = transform;
		objc.transform.position = MGFoundtion.WorldPointToNGUIPoint(new Vector3(-6.7f, 2.29f, 0f),uiCamera);
		objc.transform.localScale = new Vector3(2, 2, 1);

		objc = GameObject.Instantiate(blinkButton, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
		objc.transform.parent = transform;
		objc.transform.position = MGFoundtion.WorldPointToNGUIPoint(new Vector3(3.6f, -3.5f, 0f),uiCamera);
		objc.transform.localScale = new Vector3(2, 2, 1);
        */
        
    }
}
