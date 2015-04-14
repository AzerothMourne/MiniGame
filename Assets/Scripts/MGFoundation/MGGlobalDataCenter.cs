using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MGGlobalDataCenter  {
    private static MGGlobalDataCenter instance;
    public int rRoleBlood, lRoleBlood;
    /// <summary>
    /// 屏幕上下左右边框的坐标值
    /// </summary>
    public float screenLiftX, screenRightX,screenTopY,screenBottomY;

    private MGGlobalDataCenter()
    {
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
}
