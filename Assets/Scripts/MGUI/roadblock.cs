using UnityEngine;
using System.Collections;

public class roadblock : UIBase
{
    private float roadblockGCDTimer;
    private float holdLevel;
    // Use this for initialization
    void Start()
    {
        holdLevel = MGSkillRoadblockInfo.skillHoldLevel;
        roadblockGCDTimer = 0;
        direction = true;
        addOrDec = true;
        cdBackObject = GameObject.Find("roadblockBack");
        cdBack = cdBackObject.GetComponent<UISprite>();
        cdBack.fillAmount = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCD)
        {

            if (holdLevel > 0)
            {
                roadblockGCDTimer += Time.deltaTime;
                if (roadblockGCDTimer > MGSkillRoadblockInfo.skillGCD)
                {
                    roadblockGCDTimer = 0;
                    --holdLevel;
                    MGNotificationCenter.defaultCenter().postNotification(SkillActEventEnum.roadblock, null);
                }
            }

            cdBack.fillAmount += (addOrDec ? 1 : -1) * (1f / MGSkillRoadblockInfo.skillCD) * Time.deltaTime;
            if (addOrDec)
            {
                if (cdBack.fillAmount >= 1f)
                {
                    isCD = false;
                    holdLevel = MGSkillRoadblockInfo.skillHoldLevel;
                    cdBack.fillAmount = 1f;
                }
            }
            else
            {
                if (cdBack.fillAmount <= 0f)
                {
                    isCD = false;
                    holdLevel = MGSkillRoadblockInfo.skillHoldLevel;
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
            roadblockGCDTimer = 0;
            holdLevel=MGSkillRoadblockInfo.skillHoldLevel-1;
            MGNotificationCenter.defaultCenter().postNotification(SkillActEventEnum.roadblock, null);
        }
    }
}
