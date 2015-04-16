using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Personnel
{

    public int Id { get; set; }

    public string Name { get; set; }

}
public class MGGlobalDataCenter  {
    public int isHost;
    private static MGGlobalDataCenter instance;
    public int rRoleBlood, lRoleBlood;
    /// <summary>
    /// 屏幕上下左右边框的坐标值
    /// </summary>
    public float screenLiftX, screenRightX,screenTopY,screenBottomY;
    /*
    switch (msg) {
		    case "1"://前面的角色放飞镖
			    break;
		    case "2"://前面的角色一段跳
			    break;
		    case "3"://前面的角色二段跳
			    break;
            case "4"://前面的角色下翻
                break;
            case "5"://后面的角色一段跳
                break;
            case "6"://后面的角色二段跳
                break;
            case "7"://后面的角色下翻
                break;
		}
     * */
    private MGGlobalDataCenter()
    {
        this.isHost = 0;
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
    public static string timestamp()
    {
        TimeSpan ts = DateTime.Now - DateTime.Parse("1970-1-1");
        return ts.TotalMilliseconds.ToString();
    }
    

}
