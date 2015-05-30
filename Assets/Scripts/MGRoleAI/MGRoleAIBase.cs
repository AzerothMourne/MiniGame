using UnityEngine;
using System.Collections;

public class MGRoleAIBase : MonoBehaviour {
    protected Jump roleJumpScript;
    protected RoleAnimController roleAnimController;
    protected float roleAITimer;
    protected GameObject otherRole;
	// Use this for initialization

    public void initRoleAIData()
    {
        if (MGGlobalDataCenter.defaultCenter().isLaterRoler)
            otherRole = MGGlobalDataCenter.defaultCenter().role;
        else if (MGGlobalDataCenter.defaultCenter().isFrontRoler)
            otherRole = MGGlobalDataCenter.defaultCenter().roleLater;
        roleJumpScript = this.GetComponent<Jump>();
        roleAnimController = this.GetComponent<RoleAnimController>();
        roleAITimer = 0;
    }
	// Update is called once per frame

}
