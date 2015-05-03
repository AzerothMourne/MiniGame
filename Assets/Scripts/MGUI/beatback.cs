using UnityEngine;
using System.Collections;

public class beatback : UIBase
{
	private float fireTimer;
	private int index;
	public GameObject fireSprite;
    void Awake()
    {
        cdBackObject = GameObject.Find("beatbackBack");
        cdBack = cdBackObject.GetComponent<UISprite>();
    }
    // Use this for initialization
    void Start()
    {
		fireTimer = 0;
		index = 0;
        direction = true;
        addOrDec = true;
        cdBackObject.transform.localScale = new Vector3((addOrDec ? 1 : -1) * (direction ? -1 : 1), 1, 1);
        isCD = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (isCD || holdCD)
        {
            float time = MGSkillBeatbackInfo.skillCD;
            if (holdCD) time = MGSkillBeatbackInfo.durationTime;
            cdBack.fillAmount += (addOrDec ? 1 : -1) * (1f / time) * Time.deltaTime;
            if (addOrDec)
            {
                if (cdBack.fillAmount >= 1f)
                {
                    if (holdCD == true)
                    {
                        addOrDec = !addOrDec;
                        holdCD = false;
                    }
                    isCD = !isCD;
                    cdBack.fillAmount = 1f;
                }
            }
            else
            {
                if (cdBack.fillAmount <= 0f)
                {
                    if (holdCD == true)
                    {
                        addOrDec = !addOrDec;
                        holdCD = false;
                    }
                    isCD = !isCD;
                    cdBack.fillAmount = 0f;
                }
            }

        }
		if (!isCD && !holdCD) {
			Color color=fireSprite.GetComponent<UISprite>().color;
			color.a=1;
			fireSprite.GetComponent<UISprite>().color=color;
			fireTimer+=Time.deltaTime;
			if(fireTimer>0.15f){
				fireTimer=0;
				fireSprite.GetComponent<UISprite>().spriteName=(index+1).ToString();
				index=(index+1)%12;
			}
		}
    }
    public void OnMouseDown()
    {
        if (MGGlobalDataCenter.defaultCenter().isStop == true) return;
        if (!isCD && !holdCD && !MGGlobalDataCenter.defaultCenter().isBigSkilling)
        {
			Color color=fireSprite.GetComponent<UISprite>().color;
			color.a=0;
			fireSprite.GetComponent<UISprite>().color=color;
			index=0;
			fireTimer=0;
            addOrDec = false;
            direction = false;
            cdBack.fillAmount = addOrDec ? 0f : 1f;
            holdCD = true;
            cdBackObject.transform.localScale = new Vector3((addOrDec ? 1 : -1) * (direction ? -1 : 1), 1, 1);
            MGNotificationCenter.defaultCenter().postNotification(EventEnum.beatback, null);
        }
    }
}
