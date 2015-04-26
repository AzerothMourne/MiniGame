using UnityEngine;
using System.Collections;

public class startUI : MonoBehaviour {
	public GameObject chooseButton;
	public GameObject fuzzyButton;
	public GameObject NGUIRoot;
	public bool isPressStartButton;
	public int countPress;
	public GameObject ChooseUIObj;
	public GameObject FuzzyUIObj;
	public GameObject tianyaObj;
	public GameObject mingyueObj;
	// Use this for initialization
	void Start () {
	
		isPressStartButton = false;
		countPress = 0;
		//加上背景音乐
		AudioManager._instance.MusicBackground();
	}
	
	// Update is called once per frame
	void Update () {

		if (isPressStartButton == true && countPress == 0) {
			createChooseUI();
			createFuzzyBG();
			countPress += 1;
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


	public void OnChoosePlayer_tianya(GameObject button) {
		print ("1 click start tianya");
        this.GetComponent<MyNetworkTest>().findHost();
	}


	public void OnChoosePlayer_mingyue(GameObject button) {
		print ("2 click start mingyue");
        this.GetComponent<MyNetworkTest>().createHost();
	}

	public void createChooseUI() {
		print ("createChooseUI");
		//实例化选择角色的界面
		ChooseUIObj = GameObject.Instantiate (chooseButton, new Vector3 (0, 0, 0), Quaternion.Euler (0, 0, 0)) as GameObject;
		ChooseUIObj.transform.parent = NGUIRoot.transform;
		ChooseUIObj.transform.localPosition = new Vector3 (4, -13, 0);
		ChooseUIObj.transform.localScale = new Vector3 (1, 1, 1);
		ChooseUIObj.GetComponent<UISprite>().depth = 7;
		clicktianya ();
		clickmingyue ();
	}

	public void createFuzzyBG() {
		print ("createFuzzyBG");
		FuzzyUIObj = GameObject.Instantiate(fuzzyButton, new Vector3(0,0,0),Quaternion.Euler(0,0,0)) as GameObject;
		FuzzyUIObj.transform.parent = NGUIRoot.transform;
		FuzzyUIObj.transform.localPosition = new Vector3(-1, 1, 0);
		FuzzyUIObj.transform.localScale = new Vector3(1, 1, 1);
	}
}
