using UnityEngine;
using System.Collections;

public class Down : UIBase
{
    private float cameraMoveSpeed;
    private bool isClick, isMoveCamera;
    private float timer;

   // private MusicPlayer music;
	// Use this for initialization
	void Start () {
        cameraMoveSpeed = 8f;
		timer = 0.0f;
		isClick = false;

    //    music = (GetComponent("MusicPlayer") as MusicPlayer);//获取播放器对象
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
            if (timer <= 0.10f )
            {
                transform.localScale = new Vector3((transform.localScale.x - 0.01f * Time.timeScale), (transform.localScale.y - 0.01f * Time.timeScale), (transform.localScale.z - 0.01f * Time.timeScale));
				timer+=Time.deltaTime;
			}
            else if (timer <= 0.2f )
            {
                transform.localScale = new Vector3((transform.localScale.x + 0.01f * Time.timeScale), (transform.localScale.y + 0.01f * Time.timeScale), (transform.localScale.z + 0.01f * Time.timeScale));
				timer+=Time.deltaTime;
			}
            else if (timer > 0.2f )
            {
				isClick=false;timer=0.0f;
				transform.localScale=new Vector3(MGGlobalDataCenter.defaultCenter().UIScale,MGGlobalDataCenter.defaultCenter().UIScale,1);
			}
		}
	}

	public void OnMouseDown () {
        if (MGGlobalDataCenter.defaultCenter().isStop == true) return;
		isClick = true;
        if(MGGlobalDataCenter.defaultCenter().isHost==true)
            MGNotificationCenter.defaultCenter().postNotification("downToLine", null);
        else
            MGNotificationCenter.defaultCenter().postNotification("1downToLine", null);
        //按向下后调出向上按钮
        GameObject upButton = GameObject.Find("upButton(Clone)");
        //播放音效
        //if(upButton.GetComponent<UISprite>().spriteName != "up")
        //    music.play("Sound/updown_roll");
     
        upButton.GetComponent<UISprite>().spriteName= "up";
        upButton.GetComponent<UIButton>().normalSprite = "up";
        isMoveCamera = true;
	}
}
