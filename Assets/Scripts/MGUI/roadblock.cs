using UnityEngine;
using System.Collections;

public class roadblock : MonoBehaviour {
    public float cdTime ;
    private bool isCD = false;
    private UISprite cdBack;
    public bool direction;// true 顺时针，false逆时针
    public bool addOrDec;// true 添加,false 减少
    public GameObject cdBackObject;
    // Use this for initialization
    void Start()
    {
        cdTime = MGSkillRoadblockInfo.skillCD;
        direction = true;
        addOrDec = true;
        cdBack = cdBackObject.GetComponent<UISprite>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isCD)
        {
            cdBack.fillAmount += (addOrDec ? 1 : -1) * (1f / cdTime) * Time.deltaTime;
            if (addOrDec)
            {
                if (cdBack.fillAmount >= 0.95f)
                {
                    isCD = false;
                    cdBack.fillAmount = 1f;
                }
            }
            else
            {
                if (cdBack.fillAmount <= 0.05f)
                {
                    isCD = false;
                    cdBack.fillAmount = 0f;
                }
            }

        }
    }
    public void OnMouseDown()
    {
        if (!isCD)
        {
            cdBack.fillAmount = addOrDec ? 0f : 1f;
            isCD = true;
            cdBackObject.transform.localScale = new Vector3((addOrDec ? 1 : -1) * (direction ? -1 : 1), 1, 1);
            MGNotificationCenter.defaultCenter().postNotification(EventEnum.roadblock, null);
        }
       

    }
}
