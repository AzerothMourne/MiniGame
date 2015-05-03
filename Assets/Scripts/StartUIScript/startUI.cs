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


	//test
	//public GameObject tianyaLoad;
	public GameObject tianyaLoadObj;
	public Sprite[] tianyaLoadList;
	public bool istianyaLoad;
	public float timer;
	public GameObject tempObj;
	public int count;
	public GameObject start;

	// Use this for initialization
	void Start () {
	
		isPressStartButton = false;
		countPress = 0;
		//加上背景音乐
		AudioManager._instance.MusicBackground();
		isPressSlogan = false;

		istianyaLoad = false;
		timer = 0f;
		count = 0;

	}
	
	// Update is called once per frame
	void Update () {

		if (isPressStartButton == true && countPress == 0) {
			createChooseUI();
			createFuzzyBG();
			creatCloseButton();
			countPress += 1;
		}

		if(isPressSlogan || isPressStartButton && countPress == 1) {
			SloganObj.GetComponent<TypewriterEffect>().charsPerSecond = 2000;
			isPressSlogan = false;
			if(countPress == 1) {
				countPress += 1;
			}
		}


	}

	public void OnStartButtonClick() {
		isPressStartButton = true;
		print ("click start button");
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
		UIEventListener.Get(CloseButtonObj).onClick = OnCloseButton;
	}
	
	public void OnChoosePlayer_tianya(GameObject button) {
		print ("1 click start tianya");
		istianyaLoad = true;
        this.GetComponent<MyNetworkTest>().findHost();

	}

	public void OnChoosePlayer_mingyue(GameObject button) {
		print ("2 click start mingyue");
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
	}

	public void creatCloseButton() {
		print("creatCloseButton");
		CloseButtonObj = GameObject.Instantiate(closeButton, new Vector3(0,0,0),Quaternion.Euler(0,0,0)) as GameObject;
		CloseButtonObj.transform.parent = GameObject.Find("start").transform;
		CloseButtonObj.transform.localPosition = new Vector3(-187, 120, 0);
		CloseButtonObj.transform.localScale = new Vector3(1, 1, 1);
		CloseButtonObj.GetComponent<UISprite>().depth = 8;
		clickCloseButton();
	}

	//标语控制
	public void OnSloganClick() {
		isPressSlogan = true;
		SloganObj = GameObject.Find ("slogan");
		print("OnSloganClick");
	}
}
