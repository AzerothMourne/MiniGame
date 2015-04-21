using UnityEngine;
using System.Collections;

public class Down : MonoBehaviour {
    private float cameraMoveSpeed;
    private bool isClick, isMoveCamera;
	// Use this for initialization
	void Start () {
        cameraMoveSpeed = 8f;
	}
	
	// Update is called once per frame
	void Update () {
        if (isMoveCamera)
        {
            Vector3 pos = Camera.main.transform.position;
            pos.y -= cameraMoveSpeed * Time.deltaTime;
            Camera.main.transform.position = pos;
            if (pos.y <= -1f)
            {
                pos.y = -1f;
                Camera.main.transform.position = pos;
                isMoveCamera = false;
            }
        }
	}

	public void OnMouseDown () {
        //print("Down OnMouseDown time : " + MGGlobalDataCenter.timestamp());
        if(MGGlobalDataCenter.defaultCenter().isHost==true)
            MGNotificationCenter.defaultCenter().postNotification("downToLine", null);
        else
            MGNotificationCenter.defaultCenter().postNotification("1downToLine", null);
		//按向下后调出向上按钮
        GameObject upButton=GameObject.Find("upButton(Clone)");
        upButton.GetComponent<UISprite>().spriteName= "up";
        upButton.GetComponent<UIButton>().normalSprite = "up";
        isMoveCamera = true;
	}
}
