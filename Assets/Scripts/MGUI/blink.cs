using UnityEngine;
using System.Collections;

public class blink : UIBase
{
    void Awake()
    {
        cdBackObject = GameObject.Find("blinkBack");
        cdBack = cdBackObject.GetComponent<UISprite>();
    }
    // Use this for initialization
    void Start()
    {
        direction = true;
        addOrDec = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCD)
        {
            cdBack.fillAmount += (addOrDec ? 1 : -1) * (1f / MGSkillBlinkInfo.skillCD) * Time.deltaTime;
            if (addOrDec)
            {
                if (cdBack.fillAmount >= 1f)
                {
                    isCD = false;
                    cdBack.fillAmount = 1f;
                }
            }
            else
            {
                if (cdBack.fillAmount <= 0f)
                {
                    isCD = false;
                    cdBack.fillAmount = 0f;
                }
            }

        }
    }
    public void OnMouseDown()
    {
        if (MGGlobalDataCenter.defaultCenter().isStop == true) return;
        if (!isCD && !MGGlobalDataCenter.defaultCenter().isBigSkilling)
        {
            UILabel label = GameObject.Find("log").GetComponent<UIInput>().label;
            label.text += "\r\n blink OnMouseDown";
            cdBack.fillAmount = addOrDec ? 0f : 1f;
            isCD = true;
            cdBackObject.transform.localScale = new Vector3((addOrDec ? 1 : -1) * (direction ? -1 : 1), 1, 1);
            MGNotificationCenter.defaultCenter().postNotification(EventEnum.blink, null);
        }
    }
}
