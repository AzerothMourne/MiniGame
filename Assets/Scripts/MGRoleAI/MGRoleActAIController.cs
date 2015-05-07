using UnityEngine;
using System.Collections;

public class MGRoleActAIController : MGRoleAIBase
{
    
	// Use this for initialization
	void Start () {
        initRoleAIData();
	}
    float randAFloat()
    {
        float randNum = Random.Range(0f,1f);
        Debug.Log("-------------------------------randNum= " + randNum);
        return randNum;
    }
	// Update is called once per frame
	void Update () {
        
        roleAITimer += Time.deltaTime;
        if (MGGlobalDataCenter.defaultCenter().isGameOver) return;
        if (roleAITimer >= 0.5f)
        {
            roleAITimer = 0;
            roleAIOnceAct();
        }
	}
    void controlleOtherToJump()
    {
        if (MGGlobalDataCenter.defaultCenter().isLaterRoler)
            MGNotificationCenter.defaultCenter().postNotification(RoleActEventEnum.jumpFormerEventId, null);
        else if (MGGlobalDataCenter.defaultCenter().isFrontRoler)
            MGNotificationCenter.defaultCenter().postNotification(RoleActEventEnum.jumpLatterEventId, null);
    }
    void controlleOtherToDown()
    {
        if (MGGlobalDataCenter.defaultCenter().isLaterRoler)
            MGNotificationCenter.defaultCenter().postNotification(RoleActEventEnum.downToLineFormerEventId, null);
        else if (MGGlobalDataCenter.defaultCenter().isFrontRoler)
            MGNotificationCenter.defaultCenter().postNotification(RoleActEventEnum.dowmToLineLatterEventId, null);
    }
    void roleAIOnceAct()
    {
        
        if (roleJumpScript.jumpCount==2 && roleJumpScript.isGround == false)
        {
            Debug.Log("二段跳下翻");
            controlleOtherToDown();
        }
        else if (roleJumpScript.jumpCount == 1 && roleJumpScript.isGround == false)
        {
            if (randAFloat() <= 0.7)
            {
                Debug.Log("一段跳接二段跳");
                controlleOtherToJump();
            }
            else
            {
                Debug.Log("一段跳接下翻");
                controlleOtherToDown();
            }
        }
        else if (!roleAnimController.isRoll && roleJumpScript.isGround && otherRole.transform.localScale.y > 0)
        {
            if (randAFloat() <= 0.7)
            {
                Debug.Log("在地上跳接跳跃");
                controlleOtherToJump();
            }
            else
            {
                Debug.Log("在地上跳接下翻");
                controlleOtherToDown();
            }
        }
        else if (!roleAnimController.isRoll && roleJumpScript.isGround && otherRole.transform.localScale.y < 0)
        {
            Debug.Log("在绳子下跳接上翻");
            controlleOtherToJump();
        }
    }
}
