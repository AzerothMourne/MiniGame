using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MGGlobalDataCenter  {
    private static MGGlobalDataCenter instance;
    public int rRoleBlood, lRoleBlood;

    private MGGlobalDataCenter()
    {
        this.rRoleBlood = 1;
        this.lRoleBlood = 2;
	}

    public static MGGlobalDataCenter defaultCenter()
    {
		if (instance == null) {
            instance = new MGGlobalDataCenter();
		}
		return instance;
	}
}
