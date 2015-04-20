using UnityEngine;
using System.Collections;

public class createRoleUI : MonoBehaviour {
    public GameObject dartButton,roadblockButton,bonesButton;
    public GameObject NGUIRoot;
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

        }
    }
	public void createFrontRoleUI()
    {
        print("createFrontRoleUI");
        GameObject objc= GameObject.Instantiate(dartButton, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, -1)) as GameObject;
        objc.transform.parent = NGUIRoot.transform;
        objc.transform.localPosition = new Vector3(1121, 510, 0);
        objc.transform.localScale = new Vector3(1, 1, 1);

        objc = GameObject.Instantiate(roadblockButton, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, -1)) as GameObject;
        objc.transform.parent = NGUIRoot.transform;
        objc.transform.localPosition = new Vector3(1000, -950, 0);
        objc.transform.localScale = new Vector3(1, 1, 1);

        objc = GameObject.Instantiate(bonesButton, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, -1)) as GameObject;
        objc.transform.parent = NGUIRoot.transform;
        objc.transform.localPosition = new Vector3(-2000, 500, 0);
        objc.transform.localScale = new Vector3(1, 1, 1);
    }
    public void createLaterRoleUI()
    {

    }
}
