using UnityEngine;
using System.Collections;

public class Down : UIBase
{
    private float cameraMoveSpeed,fx;
    private bool isClick, isMoveCamera;
    private float timer;
    private string selfSpriteName;
   // private MusicPlayer music;
	// Use this for initialization
	void Start () {
        cameraMoveSpeed = 8f;
		timer = 0.0f;
		isClick = false;
        selfSpriteName = this.GetComponent<UISprite>().spriteName;
        fx = 1;
    //    music = (GetComponent("MusicPlayer") as MusicPlayer);//获取播放器对象
	}
	
	// Update is called once per frame
	void Update () {
        if (isMoveCamera)
        {
            Vector3 pos = Camera.main.transform.position;
            pos.y -= cameraMoveSpeed * Time.deltaTime * fx;
            Camera.main.transform.position = pos;
            if ((pos.y <= -1 && fx == 1) || (pos.y >= 0 && fx == -1))
            {
                pos.y = fx > 0 ? -1 : 0;
                Camera.main.transform.position = pos;
                isMoveCamera = false;
            }
        }
		if (isClick) {
            if (timer <= 0.10f )
            {
                transform.localScale = new Vector3((transform.localScale.x - 0.03f * Time.timeScale), (transform.localScale.y - 0.03f * Time.timeScale), (transform.localScale.z - 0.03f * Time.timeScale));
				timer+=Time.deltaTime;
			}
            else if (timer <= 0.2f )
            {
                transform.localScale = new Vector3((transform.localScale.x + 0.03f * Time.timeScale), (transform.localScale.y + 0.03f * Time.timeScale), (transform.localScale.z + 0.03f * Time.timeScale));
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
        
        if (this.GetComponent<UISprite>().spriteName == selfSpriteName)
        {
            fx = 1;
            this.GetComponent<UISprite>().spriteName = "up";
            this.GetComponent<UIButton>().normalSprite = "up";
            if (MGGlobalDataCenter.defaultCenter().isFrontRoler)
                MGNotificationCenter.defaultCenter().postNotification(RoleActEventEnum.downToLineFormerEventId, null);
            if (MGGlobalDataCenter.defaultCenter().isLaterRoler)
                MGNotificationCenter.defaultCenter().postNotification(RoleActEventEnum.dowmToLineLatterEventId, null);
        }
        else if (this.GetComponent<UISprite>().spriteName == "up")
        {
            fx = -1;
            this.GetComponent<UISprite>().spriteName = selfSpriteName;
            this.GetComponent<UIButton>().normalSprite = selfSpriteName;
            if (MGGlobalDataCenter.defaultCenter().isFrontRoler)
                MGNotificationCenter.defaultCenter().postNotification(RoleActEventEnum.jumpFormerEventId, null);
            if (MGGlobalDataCenter.defaultCenter().isLaterRoler)
                MGNotificationCenter.defaultCenter().postNotification(RoleActEventEnum.jumpLatterEventId, null);
        }
        isMoveCamera = true;
	}
}
