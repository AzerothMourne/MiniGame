using UnityEngine;
using System.Collections;

public class Upward : UIBase
{
	private float timer;
    private float cameraMoveSpeed;
	private bool isClick,isMoveCamera;
    private Vector3 originVec3;
    private float duration;

	// Use this for initialization
	void Start () {
        cameraMoveSpeed = 8f;
		timer = 0.0f;
        duration = 0.10f;
		isClick = false;
        isMoveCamera = false;
        originVec3 = transform.localScale;
	}

	void Update(){
        
		if (isClick) {
            if (timer <= duration )
            {
                transform.localScale = new Vector3((transform.localScale.x - 0.01f * Time.timeScale), (transform.localScale.y - 0.01f * Time.timeScale), (transform.localScale.z - 0.01f * Time.timeScale));
				timer+=Time.deltaTime;
			}
            else if (timer <= 2 * duration )
            {
                transform.localScale = new Vector3((transform.localScale.x + 0.01f * Time.timeScale), (transform.localScale.y + 0.01f * Time.timeScale), (transform.localScale.z + 0.01f * Time.timeScale));
                timer += Time.deltaTime;
			}
            else if (timer > 2 * duration )
            {
				isClick=false;timer=0.0f;
                transform.localScale = originVec3;
			}
		}
        if (isMoveCamera)
        {
            Vector3 pos = Camera.main.transform.position;
            pos.y +=cameraMoveSpeed*Time.deltaTime;
            Camera.main.transform.position = pos;
            if (pos.y >= 0f)
            {
                pos.y = 0f;
                Camera.main.transform.position = pos;
                isMoveCamera = false;
            }
        }
	}
	
	// Update is called once per frame
	public void OnMouseDown () {
        if (MGGlobalDataCenter.defaultCenter().isStop == true) return;
		isClick = true;
		//将向上的按钮变为跳的按钮
        isMoveCamera = true;
		this.GetComponent<UISprite>().spriteName = "jump";
        this.GetComponent<UIButton>().normalSprite = "jump";
        if (MGGlobalDataCenter.defaultCenter().isHost == true)
            MGNotificationCenter.defaultCenter().postNotification("jump", null);
        else
            MGNotificationCenter.defaultCenter().postNotification("1jump", null);
	}
}
	