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
            cdBack.fillAmount = addOrDec ? 0f : 1f;
            isCD = true;
            cdBackObject.transform.localScale = new Vector3(direction ? -1 : 1, 1, 1);
        }
        print("OnMouseDown");
    }
}
