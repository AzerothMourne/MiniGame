using UnityEngine;
using System.Collections;

public class roadblock : UIBase
{
    // Use this for initialization
    void Start()
    {
        direction = true;
        addOrDec = true;
        cdBackObject = GameObject.Find("roadblockBack");
        cdBack = cdBackObject.GetComponent<UISprite>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isCD)
        {
            cdBack.fillAmount += (addOrDec ? 1 : -1) * (1f / MGSkillRoadblockInfo.skillCD) * Time.deltaTime;
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
            cdBack.fillAmount = addOrDec ? 0f : 1f;
            isCD = true;
            cdBackObject.transform.localScale = new Vector3((addOrDec ? 1 : -1) * (direction ? -1 : 1), 1, 1);
            MGNotificationCenter.defaultCenter().postNotification(EventEnum.roadblock, null);
        }
       

    }
}
