using UnityEngine;
using System.Collections;

public class startUI : MonoBehaviour {
	public GameObject chooseButton;
	public GameObject NGUIRoot;
	public bool isPressStartButton;
	public int countPress;
	// Use this for initialization
	void Start () {
	
		isPressStartButton = false;
		countPress = 0;

		//加上背景音乐
		print ("2");
		AudioManager._instance.MusicBackground();
	}
	
	// Update is called once per frame
	void Update () {

		if (isPressStartButton == true && countPress == 0) {
			createChooseUI();
			countPress += 1;
		}
	}

	public void OnStartButtonClick() {
		isPressStartButton = true;
		print ("click start button");
	}

	public void OnChoosePlayer_tianya() {
		print ("click start tianya");
	}

	public void OnChoosePlayer_mingyue() {
		print ("click start mingyue");
	}

	public void createChooseUI() {
		print ("createChooseUI");
		GameObject objc = GameObject.Instantiate(chooseButton, new Vector3(0,0,0),Quaternion.Euler(0,0,0)) as GameObject;
		objc.transform.parent = NGUIRoot.transform;
		objc.transform.localPosition = new Vector3(4, -13, 0);
		objc.transform.localScale = new Vector3(1, 1, 1);

	}
}
