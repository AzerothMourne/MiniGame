﻿using UnityEngine;
using System.Collections;

public static class MGGuideManagerState
{
    public static int jump = 1 << 0;
    public static int secondJump = 1 << 1;
    public static int downToLine = 1 << 2;
    public static int bones = 1 << 3;
    public static int blink = 1 << 4;
    public static int beatback = 1 << 5;
    public static int sprint = 1 << 6;
}
public class MGGuideManager : MonoBehaviour {
    public Camera uiCamera;
    private GameObject roleLater, roleFront;
    private Jump roleLaterJumpScript, roleFrontJumpScript;
    private int guideMask,guideEndMask;
    private MGSkillsBase skillObjc;
    private GameObject[] UIButtons;
    private GameObject gameTimer, stopButton;
    private bool isHiddenButtons,flag;
    private float roadblockHoldLevel, roadblockGCDTimer;
	void Start () {
        roleLater = MGGlobalDataCenter.defaultCenter().roleLater;
        roleFront = MGGlobalDataCenter.defaultCenter().role;
        roleLaterJumpScript = roleLater.GetComponent<Jump>();
        roleFrontJumpScript = roleFront.GetComponent<Jump>();
        guideMask = 0;
        guideEndMask = 0;
        isHiddenButtons = false;
        UIButtons = null;
        flag = false;
	}
    public void darkMidClick()
    {
        if ((guideEndMask & MGGuideManagerState.jump) == 0)
        {
            Time.timeScale = 1;
            MGNotificationCenter.defaultCenter().postNotification(RoleActEventEnum.jumpLatterEventId, null);
            GameObject.Find("NetWork").GetComponent<MGGuideDarkLayer>().destoryDarkLayer();
            foreach (GameObject uiButton in UIButtons)
            {
                if (uiButton.name == "upButton(Clone)")
                {
                    uiButton.SetActive(false);
                    break;
                }
            }
            guideEndMask |= MGGuideManagerState.jump;
        }
        else if ((guideEndMask & MGGuideManagerState.secondJump) == 0)
        {
            if (flag == false)
            {
                Time.timeScale = 1;
                MGNotificationCenter.defaultCenter().postNotification(RoleActEventEnum.jumpLatterEventId, null);
                GameObject.Find("NetWork").GetComponent<MGGuideDarkLayer>().destoryDarkLayer();
                flag = true;
            }
            else if (flag == true)
            {
                Debug.Log("second jump click");
                flag = false;
                Time.timeScale = 1;
                MGNotificationCenter.defaultCenter().postNotification(RoleActEventEnum.jumpLatterEventId, null);
                GameObject.Find("NetWork").GetComponent<MGGuideDarkLayer>().destoryDarkLayer();
                foreach (GameObject uiButton in UIButtons)
                {
                    if (uiButton.name == "upButton(Clone)")
                    {
                        uiButton.SetActive(false);
                        break;
                    }
                }
                guideEndMask |= MGGuideManagerState.secondJump;
            }
        }
        else if ((guideEndMask & MGGuideManagerState.downToLine) == 0)
        {
            Time.timeScale = 1;
            MGNotificationCenter.defaultCenter().postNotification(RoleActEventEnum.dowmToLineLatterEventId, null);
            MGNotificationCenter.defaultCenter().postNotification(RoleActEventEnum.downToLineFormerEventId, null);
            GameObject.Find("NetWork").GetComponent<MGGuideDarkLayer>().destoryDarkLayer();
            foreach (GameObject uiButton in UIButtons)
            {
                if (uiButton.name == "downButton(Clone)")
                {
                    uiButton.SetActive(false);
                    break;
                }
            }
            guideEndMask |= MGGuideManagerState.downToLine;
        }
        else if ((guideEndMask & MGGuideManagerState.bones) == 0)
        {
            Time.timeScale = 1;
            roadblockGCDTimer = 0f;
            MGNotificationCenter.defaultCenter().postNotification(SkillActEventEnum.bones, null);
            GameObject.Find("NetWork").GetComponent<MGGuideDarkLayer>().destoryDarkLayer();
            foreach (GameObject uiButton in UIButtons)
            {
                if (uiButton.name == "bonesButton(Clone)")
                {
                    uiButton.SetActive(false);
                    break;
                }
            }
            guideEndMask |= MGGuideManagerState.bones;
        }
        else if ((guideEndMask & MGGuideManagerState.blink) == 0)
        {
            Time.timeScale = 1;
            roleLaterJumpScript.skillsBlink();
            GameObject.Find("NetWork").GetComponent<MGGuideDarkLayer>().destoryDarkLayer();
            foreach (GameObject uiButton in UIButtons)
            {
                if (uiButton.name == "blinkButton(Clone)")
                {
                    uiButton.SetActive(false);
                    break;
                }
            }
            guideEndMask |= MGGuideManagerState.blink;
        }
        else if ((guideEndMask & MGGuideManagerState.sprint) == 0)
        {
            Time.timeScale = 1;
            roleLaterJumpScript.skillsSprint();
            MGNotificationCenter.defaultCenter().postNotification(RoleActEventEnum.jumpFormerEventId, null);
            MGNotificationCenter.defaultCenter().postNotification(RoleActEventEnum.jumpLatterEventId, null);
            GameObject.Find("NetWork").GetComponent<MGGuideDarkLayer>().destoryDarkLayer();
            foreach (GameObject uiButton in UIButtons)
            {
                if (uiButton.name == "sprintButton(Clone)")
                {
                    uiButton.SetActive(false);
                    break;
                }
            }
            guideEndMask |= MGGuideManagerState.sprint;
        }
        skillObjc = null;
    }
	// Update is called once per frame
	void Update () {
        if (!isHiddenButtons)
        {
            UIButtons = GameObject.FindGameObjectsWithTag("UIButton");
            if (UIButtons != null && UIButtons.Length > 0)
            {
                Debug.Log("hidden all buttons");
                isHiddenButtons = true;
                foreach (GameObject uiButton in UIButtons)
                {
                    uiButton.SetActive(false);
                }
                try
                {
                    gameTimer = GameObject.Find("gameTimer(Clone)");
                    stopButton = GameObject.Find("stopButton(Clone)");
                    gameTimer.SetActive(false);
                    stopButton.SetActive(false);
                }
                catch { }
            }
        }

        guideJump();
        guideSecondJump();
        guideDownToLine();
        guideUp();
        guideBones();
        guideBlink();
        guideBeatback();
        guideSprint();
	}
    void guideJump()
    {
        //一段跳引导
        if ((guideMask & MGGuideManagerState.jump) == 0)
        {
            if (roleFrontJumpScript.isGround)
            {
                guideMask |= MGGuideManagerState.jump;
                skillObjc = roleFrontJumpScript.skillsDart();
            }
        }
        if ((guideEndMask & MGGuideManagerState.jump) == 0)
        {
            //Debug.Log("guideJump已经开始");
            if (skillObjc != null && skillObjc.transform.position.x - roleLater.transform.position.x <= 3f)
            {
                //Time.timeScale = 0;
                skillObjc = null;
                foreach (GameObject uiButton in UIButtons)
                {
                    if (uiButton.name == "upButton(Clone)")
                    {
                        uiButton.SetActive(true);
                        uiButton.GetComponent<UIButton>().enabled = false;
                        this.GetComponent<MGGuideDarkLayer>().createDarkLayerInPos(MGFoundtion.NGUIPointToWorldPoint(uiButton.transform.position, uiCamera));
                        break;
                    }
                }
                Time.timeScale = 0;
            }
        }
    }
    void guideSecondJump()
    {
        //二段跳引导
        if ((guideEndMask & MGGuideManagerState.jump) != 0 && (guideMask & MGGuideManagerState.secondJump) == 0)
        {
            if (roleLaterJumpScript.isGround)
            {
                guideMask |= MGGuideManagerState.secondJump;
                roadblockGCDTimer = 0;
                roadblockHoldLevel = MGSkillRoadblockInfo.skillHoldLevel - 1;
                skillObjc = roleFrontJumpScript.skillsRoadblock();
            }
        }
        if ((guideEndMask & MGGuideManagerState.secondJump) == 0 && (guideMask & MGGuideManagerState.secondJump) != 0)
        {
            if (roadblockHoldLevel > 0)
            {
                roadblockGCDTimer += Time.deltaTime;
                if (roadblockGCDTimer > MGSkillRoadblockInfo.skillGCD)
                {
                    roadblockGCDTimer = 0;
                    --roadblockHoldLevel;
                    roleFrontJumpScript.skillsRoadblock();
                }
            }
            //Debug.Log("guideJump已经开始");
            if (skillObjc != null && skillObjc.transform.position.x - roleLater.transform.position.x <= 3f)
            {
                //Time.timeScale = 0;
                skillObjc = null;
                foreach (GameObject uiButton in UIButtons)
                {
                    if (uiButton.name == "upButton(Clone)")
                    {
                        uiButton.SetActive(true);
                        uiButton.GetComponent<UIButton>().enabled = false;
                        this.GetComponent<MGGuideDarkLayer>().createDarkLayerInPos(MGFoundtion.NGUIPointToWorldPoint(uiButton.transform.position, uiCamera));
                        break;
                    }
                }
                Time.timeScale = 0;
            }
            if (flag == true && roleLater.rigidbody2D.velocity.y < -2f && Time.timeScale != 0)
            {
                Debug.Log("second jump");
                foreach (GameObject uiButton in UIButtons)
                {
                    if (uiButton.name == "upButton(Clone)")
                    {
                        this.GetComponent<MGGuideDarkLayer>().createDarkLayerInPos(MGFoundtion.NGUIPointToWorldPoint(uiButton.transform.position, uiCamera));
                        break;
                    }
                }
                Time.timeScale = 0;
            }
        }
    }
    void guideDownToLine()
    {
        if ((guideEndMask & MGGuideManagerState.secondJump) != 0 && (guideMask & MGGuideManagerState.downToLine) == 0)
        {
            if (roleLaterJumpScript.isGround)
            {
                MGNotificationCenter.defaultCenter().postNotification(RoleActEventEnum.jumpFormerEventId, null);
                guideMask |= MGGuideManagerState.downToLine;
                flag = false;
                roadblockGCDTimer = 0;
                roadblockHoldLevel = MGSkillRoadblockInfo.skillHoldLevel;
            }
        }
        if ((guideEndMask & MGGuideManagerState.downToLine) == 0 && (guideMask & MGGuideManagerState.downToLine) != 0)
        {
            if (flag==false && roleFront.rigidbody2D.velocity.y <= 0f && roleFrontJumpScript.isGround == false)
            {
                MGNotificationCenter.defaultCenter().postNotification(RoleActEventEnum.jumpFormerEventId, null);
                flag = true;
            }
            else if (flag && roleFrontJumpScript.isGround == false && roleFront.rigidbody2D.velocity.y <= 0f && roleFront.transform.position.y >= roleFront.GetComponent<SpriteRenderer>().bounds.size.y + MGGlobalDataCenter.defaultCenter().roadOrignY)
            {
                roadblockGCDTimer += Time.deltaTime;
                if (roadblockGCDTimer > 0.05f)
                {
                    roadblockGCDTimer = 0;
                    skillObjc = roleFrontJumpScript.skillsDart();
                }
            }
            else if (skillObjc !=null && roleFront.transform.position.y < roleFront.GetComponent<SpriteRenderer>().bounds.size.y + MGGlobalDataCenter.defaultCenter().roadOrignY)
            {
                if (roadblockHoldLevel > 0)
                {
                    if (roadblockHoldLevel == MGSkillRoadblockInfo.skillHoldLevel)
                    {
                        roadblockGCDTimer = 0;
                        --roadblockHoldLevel;
                        roleFrontJumpScript.skillsRoadblock();
                    }
                    roadblockGCDTimer += Time.deltaTime;
                    if (roadblockGCDTimer > MGSkillRoadblockInfo.skillGCD)
                    {
                        roadblockGCDTimer = 0;
                        --roadblockHoldLevel;
                        roleFrontJumpScript.skillsRoadblock();
                    }
                }
            }
            if (skillObjc != null && skillObjc.transform.position.x - roleLater.transform.position.x <= 3f)
            {
                skillObjc = null;
                foreach (GameObject uiButton in UIButtons)
                {
                    if (uiButton.name == "downButton(Clone)")
                    {
                        uiButton.SetActive(true);
                        uiButton.GetComponent<UIButton>().enabled = false;
                        this.GetComponent<MGGuideDarkLayer>().createDarkLayerInPos(MGFoundtion.NGUIPointToWorldPoint(uiButton.transform.position, uiCamera));
                        break;
                    }
                }
                Time.timeScale = 0;
            }
        }
    }
    void guideUp()
    {
        
    }
    void guideBones()
    {
        if ((guideEndMask & MGGuideManagerState.downToLine) != 0 && (guideMask & MGGuideManagerState.bones) == 0)
        {
            Debug.Log("123123");
            if (roleFront.transform.localScale.y < 0)
            {
                guideMask |= MGGuideManagerState.bones;
                skillObjc = roleFrontJumpScript.skillsDart();
                roadblockGCDTimer = 0;
            }
        }
        if ((guideEndMask & MGGuideManagerState.bones) == 0 && (guideMask & MGGuideManagerState.bones) != 0)
        {
            if (skillObjc != null && skillObjc.transform.position.x - roleLater.transform.position.x <= 3f)
            {
                skillObjc = null;
                foreach (GameObject uiButton in UIButtons)
                {
                    if (uiButton.name == "bonesButton(Clone)")
                    {
                        uiButton.SetActive(true);
                        uiButton.GetComponent<UIButton>().enabled = false;
                        this.GetComponent<MGGuideDarkLayer>().createDarkLayerInPos(MGFoundtion.NGUIPointToWorldPoint(uiButton.transform.position, uiCamera));
                        break;
                    }
                }
                Time.timeScale = 0;
            }
            if (skillObjc != null)
            {
                roadblockGCDTimer += Time.deltaTime;
                if (roadblockGCDTimer > 0.25f)
                {
                    roadblockGCDTimer = 0;
                    roleFrontJumpScript.skillsDart();
                }
            }
        }
    }
    void guideBlink()
    {
        if ((guideEndMask & MGGuideManagerState.bones) != 0 && (guideMask & MGGuideManagerState.blink) == 0)
        {
            roadblockGCDTimer += Time.deltaTime;
            if (roadblockGCDTimer+0.25f > MGSkillBonesInfo.durationTime)
            {
                guideMask |= MGGuideManagerState.blink;
                skillObjc = roleFrontJumpScript.skillsDart();
                flag = true;
            }
        }
        if ((guideEndMask & MGGuideManagerState.blink) == 0 && (guideMask & MGGuideManagerState.blink) != 0)
        {
            roadblockGCDTimer += Time.deltaTime;
            if (roadblockGCDTimer >= MGSkillBonesInfo.durationTime+0.1f && flag)
            {
                flag = false;
                foreach (GameObject uiButton in UIButtons)
                {
                    if (uiButton.name == "blinkButton(Clone)")
                    {
                        uiButton.SetActive(true);
                        uiButton.GetComponent<UIButton>().enabled = false;
                        this.GetComponent<MGGuideDarkLayer>().createDarkLayerInPos(MGFoundtion.NGUIPointToWorldPoint(uiButton.transform.position, uiCamera));
                        break;
                    }
                }
                Time.timeScale = 0;
            }
        }
    }
    void guideBeatback()
    {
        if ((guideEndMask & MGGuideManagerState.blink) != 0 && (guideMask & MGGuideManagerState.beatback) == 0)
        {
            guideMask |= MGGuideManagerState.beatback;
            roleFrontJumpScript.skillsBeatback();
            roadblockGCDTimer = 0;
        }
        if ((guideEndMask & MGGuideManagerState.beatback) == 0 && (guideMask & MGGuideManagerState.beatback) != 0)
        {

            roadblockGCDTimer += Time.deltaTime;
            if (roadblockGCDTimer >= MGSkillBeatbackInfo.durationTime+0.5f)
            {
                guideEndMask |= MGGuideManagerState.beatback;
            }
        }
    }
    void guideSprint()
    {
        if ((guideEndMask & MGGuideManagerState.beatback) != 0 && (guideMask & MGGuideManagerState.sprint) == 0)
        {
            Time.timeScale = 0;
            guideMask |= MGGuideManagerState.sprint;
            
            foreach (GameObject uiButton in UIButtons)
            {
                if (uiButton.name == "sprintButton(Clone)")
                {
                    uiButton.SetActive(true);
                    uiButton.GetComponent<UIButton>().enabled = false;
                    this.GetComponent<MGGuideDarkLayer>().createDarkLayerInPos(MGFoundtion.NGUIPointToWorldPoint(uiButton.transform.position, uiCamera));
                    break;
                }
            }
            
        }
    }
}
