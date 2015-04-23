using UnityEngine;
using System.Collections;

public class Down : MonoBehaviour {
    private float cameraMoveSpeed;
    private bool isClick, isMoveCamera;
    private float timer;
	// Use this for initialization
	void Start () {
        cameraMoveSpeed = 8f;
		timer = 0.0f;
		isClick = false;
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
		if (isClick) {
            if (timer <= 0.10f * Time.timeScale)
            {
				transform.localScale = new Vector3((transform.localScale.x-0.01f),(transform.localScale.y-0.01f),(transform.localScale.z-0.01f));
				timer+=Time.deltaTime;
			}
            else if (timer <= 0.2f * Time.timeScale)
            {
				transform.localScale=new Vector3((transform.localScale.x+0.01f),(transform.localScale.y+0.01f),(transform.localScale.z+0.01f));
				timer+=Time.deltaTime;
			}
            else if (timer > 0.2f * Time.timeScale)
            {
				isClick=false;timer=0.0f;
				transform.localScale=new Vector3(1,1,1);
			}
		}
	}

	public void OnMouseDown () {
		isClick = true;

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
