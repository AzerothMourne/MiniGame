using UnityEngine;
using System.Collections;
using System;

public class createRoleUI : MonoBehaviour {
    public GameObject dartButton,roadblockButton,beatbackButton;
    public GameObject blinkButton,bonesButton,sprintButton;
    public GameObject downButton, upButton;
	public Camera uiCamera;
    private int UILayerMask = 7;
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
        objc.layer =  UILayerMask;
        objc.transform.position = MGFoundtion.WorldPointToNGUIPoint(new Vector3(MGGlobalDataCenter.defaultCenter().screenLiftX + 1.5f*MGGlobalDataCenter.defaultCenter().NGUI_ButtonWidth, -4f, 0f), uiCamera);
		objc.transform.localScale = new Vector3(MGGlobalDataCenter.defaultCenter().UIScale, MGGlobalDataCenter.defaultCenter().UIScale, 1);

        objc = GameObject.Instantiate(upButton, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
        objc.transform.parent = transform;
        objc.layer =  UILayerMask;
        objc.transform.position = MGFoundtion.WorldPointToNGUIPoint(new Vector3(MGGlobalDataCenter.defaultCenter().screenRightX - 1.5f*MGGlobalDataCenter.defaultCenter().NGUI_ButtonWidth, -4f, 0f), uiCamera);
		objc.transform.localScale = new Vector3(MGGlobalDataCenter.defaultCenter().UIScale, MGGlobalDataCenter.defaultCenter().UIScale, 1);

    }
	public void createFrontRoleUI()
    {
        print("createFrontRoleUI");
        //飞镖按钮UI
        GameObject objc= GameObject.Instantiate(dartButton, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
        objc.transform.parent = transform;
        objc.layer =  UILayerMask;
		objc.transform.position = MGFoundtion.WorldPointToNGUIPoint(new Vector3(MGGlobalDataCenter.defaultCenter().screenRightX-MGGlobalDataCenter.defaultCenter().NGUI_ButtonWidth / 2, 2.29f, 0f),uiCamera);
		objc.transform.localScale = new Vector3(MGGlobalDataCenter.defaultCenter().UIScale, MGGlobalDataCenter.defaultCenter().UIScale, 1);
        //路障按钮UI
        objc = GameObject.Instantiate(roadblockButton, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
        objc.transform.parent = transform;
        objc.layer =  UILayerMask;
        objc.transform.position = MGFoundtion.WorldPointToNGUIPoint(new Vector3(MGGlobalDataCenter.defaultCenter().screenRightX - 3.5f * MGGlobalDataCenter.defaultCenter().NGUI_ButtonWidth, -4f, 0f), uiCamera);
		objc.transform.localScale = new Vector3(MGGlobalDataCenter.defaultCenter().UIScale, MGGlobalDataCenter.defaultCenter().UIScale, 1);
        //击退按钮
        objc = GameObject.Instantiate(beatbackButton, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
        objc.transform.parent = transform;
        objc.layer = UILayerMask;
        objc.transform.position = MGFoundtion.WorldPointToNGUIPoint(new Vector3(MGGlobalDataCenter.defaultCenter().screenLiftX + MGGlobalDataCenter.defaultCenter().NGUI_ButtonWidth / 2, 2.29f, 0f), uiCamera);
		objc.transform.localScale = new Vector3(MGGlobalDataCenter.defaultCenter().UIScale, MGGlobalDataCenter.defaultCenter().UIScale, 1);

        
    }
    public void createLaterRoleUI()
    {
        print("createLaterRoleUI");
        //闪现按钮UI
        GameObject objc = GameObject.Instantiate(blinkButton, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
        objc.transform.parent = transform;
        objc.layer =  UILayerMask;
        objc.transform.position = MGFoundtion.WorldPointToNGUIPoint(new Vector3(MGGlobalDataCenter.defaultCenter().screenRightX-MGGlobalDataCenter.defaultCenter().NGUI_ButtonWidth / 2, 2.29f, 0f), uiCamera);
		objc.transform.localScale = new Vector3(MGGlobalDataCenter.defaultCenter().UIScale, MGGlobalDataCenter.defaultCenter().UIScale, 1);
        //金钟罩按钮UI
        objc = GameObject.Instantiate(bonesButton, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
        objc.transform.parent = transform;
        objc.layer =  UILayerMask;
        objc.transform.position = MGFoundtion.WorldPointToNGUIPoint(new Vector3(MGGlobalDataCenter.defaultCenter().screenLiftX + MGGlobalDataCenter.defaultCenter().NGUI_ButtonWidth / 2, 2.29f, 0f), uiCamera);
		objc.transform.localScale = new Vector3(MGGlobalDataCenter.defaultCenter().UIScale, MGGlobalDataCenter.defaultCenter().UIScale, 1);
        
		objc = GameObject.Instantiate(sprintButton, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
        objc.transform.parent = transform;
        objc.layer =  UILayerMask;
        objc.transform.position = MGFoundtion.WorldPointToNGUIPoint(new Vector3(MGGlobalDataCenter.defaultCenter().screenRightX - 3.5f * MGGlobalDataCenter.defaultCenter().NGUI_ButtonWidth , -4f, 0f), uiCamera);
		objc.transform.localScale = new Vector3(MGGlobalDataCenter.defaultCenter().UIScale, MGGlobalDataCenter.defaultCenter().UIScale, 1);
        
    }
}
