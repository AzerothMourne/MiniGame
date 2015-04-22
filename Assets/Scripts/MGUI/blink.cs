using UnityEngine;
using System.Collections;

public class blink : MonoBehaviour {

    public float cdTime = 2;
    private bool isCD = false;
    private UISprite cdBack;
    public bool direction;// true 顺时针，false逆时针
    public bool addOrDec;// true 添加,false 减少
    private GameObject cdBackObject;
    void Awake()
    {
        cdBackObject = GameObject.Find("blinkBack");
        cdBack = cdBackObject.GetComponent<UISprite>();
        UISprite selfSprite = this.gameObject.GetComponent<UISprite>();
        Debug.Log("Dart Width:" + selfSprite.width + ",height:" + selfSprite.height);
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
			UILabel label=GameObject.Find("Control - Simple Text Box").GetComponent<UIInput>().label;
			Vector3 pos1=GameObject.Find("role1").transform.position;
			Vector3 pos=GameObject.Find("role").transform.position;
			label.text+="\r\nrole.x="+pos.x+";role1.x="+pos1.x;
            cdBack.fillAmount = addOrDec ? 0f : 1f;
            isCD = true;
            cdBackObject.transform.localScale = new Vector3((addOrDec ? 1 : -1) * (direction ? -1 : 1), 1, 1);
            MGNotificationCenter.defaultCenter().postNotification(EventEnum.blink, null);
        }
    }
}
