using UnityEngine;
using System.Collections;

public class Dart : MonoBehaviour {

    private Animator dartAnim; 
	public bool isPressDartButton;
	int countDartFrame;
	private bool isCD = false;//isCD表示内置CD 即GCD
	private UISprite cdBack;
	public bool direction;// true 顺时针，false逆时针
	public bool addOrDec;// true 添加,false 减少
	public GameObject cdBackObject;
    public UILabel dartNum;
    private float gcdTimer;
	// Use this for initialization
	void Start()
	{
        dartAnim = GameObject.Find("role").GetComponent<Animator> ();
        isPressDartButton = false;
		countDartFrame = 0;
        gcdTimer = 0;
		direction = true;
		addOrDec = true;
		cdBack = cdBackObject.GetComponent<UISprite>();
        dartNum.text = MGSkillDartInfo.skillHoldLevel.ToString();
	}
	
	// Update is called once per frame
	void Update()
	{
        

		if (isPressDartButton == true) {
			countDartFrame += 1;
			print ("countFrame : " + countDartFrame);
		}
		if (countDartFrame == 30) {
			isPressDartButton = false;
            //触发放飞镖的动作
		    dartAnim.SetBool ("pressDartButton", isPressDartButton);
			countDartFrame = 0;
		    //	print ("update ********isPressDartButton : " + isPressDartButton);
		}
        if (isCD)
        {
            gcdTimer += Time.deltaTime;
            if (gcdTimer >= MGSkillDartInfo.skillGCD)
            {
                isCD = false;
                gcdTimer = 0;
            }
        }
        if (int.Parse(dartNum.text)<3)
		{
			cdBack.fillAmount += (addOrDec ? 1 : -1) * (1f / MGSkillDartInfo.skillCD) * Time.deltaTime;
			if (addOrDec)
			{
				if (cdBack.fillAmount >= 0.995f)
				{
                    int num = int.Parse(dartNum.text);
                    num++;
                    dartNum.text = num.ToString();
                    if (num == MGSkillDartInfo.skillHoldLevel)
                        cdBack.fillAmount = 1f;
                    else
                        cdBack.fillAmount = 0f;
				}
			}
			else
			{
				if (cdBack.fillAmount <= 0.005f)
				{
                    int num = int.Parse(dartNum.text);
                    num++;
                    dartNum.text = num.ToString();
					cdBack.fillAmount = 0f;
				}
			}
			
		}
	}
    public void OnMouseDown()
    {
        int num = int.Parse(dartNum.text);
        if (!isCD && num > 0)//如果不是GCD 且个数大于0 就可以放技能
        {
            isPressDartButton = true;
            if (num == MGSkillDartInfo.skillHoldLevel)//只有个数等于最大数量时才重置fillAmount
            {
                cdBack.fillAmount = addOrDec ? 0f : 1f;
            }
            --num;
            dartNum.text = num.ToString();
            isCD = true;
            cdBackObject.transform.localScale = new Vector3(direction ? -1 : 1, 1, 1);
            if (MGGlobalDataCenter.defaultCenter().isHost == true)
                MGNotificationCenter.defaultCenter().postNotification(EventEnum.dartFormerEventId, null);
            else
                MGNotificationCenter.defaultCenter().postNotification(EventEnum.dartLatterEventId, null);
        }
    }
}
