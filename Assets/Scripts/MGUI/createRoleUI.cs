using UnityEngine;
using System.Collections;
using System;
using LitJson;
public static class uiEvent
{
    public static string stopGame = "uiEvent_stopGame";
    public static string continueGame = "uiEvent_continueGame";
    public static string enableAllUIButton = "uiEvent_enableAllUIButton";
}
public class createRoleUI : MonoBehaviour {
    public GameObject dartButton,roadblockButton,beatbackButton;
    public GameObject blinkButton,bonesButton,sprintButton;
    public GameObject downButton, upButton, stopButton,gameTimerLabel;
    public GameObject stopLayer, homeButton, continueButton;
	public Camera uiCamera;
    private GameObject NGUIRoot;
    private GameObject stopLayerObj, homeButtonObj, continueButtonObj;
    private int UILayerMask = 10;
    private MGNetWorking mgNetWorking;
	public float x_k,y_k;
	public float width, height;
    void OnDestroy()
    {
        MGNotificationCenter.defaultCenter().removeObserver(this);
    }
    void Start()
    {
        NGUIRoot = GameObject.Find("UI Root");
        InvokeRepeating("gameTimer", 0, 0.01f);
        mgNetWorking = GameObject.Find("NetWork").GetComponent<MGNetWorking>();
        MGNotificationCenter.defaultCenter().addObserver(this, stopNotification, uiEvent.stopGame);
        MGNotificationCenter.defaultCenter().addObserver(this, continueNotification, uiEvent.continueGame);
        MGNotificationCenter.defaultCenter().addObserver(this, enableAllUIButton, uiEvent.enableAllUIButton);
        createCommonUI();
        if (MGGlobalDataCenter.defaultCenter().isHost == true)
        {
            createFrontRoleUI();
        }
        else
        {
            createLaterRoleUI();
        }

		
    }
    GameObject createOneUI(GameObject gameObject, Vector3 pos)
    {
        GameObject objc = GameObject.Instantiate(gameObject, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
        objc.transform.parent = NGUIRoot.transform;
        objc.layer = UILayerMask;
        objc.transform.position = MGFoundtion.WorldPointToNGUIPoint(pos, uiCamera);
        objc.transform.localScale = new Vector3(MGGlobalDataCenter.defaultCenter().UIScale, MGGlobalDataCenter.defaultCenter().UIScale, 1);
        return objc;
    }
    public void enableAllUIButton(MGNotification notification)
    {
        GameObject[] buttons = GameObject.FindGameObjectsWithTag("UIButton");
        for (int i = 0; i < buttons.Length; ++i)
        {
            buttons[i].GetComponent<UIButton>().enabled = (bool)notification.objc;
        }
    }
    public void gameTimer()
    {
        MGGlobalDataCenter.defaultCenter().totalGameTime -= 0.01f;
        int frontNum = (int)Math.Floor(MGGlobalDataCenter.defaultCenter().totalGameTime);
        int laterNum = (int)((MGGlobalDataCenter.defaultCenter().totalGameTime - frontNum) * 100);
        if (laterNum < 10)
        {
            gameTimerLabel.GetComponent<UILabel>().text = frontNum + ":0" + laterNum;
        }
        else
        {
            gameTimerLabel.GetComponent<UILabel>().text = frontNum + ":" + laterNum;
        }
        if (MGGlobalDataCenter.defaultCenter().totalGameTime <= 0f)
        {
			Debug.Log("dasdasdasdas");
            //游戏结束
			if(GameObject.Find("role").GetComponent<RoleAnimController>().isDead || GameObject.Find("role1").GetComponent<RoleAnimController>().isDead)
				return;
            MGGlobalDataCenter.defaultCenter().overSenceUIName = "victoryFrontGameUI";
            Application.LoadLevel("overSence");
            MGMsgModel gameoverModel = new MGMsgModel();
            gameoverModel.eventId = EventEnum.gameoverEventId;
            gameoverModel.gameobjectName = MGGlobalDataCenter.defaultCenter().overSenceUIName;
            mgNetWorking.sendMessageToPeer(JsonMapper.ToJson(gameoverModel));
        }
        
    }
    public void continueNotification(MGNotification notification)
    {
        if (notification.objc != null)
        {
            continueGame(null);
        }
    }
    public void stopNotification(MGNotification notification)
    {
        if (notification.objc != null)
        {
            clickStop(null);
        }
    }
    public void clickStop(GameObject button)
    {
        if (MGGlobalDataCenter.defaultCenter().isStop == false)
        {

            if (button)
            {
                MGMsgModel uiMsg = new MGMsgModel();
                uiMsg.eventId = uiEvent.stopGame;
                mgNetWorking.sendMessageToPeer(JsonMapper.ToJson(uiMsg));
            }

            MGGlobalDataCenter.defaultCenter().isStop = true;
            Time.timeScale = 0;

            stopLayerObj=createOneUI(stopLayer, Vector3.zero);
            continueButtonObj = createOneUI(continueButton, Vector3.zero);
            continueButtonObj.GetComponent<UISprite>().depth = 3;
            UIEventListener.Get(continueButtonObj).onClick = continueGame;
            homeButtonObj=createOneUI(homeButton,new Vector3(MGGlobalDataCenter.defaultCenter().screenLiftX + MGGlobalDataCenter.defaultCenter().NGUI_ButtonWidth / 2+0.3f, MGGlobalDataCenter.defaultCenter().screenBottomY + MGGlobalDataCenter.defaultCenter().NGUI_ButtonWidth / 2+0.3f, 0f));
            homeButtonObj.GetComponent<UISprite>().depth = 3;
            UIEventListener.Get(homeButtonObj).onClick = homeClick;
        }
    }
    public void continueGame(GameObject button)
    {
        if (MGGlobalDataCenter.defaultCenter().isStop == true)
        {

            if (button)
            {
                MGMsgModel uiMsg = new MGMsgModel();
                uiMsg.eventId = uiEvent.continueGame;
                mgNetWorking.sendMessageToPeer(JsonMapper.ToJson(uiMsg));
            }

            MGGlobalDataCenter.defaultCenter().isStop = false;
            Time.timeScale = 1;
            DestroyObject(stopLayerObj);
            DestroyObject(homeButtonObj);
            DestroyObject(continueButtonObj);
        }
    }
    public void homeClick(GameObject button)
    {
        UILabel label = GameObject.Find("log").GetComponent<UIInput>().label;
        label.text += "\r\nhomeClick";
        MGNetWorking.disconnect();
        label.text += "\r\ndisconnect";
        Application.LoadLevel("startGameScene");
        label.text += "\r\nApplication.LoadLevel(\"startGameScene\")";
        Time.timeScale = 1;
        this.GetComponent<MGInitGameData>().destroyGameData();
    }
    public void createCommonUI()
    {
        print("createCommonUI");
        createOneUI(downButton, new Vector3(MGGlobalDataCenter.defaultCenter().screenLiftX + 1.5f * MGGlobalDataCenter.defaultCenter().NGUI_ButtonWidth, -4f, 0f));
        createOneUI(upButton, new Vector3(MGGlobalDataCenter.defaultCenter().screenRightX - 1f * MGGlobalDataCenter.defaultCenter().NGUI_ButtonWidth, -4f, 0f));
        float padding = 0f;
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            padding = 0.8f;
        }
        else if(Application.platform == RuntimePlatform.Android)
        {
            padding = 0f;
            try
            {
                GameObject.Find("log").GetComponent<UILabel>().text += "\r\n" + MGGlobalDataCenter.defaultCenter().screenLiftX + ";" + MGGlobalDataCenter.defaultCenter().screenTopY;
                GameObject.Find("log").GetComponent<UILabel>().text += "\r\n" + MGGlobalDataCenter.defaultCenter().pixelWidth + ";" + MGGlobalDataCenter.defaultCenter().pixelHight;
                GameObject.Find("log").GetComponent<UILabel>().text += "\r\n" + Camera.current.orthographicSize.ToString();
                GameObject.Find("log").GetComponent<UILabel>().text += "\r\n" + uiCamera.orthographicSize.ToString();
            }
            catch { }
        }
        GameObject objc = createOneUI(stopButton, new Vector3(MGGlobalDataCenter.defaultCenter().screenRightX - MGGlobalDataCenter.defaultCenter().NGUI_ButtonWidth / 2 + padding, MGGlobalDataCenter.defaultCenter().screenTopY - MGGlobalDataCenter.defaultCenter().NGUI_ButtonWidth / 2 + padding, 0f));
        UIEventListener.Get(objc).onClick = clickStop;
        gameTimerLabel = createOneUI(gameTimerLabel, new Vector3(1.2f, MGGlobalDataCenter.defaultCenter().screenTopY, 0));
        gameTimerLabel.GetComponent<UILabel>().text = "60:00";
    }
	public void createFrontRoleUI()
    {
        print("createFrontRoleUI");
        //路障按钮UI
        createOneUI(roadblockButton, new Vector3(MGGlobalDataCenter.defaultCenter().screenRightX - MGGlobalDataCenter.defaultCenter().NGUI_ButtonWidth / 2 - 0.25f, 2.29f, 0f));
        //飞镖按钮UI
        createOneUI(dartButton , new Vector3(MGGlobalDataCenter.defaultCenter().screenRightX - 2.5f * MGGlobalDataCenter.defaultCenter().NGUI_ButtonWidth, -4f, 0f));
        //击退按钮
        createOneUI(beatbackButton, new Vector3(MGGlobalDataCenter.defaultCenter().screenLiftX + MGGlobalDataCenter.defaultCenter().NGUI_ButtonWidth / 2 + 0.25f, 2.29f, 0f)); 
    }
    public void createLaterRoleUI()
    {
        print("createLaterRoleUI");
        //金钟罩按钮UI
        createOneUI(bonesButton, new Vector3(MGGlobalDataCenter.defaultCenter().screenRightX - MGGlobalDataCenter.defaultCenter().NGUI_ButtonWidth / 2 - 0.25f, 2.29f, 0f));
        //冲刺
        createOneUI(sprintButton, new Vector3(MGGlobalDataCenter.defaultCenter().screenLiftX + MGGlobalDataCenter.defaultCenter().NGUI_ButtonWidth / 2 + 0.25f, 2.29f, 0f));
        //闪现按钮UI
        createOneUI(blinkButton, new Vector3(MGGlobalDataCenter.defaultCenter().screenRightX - 2.5f * MGGlobalDataCenter.defaultCenter().NGUI_ButtonWidth, -4f, 0f));
    }
}
