using UnityEngine;
using System.Collections;

public class bones : UIBase
{

    void Awake()
    {
        cdBackObject = GameObject.Find("bonesBack");
        cdBack = cdBackObject.GetComponent<UISprite>();
    }
    // Use this for initialization
    void Start()
    {
        direction = true;
        addOrDec = true;
        cdBackObject.transform.localScale = new Vector3((addOrDec ? 1 : -1) * (direction ? -1 : 1), 1, 1);
        cdBack.fillAmount = 0f;
        isCD = true;
		//@test
		//MGNotificationCenter.defaultCenter().postNotification(EventEnum.bones, null);
    }
    // Update is called once per frame
    void Update()
    {
        if (isCD || holdCD)
        {
            float time = MGSkillBonesInfo.skillCD;
            if (holdCD) time = MGSkillBonesInfo.durationTime;
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
    }
    public void OnMouseDown()
    {
        if (MGGlobalDataCenter.defaultCenter().isStop == true) return;
        if (!isCD && !holdCD && !MGGlobalDataCenter.defaultCenter().isBigSkilling)
        {
            addOrDec = false;
            direction = false;
            cdBack.fillAmount = addOrDec ? 0f : 1f;
            holdCD = true;
            cdBackObject.transform.localScale = new Vector3((addOrDec ? 1 : -1)*(direction ? -1 : 1), 1, 1);
            MGNotificationCenter.defaultCenter().postNotification(SkillActEventEnum.bones, null);
        }
    }
}
