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
        objc.transform.localPosition = new Vector3(1130, 520, 0);
        objc.transform.localScale = new Vector3(1, 1, 1);

        objc = GameObject.Instantiate(roadblockButton, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, -1)) as GameObject;
        objc.transform.parent = NGUIRoot.transform;
        objc.transform.localPosition = new Vector3(350, -600, 0);
        objc.transform.localScale = new Vector3(1, 1, 1);

        objc = GameObject.Instantiate(bonesButton, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, -1)) as GameObject;
        objc.transform.parent = NGUIRoot.transform;
        objc.transform.localPosition = new Vector3(-1000, 400, 0);
        objc.transform.localScale = new Vector3(1, 1, 1);
    }
    public void createLaterRoleUI()
    {

    }
}
