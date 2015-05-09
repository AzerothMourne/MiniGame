using UnityEngine;
using System.Collections;

public class MGRoleLaterSkillAIController : MGRoleAIBase
{
    private float blinkTimer, bonesTimer, sprintTimer;
    private float roadblockGCDTimer;
    private float holdLevel;
    private bool isUseFirstSkill;
	// Use this for initialization
	void Start () {
        initRoleAIData();
        isUseFirstSkill = false;
        blinkTimer = bonesTimer = sprintTimer = 0;
	}
    void firstSkill()
    {
        MGNotificationCenter.defaultCenter().postNotification(SkillActEventEnum.bones, null);
        isUseFirstSkill = true;
        blinkTimer = bonesTimer = sprintTimer = 0;
    }
    void Update()
    {
        if (!isUseFirstSkill && blinkTimer > 0.5f)
        {
            firstSkill();
        }
        blinkTimer += Time.deltaTime;
        bonesTimer += Time.deltaTime;
        sprintTimer += Time.deltaTime;
        if (MGGlobalDataCenter.defaultCenter().isGameOver) return;
        if (blinkTimer > 7f)
        {
            blinkTimer = 0f;
            MGNotificationCenter.defaultCenter().postNotification(SkillActEventEnum.blink, null);
        }
        if (bonesTimer > 8f)
        {
            bonesTimer = 0f;
            MGNotificationCenter.defaultCenter().postNotification(SkillActEventEnum.bones, null);
        }
        if (sprintTimer > 9f)
        {
            sprintTimer = 0f;
            MGNotificationCenter.defaultCenter().postNotification(SkillActEventEnum.sprint, null);
        }
    }
}
