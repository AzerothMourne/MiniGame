using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class MGMsgModel{
	public string eventId { get; set;}
    public string gameobjectName { get; set; }
}


public class MGGlobalDataCenter  {
    public bool isNetworkViewEnable;
    public bool isHost;
    public int connecttions;
    public int listenPort;
    private static MGGlobalDataCenter instance;
    public int rRoleBlood, lRoleBlood;
    public GameObject role, roleLater;
    private string _serverIp;
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
    /// <summary>
    /// 屏幕上下左右边框的坐标值
    /// </summary>
    public float screenLiftX, screenRightX,screenTopY,screenBottomY;

    private MGGlobalDataCenter()
    {
        Debug.Log("Init GlobalData");
        this.role = null;
        this.roleLater = null;
        this.connecttions = 1;
        this.listenPort = 8899;
        this.isNetworkViewEnable = false;
        //this.isHost = true;
		this.isHost = false;
        this.rRoleBlood = 1;
        this.lRoleBlood = 2;
        this.screenLiftX = -10.0f;
        this.screenRightX = -1*this.screenLiftX;
        this.screenTopY = 5f;
        this.screenBottomY = -1 * this.screenBottomY;
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
