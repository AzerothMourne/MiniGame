﻿using UnityEngine;
using System.Collections;

public class createRoleUI : MonoBehaviour {
    public GameObject dartButton,roadblockButton,bonesButton;
    public GameObject blinkButton;
	public Camera uiCamera;
    void Awake()
    {
        MGGlobalDataCenter.defaultCenter();
    }
    void Start()
    {
        if (MGGlobalDataCenter.defaultCenter().isHost == true)
        {
            createFrontRoleUI();
        }
        else
        {
            createLaterRoleUI();
        }
    }
	public void createFrontRoleUI()
    {
        print("createFrontRoleUI");
        GameObject objc= GameObject.Instantiate(dartButton, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
        objc.transform.parent = transform;
		objc.transform.position = MGFoundtion.WorldPointToNGUIPoint(new Vector3(6.7f, 2.29f, 0f),uiCamera);
        objc.transform.localScale = new Vector3(2, 2, 1);

        objc = GameObject.Instantiate(roadblockButton, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
        objc.transform.parent = transform;
		objc.transform.position = MGFoundtion.WorldPointToNGUIPoint(new Vector3(3.6f, -3.5f, 0f),uiCamera);
        objc.transform.localScale = new Vector3(2, 2, 1);

        objc = GameObject.Instantiate(bonesButton, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
        objc.transform.parent = transform;
		objc.transform.position = MGFoundtion.WorldPointToNGUIPoint(new Vector3(-6.7f, 2.29f, 0f),uiCamera);
        objc.transform.localScale = new Vector3(2, 2, 1);
    }
    public void createLaterRoleUI()
    {
        print("createLaterRoleUI");
        GameObject objc = GameObject.Instantiate(blinkButton, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
        objc.transform.parent = transform;
		objc.transform.position = MGFoundtion.WorldPointToNGUIPoint(new Vector3(6.7f, 2.29f, 0f),uiCamera);
        objc.transform.localScale = new Vector3(2, 2, 1);

		objc = GameObject.Instantiate(blinkButton, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
		objc.transform.parent = transform;
		objc.transform.position = MGFoundtion.WorldPointToNGUIPoint(new Vector3(-6.7f, 2.29f, 0f),uiCamera);
		objc.transform.localScale = new Vector3(2, 2, 1);

		objc = GameObject.Instantiate(blinkButton, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
		objc.transform.parent = transform;
		objc.transform.position = MGFoundtion.WorldPointToNGUIPoint(new Vector3(3.6f, -3.5f, 0f),uiCamera);
		objc.transform.localScale = new Vector3(2, 2, 1);

        
    }
}
