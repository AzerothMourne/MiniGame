using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class MGMsgModel{
	public string eventId { get; set;}
    public string gameobjectName { get; set; }
	public long timestamp { get; set; }
    public string posJson;
}


public class MGGlobalDataCenter  {
    public float NGUI_ButtonWidth;
    public bool isNetworkViewEnable;
    public bool isHost,isBigSkilling,isStop;
    public int connecttions;
    public int listenPort,mySocketPort;
    private static MGGlobalDataCenter instance;
    public int rRoleBlood, lRoleBlood;
    public GameObject role, roleLater;
    private string _serverIp;
	public float UIScale,roadOrignY;
    public float totalGameTime;
    public string overSenceUIName;
	public bool isDartHit;
	public bool isDartDefence;
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
    /// 屏幕上下左右边框的坐标值
    /// </summary>
    public float screenLiftX, screenRightX,screenTopY,screenBottomY;
    /// <summary>
    /// 屏幕的像素宽高
    /// </summary>
    public float pixelWidth, pixelHight;
    private MGGlobalDataCenter()
    {
        Debug.Log("Init GlobalData");
        //this.isHost = true;
        this.isHost = false;
        this.overSenceUIName = null;
        backToDefaultValues();
	}
    public void backToDefaultValues()
    {
        Time.timeScale = 1;
        this.totalGameTime = 60f;
        this.isStop = false;
        this.isBigSkilling = false;
        this.role = null;
        this.roleLater = null;
        this.connecttions = 1;
        this.listenPort = 8899;
        this.mySocketPort = 10000;
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
