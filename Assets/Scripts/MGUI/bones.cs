using UnityEngine;
using System.Collections;

public class bones : MonoBehaviour {

    public float cdTime = 2;
    private bool isCD = false;
    private bool holdCD = false;
    private UISprite cdBack;
    public bool direction;// true 顺时针，false逆时针
    public bool addOrDec;// true 添加,false 减少
    private GameObject cdBackObject;
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
                if (cdBack.fillAmount >= 0.95f)
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
                if (cdBack.fillAmount <= 0.05f)
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
        if (!isCD && !holdCD)
        {
            addOrDec = false;
            direction = false;
            cdBack.fillAmount = addOrDec ? 0f : 1f;
            holdCD = true;
            cdBackObject.transform.localScale = new Vector3((addOrDec ? 1 : -1)*(direction ? -1 : 1), 1, 1);
            MGNotificationCenter.defaultCenter().postNotification(EventEnum.bones, null);
        }
    }
}
