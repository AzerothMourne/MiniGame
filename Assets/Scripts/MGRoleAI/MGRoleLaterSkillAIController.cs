using UnityEngine;
using System.Collections;

public class MGRoleLaterSkillAIController : MGRoleAIBase
{
    private float blinkTimer, bonesTimer, sprintTimer;
    private float roadblockGCDTimer;
    private float holdLevel;
    private bool canUseRoadblock;
	// Use this for initialization
	void Start () {
        initRoleAIData();
        canUseRoadblock = false;
        blinkTimer = bonesTimer = sprintTimer = 0;
	}

    void Update()
    {
        blinkTimer += Time.deltaTime;
        bonesTimer += Time.deltaTime;
        sprintTimer += Time.deltaTime;
        if (MGGlobalDataCenter.defaultCenter().isGameOver) return;
        if (blinkTimer > 6f)
        {
            blinkTimer = 0f;
            MGNotificationCenter.defaultCenter().postNotification(SkillActEventEnum.blink, null);
        }
        if (bonesTimer > 7f)
        {
            bonesTimer = 0f;
            MGNotificationCenter.defaultCenter().postNotification(SkillActEventEnum.bones, null);
        }
        if (sprintTimer > 8f)
        {
            sprintTimer = 0f;
            MGNotificationCenter.defaultCenter().postNotification(SkillActEventEnum.sprint, null);
        }
    }
}
