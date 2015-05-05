using UnityEngine;
using System.Collections;

public class startUI : MonoBehaviour {
	public GameObject chooseButton;
	public GameObject fuzzyButton;
	public GameObject closeButton;
	public GameObject NGUIRoot;
	public bool isPressStartButton;
	public int countPress;
	public GameObject ChooseUIObj;
	public GameObject FuzzyUIObj;
	public GameObject CloseButtonObj;
	public GameObject tianyaObj;
	public GameObject mingyueObj;
	public bool isPressSlogan; //标语
	public GameObject SloganObj;

	public Sprite[] tianyaLoadList;
	public bool istianyaLoad;
	public float timer;
	public int count;

	public Sprite[] mingyueLoadList;
	public bool ismingyueLoad;
    public bool isHood;
    public Sprite[] hoodList;
    public float timerHood;
    public int countHood;

	// Use this for initialization
	void Start () {
	
		isPressStartButton = false;
		countPress = 0;
		//加上背景音乐
		AudioManager._instance.MusicBackground();
		isPressSlogan = false;
		istianyaLoad = false;
		ismingyueLoad = false;
		timer = 0f;
		count = 0;
        isHood = true;
        timerHood = 0f;
        countHood = 0;
        
	}

    //@Test@Annyqzhou
    //void PlayObjecet(string spritename, Sprite[] spriteVec, int nCount = 4)
    //{
    //    timer += Time.deltaTime;
    //    if (timer >= 0.1f)
    //    {
    //        spritename = spriteVec[count].name;
    //        timer = 0f;
    //        count = (++count) % nCount;
    //        print(count);
    //        print(spriteVec[count].name);
    //    }
    //}
	
	// Update is called once per frame
    void Update()
    {

        if (isPressStartButton == true && countPress == 0)
        {
            createChooseUI();
            createFuzzyBG();
            creatCloseButton();
            countPress += 1;
        }

        if (isPressSlogan || isPressStartButton && countPress == 1)
        {
            SloganObj.GetComponent<TypewriterEffect>().charsPerSecond = 2000;
            isPressSlogan = false;
            if (countPress == 1)
            {
                countPress += 1;
            }
        }

        //天涯load播放帧
        if (istianyaLoad)
        {
            timer += Time.deltaTime;
            if (timer >= 0.1f)
            {                
				try{
					GameObject.Find("tianya").GetComponent<UIButton>().normalSprite = tianyaLoadList[count].name;
				}
				catch{}
                timer = 0f;
                count = (++count) % 5;
            }
        }
      
        if (ismingyueLoad)
        {            
            timer += Time.deltaTime;
            if (timer >= 0.1f)
            {                
				try{
					GameObject.Find("mingyue").GetComponent<UIButton>().normalSprite = mingyueLoadList[count].name;
				}
				catch{}
                timer = 0f;
                count = (++count) % 5;
            }
        }

        if (isHood)
        {
            timerHood += Time.deltaTime;            
            if (timerHood >= 0.2f)
            {
                GameObject.Find("player").GetComponent<UISprite>().spriteName = hoodList[countHood].name;
                timerHood = 0f;
                countHood = (++countHood) % 5;
            }
        }        
    }

	public void OnStartButtonClick() {
		isPressStartButton = true;
		print ("click start button");
	}

    public void OnGuideButtonClick()
    {
        MGGlobalDataCenter.defaultCenter().isSingle = true;
        Application.LoadLevel("guideScene");
    }
	
	public void clicktianya() {
		print ("click tianya");
		tianyaObj = GameObject.Find ("tianya");
		UIEventListener.Get(tianyaObj).onClick = OnChoosePlayer_tianya;
		tianyaObj.GetComponent<UISprite>().depth = 7;
	}

	public void clickmingyue() {
		print ("click mingyue");
		mingyueObj = GameObject.Find ("mingyue");
		UIEventListener.Get(mingyueObj).onClick = OnChoosePlayer_mingyue;
		mingyueObj.GetComponent<UISprite>().depth = 8;
	}

	public void clickCloseButton() {
		print("click CloseButton ");
		istianyaLoad = false;
		ismingyueLoad = false;
		timer = 0f;
		UIEventListener.Get(CloseButtonObj).onClick = OnCloseButton;
	}
	
	public void OnChoosePlayer_tianya(GameObject button) {
		print ("1 click start tianya");
		if(ismingyueLoad == false)
			istianyaLoad = true;
		if(istianyaLoad)
        	this.GetComponent<MyNetworkTest>().findHost();

	}

	public void OnChoosePlayer_mingyue(GameObject button) {
		print ("2 click start mingyue");
		if(istianyaLoad == false)
			ismingyueLoad = true;
		if(ismingyueLoad)
        	this.GetComponent<MyNetworkTest>().createHost();
	}

	public void OnCloseButton(GameObject button) {
		print ("3 click CloseButton");
        this.GetComponent<MyNetworkTest>().cancelConnect();
		resetParams();
		Destroy(ChooseUIObj);
		Destroy(FuzzyUIObj);
		Destroy(tianyaObj);
		Destroy(mingyueObj);
		Destroy(CloseButtonObj);
	}

	public void resetParams() {
		isPressStartButton = false;
		countPress = 0;
		isPressSlogan = true;
	}
	
	public void createChooseUI() {
		print ("createChooseUI");
		//实例化选择角色的界面
		ChooseUIObj = GameObject.Instantiate (chooseButton, new Vector3 (0, 0, 0), Quaternion.Euler (0, 0, 0)) as GameObject;
		ChooseUIObj.transform.parent = GameObject.Find("start").transform;
        ChooseUIObj.layer = 10;
		ChooseUIObj.transform.localPosition = new Vector3 (0, 0, 0);
		ChooseUIObj.transform.localScale = new Vector3 (1, 1, 1);
		ChooseUIObj.GetComponent<UISprite>().depth = 7;
		clicktianya ();
		clickmingyue ();
	}

	public void createFuzzyBG() {
		print ("createFuzzyBG");
		FuzzyUIObj = GameObject.Instantiate(fuzzyButton, new Vector3(0,0,0),Quaternion.Euler(0,0,0)) as GameObject;
		FuzzyUIObj.transform.parent = GameObject.Find("start").transform;
		FuzzyUIObj.transform.localPosition = new Vector3(0, 0, 0);
		FuzzyUIObj.transform.localScale = new Vector3(1, 1, 1);
        FuzzyUIObj.layer = 10;
	}

	public void creatCloseButton() {
		print("creatCloseButton");
		CloseButtonObj = GameObject.Instantiate(closeButton, new Vector3(0,0,0),Quaternion.Euler(0,0,0)) as GameObject;
		CloseButtonObj.transform.parent = GameObject.Find("start").transform;
		CloseButtonObj.transform.localPosition = new Vector3(185, 120, 0);
		CloseButtonObj.transform.localScale = new Vector3(1, 1, 1);
		CloseButtonObj.GetComponent<UISprite>().depth = 8;
        CloseButtonObj.layer = 10;
		clickCloseButton();
	}

	//标语控制
	public void OnSloganClick() {
		isPressSlogan = true;
		SloganObj = GameObject.Find ("slogan");
		print("OnSloganClick");
	}
}
