using UnityEngine;
using System.Collections;

public class MGRoleFrontSkillAIController : MGRoleAIBase
{
    private float dartTimer,roadblockTimer,beatbackTimer;
    private float roadblockGCDTimer;
    private float holdLevel;
    private bool canUseRoadblock;
	// Use this for initialization
	void Start () {
        initRoleAIData();
        canUseRoadblock = false;
        dartTimer = roadblockTimer = beatbackTimer = 0;
	}
	
	// Update is called once per frame
	void Update () {
        
        dartTimer += Time.deltaTime;
        roadblockTimer += Time.deltaTime;
        beatbackTimer += Time.deltaTime;
        if (MGGlobalDataCenter.defaultCenter().isGameOver) return;
        if (dartTimer > 1f)
        {
            dartTimer = 0f;
            MGNotificationCenter.defaultCenter().postNotification(SkillActEventEnum.dart, null);
        }
        if (roadblockTimer > 6f)
        {
            roadblockTimer = 0f;
            roadblockGCDTimer = 0;
            holdLevel = MGSkillRoadblockInfo.skillHoldLevel - 1;
            MGNotificationCenter.defaultCenter().postNotification(SkillActEventEnum.roadblock, null);
            canUseRoadblock = true;
        }
        if (canUseRoadblock)
        {
            if (holdLevel > 0)
            {
                roadblockGCDTimer += Time.deltaTime;
                if (roadblockGCDTimer > MGSkillRoadblockInfo.skillGCD)
                {
                    roadblockGCDTimer = 0;
                    --holdLevel;
                    MGNotificationCenter.defaultCenter().postNotification(SkillActEventEnum.roadblock, null);
                }
            }
        }
        if (beatbackTimer > 8f)
        {
            beatbackTimer = 0f;
            MGNotificationCenter.defaultCenter().postNotification(SkillActEventEnum.beatback, null);
        }
	}
}
