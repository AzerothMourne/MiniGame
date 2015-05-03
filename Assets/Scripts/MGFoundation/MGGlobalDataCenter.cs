using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class MGMsgModel{
	public string eventId { get; set;}
    public string gameobjectName { get; set; }
	public long timestamp { get; set; }
    public string posJson;
    public string tag;
    public string name;
}


public class MGGlobalDataCenter  {
    public float NGUI_ButtonWidth;
    public bool isNetworkViewEnable;
    public bool isHost,isBigSkilling,isStop;
    public int connecttions;
    public int listenPort,mySocketPort,UPNPPort,SyncPort;
    private static MGGlobalDataCenter instance;
    public int rRoleBlood, lRoleBlood;
    public GameObject role, roleLater;
    private string _serverIp;
	public float UIScale,roadOrignY;
    public float totalGameTime;
    public string overSenceUIName;
    public int dartIndex;
	//@aragornwang
	//play music 
	public bool isDartHit;
	public bool isDartDefence;
	public bool isDartRelease;
	public bool isKillMingyue;
	public bool isRoadBlockHit;
	public bool isRoadBlockDefence;

	public bool isVictory;
	public bool isDefeat;
	public bool isFlash;
	public bool isSprint;
	public bool isRun;

    public string serverIp
    {
        get
        {
            return _serverIp;
        }
        set
        {
            _serverIp = value;
            isNetworkViewEnable = true;
        }
    }
    public Vector3 leftBottomPos, rightTopPos,roleFrontPos,roleLaterPos;
    /// <summary>
    /// 屏幕上下左右边框的坐标值(世界坐标)
    /// </summary>
    public float screenLiftX, screenRightX,screenTopY,screenBottomY;
    /// <summary>
    /// 屏幕的像素宽高
    /// </summary>
    public float pixelWidth, pixelHight;
    private MGGlobalDataCenter()
    {
        Debug.Log("Init GlobalData");
 //       this.isHost = true;
        this.isHost = false;
        this.overSenceUIName = null;
        backToDefaultValues();
	}
    public void backToDefaultValues()
    {
        this.dartIndex = 0;
        Time.timeScale = 1;
        this.totalGameTime = 60f;
        this.isStop = false;
        this.isBigSkilling = false;
        this.role = null;
        this.roleLater = null;
        this.connecttions = 1;
        this.listenPort = 8899;
        this.SyncPort = 12000;
        this.mySocketPort = 10000;
        this.serverIp = "127.0.0.0";
        this.isNetworkViewEnable = false;
        this.rRoleBlood = 1;
        this.lRoleBlood = 2;
        this.screenLiftX = -8.9f;
        this.screenRightX = -1 * this.screenLiftX;

        this.screenTopY = 5f;
        this.screenBottomY = -1 * this.screenBottomY;
        this.NGUI_ButtonWidth = 1.65f;
        this.UIScale = 1.5f;
        this.roadOrignY = -1000;
        this.leftBottomPos = this.rightTopPos = this.roleFrontPos = this.roleLaterPos = Vector3.zero;
        this.isDartHit = false;
        this.isDartDefence = false;
		this.isRoadBlockHit = false;
		this.isVictory = false;
		this.isDefeat = false;
		this.isRoadBlockDefence = false;
		this.isFlash = false;
		this.isSprint = false;
		this.isRun = false;
    }
    public static MGGlobalDataCenter defaultCenter()
    {
		if (instance == null) {
            instance = new MGGlobalDataCenter();
		}
		return instance;
	}
    public static long timestamp()
    {
        TimeSpan ts = DateTime.Now - DateTime.Parse("1970-1-1");
        return (long)Math.Floor(ts.TotalMilliseconds);
    }
    

}
