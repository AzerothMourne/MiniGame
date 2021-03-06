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
    public static int up = 1 << 7;
    public static int dart = 1 << 8;
    public static int roadblock = 1 << 9;
}
public class MGGuideManager : MonoBehaviour {
    public Camera uiCamera;
    public GameObject NGUIRoot;
    public GameObject guideLabel;
    private GameObject roleLater, roleFront;
    private Jump roleLaterJumpScript, roleFrontJumpScript;
    private int guideMask,guideEndMask;
    private MGSkillsBase skillObjc;
    private GameObject[] UIButtons;
    private GameObject gameTimer, stopButton;
    private bool isHiddenButtons,flag,isGuideEnd,isReStart;
    private float roadblockHoldLevel, roadblockGCDTimer, guideDelayTimer, guideLastStepTimer, guideStepTimer;
    private Vector3 roleLaterStartPos,roleFrontStartPos;
    private int guideDartStep;
	void Start () {
        roleLater = MGGlobalDataCenter.defaultCenter().roleLater;
        roleLaterStartPos = roleLater.transform.position;
        roleFront = MGGlobalDataCenter.defaultCenter().role;
        roleFrontStartPos = roleFront.transform.position;

        if (MGGlobalDataCenter.defaultCenter().isLaterRoler)
        {
            //先删除AI脚本 
            Object Script = roleFront.GetComponent<MGRoleActAIController>();
            Destroy(Script);
            Script = roleFront.GetComponent<MGRoleFrontSkillAIController>();
            Destroy(Script);
        }
        else
        {
            //先删除AI脚本 
            Object Script = roleLater.GetComponent<MGRoleActAIController>();
            Destroy(Script);
            Script = roleLater.GetComponent<MGRoleLaterSkillAIController>();
            Destroy(Script);
        }

        roleLaterJumpScript = roleLater.GetComponent<Jump>();
        roleFrontJumpScript = roleFront.GetComponent<Jump>();
        guideMask = 0;
        guideEndMask = 0;
        guideDelayTimer = 0;
        guideLastStepTimer = 0;
        guideDartStep = 1;
        guideStepTimer = 0;
        isHiddenButtons = false;
        UIButtons = null;
        isReStart = false;
        flag = false;
        isGuideEnd = false;
        guideLabel = GameObject.Instantiate(guideLabel, new Vector3(0,0,0), Quaternion.Euler(0, 0, 0)) as GameObject;
        guideLabel.transform.parent = NGUIRoot.transform;
        guideLabel.transform.position = MGFoundtion.WorldPointToNGUIPoint(new Vector3(0, 2.11f, 0), uiCamera);
        guideLabel.transform.localScale = new Vector3(1, 1, 1);
        guideLabel.layer = 10;
        guideLabel.GetComponent<UILabel>().text = "";
        guideLabel.SetActive(false);
	}

    void delaySetGameOver()
    {
        Debug.Log("delaySetGameOver");
        MGGlobalDataCenter.defaultCenter().isGameOver = false;
    }
    void showButtonAndLabel(string buttonName, string labelString)
    {
        foreach (GameObject uiButton in UIButtons)
        {
            if (uiButton.name == buttonName)
            {
                uiButton.SetActive(true);
                uiButton.GetComponent<UIButton>().enabled = false;
                this.GetComponent<MGGuideDarkLayer>().createDarkLayerInPos(MGFoundtion.NGUIPointToWorldPoint(uiButton.transform.position, uiCamera));
                break;
            }
        }
        guideLabel.GetComponent<UILabel>().text = labelString;
        guideLabel.SetActive(true);
        Time.timeScale = 0;
    }
    public void roleFrontGuideClick()
    {
        if ((guideEndMask & MGGuideManagerState.jump) == 0)
        {
            Time.timeScale = 1;
            MGNotificationCenter.defaultCenter().postNotification(RoleActEventEnum.jumpFormerEventId, null);
            this.GetComponent<MGGuideDarkLayer>().destoryDarkLayer();
            foreach (GameObject uiButton in UIButtons)
            {
                if (uiButton.name == "upButton(Clone)")
                {
                    uiButton.SetActive(false);
                    break;
                }
            }
            guideEndMask |= MGGuideManagerState.jump;
            guideLastStepTimer = guideDelayTimer;
        }
        else if ((guideEndMask & MGGuideManagerState.secondJump) == 0)
        {
            Time.timeScale = 1;
            MGNotificationCenter.defaultCenter().postNotification(RoleActEventEnum.jumpFormerEventId, null);
            this.GetComponent<MGGuideDarkLayer>().destoryDarkLayer();
            foreach (GameObject uiButton in UIButtons)
            {
                if (uiButton.name == "upButton(Clone)")
                {
                    uiButton.SetActive(false);
                    break;
                }
            }
            guideEndMask |= MGGuideManagerState.secondJump;
            guideLastStepTimer = guideDelayTimer;
        }
        else if ((guideEndMask & MGGuideManagerState.downToLine) == 0)
        {
            Time.timeScale = 1;
            foreach (GameObject uiButton in UIButtons)
            {
                if (uiButton.name == "downButton(Clone)")
                {
                    uiButton.GetComponent<Down>().OnMouseDown();
                    uiButton.SetActive(false);
                    break;
                }
            }
            this.GetComponent<MGGuideDarkLayer>().destoryDarkLayer();
            guideEndMask |= MGGuideManagerState.downToLine;
            guideLastStepTimer = guideDelayTimer;
        }
        else if ((guideEndMask & MGGuideManagerState.up) == 0)
        {
            Time.timeScale = 1;
            foreach (GameObject uiButton in UIButtons)
            {
                if (uiButton.name == "downButton(Clone)")
                {
                    uiButton.GetComponent<Down>().OnMouseDown();
                    uiButton.SetActive(false);
                    break;
                }
            }
            this.GetComponent<MGGuideDarkLayer>().destoryDarkLayer();
            guideEndMask |= MGGuideManagerState.up;
            guideLastStepTimer = guideDelayTimer;
        }
        else if ((guideEndMask & MGGuideManagerState.dart) == 0)
        {
            Time.timeScale = 1;
            string buttonName = null;
            switch (guideDartStep)
            {
                case 2:
                    buttonName = "dartButton(Clone)";
                    break;
                case 3:
                    buttonName = "downButton(Clone)";
                    break;
                case 4:
                    buttonName = "dartButton(Clone)";
                    break;
                case 5:
                    buttonName = "downButton(Clone)";
                    break;
                case 6:
                    buttonName = "dartButton(Clone)";
                    break;
                default:
                    break;
            }
            foreach (GameObject uiButton in UIButtons)
            {
                if (uiButton.name == buttonName)
                {
                    if((guideDartStep&1) == 1)
                        uiButton.GetComponent<Down>().OnMouseDown();
                    else
                        uiButton.GetComponent<Dart>().OnMouseDown();
                    uiButton.SetActive(false);
                    break;
                }
            }
            this.GetComponent<MGGuideDarkLayer>().destoryDarkLayer();
            if (guideDartStep >= 6)
            {
                guideEndMask |= MGGuideManagerState.dart;
                guideDartStep = 1;
                guideLastStepTimer = guideDelayTimer;
            }
            roadblockGCDTimer = 0;
        }
        else if ((guideEndMask & MGGuideManagerState.roadblock) == 0)
        {
            
            Time.timeScale = 1;
            string buttonName = null;
            switch (guideDartStep)
            {
                case 2:
                    buttonName = "roadblockButton(Clone)";
                    break;
                case 3:
                    buttonName = "downButton(Clone)";
                    break;
                case 4:
                    buttonName = "downButton(Clone)";
                    break;
                default:
                    break;
            }
            foreach (GameObject uiButton in UIButtons)
            {
                if (uiButton.name == buttonName)
                {
                    if (guideDartStep == 2)
                    {
                        roadblockHoldLevel = MGSkillRoadblockInfo.skillHoldLevel - 1;
                        roadblockGCDTimer = 0;
                        roleFrontJumpScript.skillsRoadblock();
                    }
                    else
                        uiButton.GetComponent<Down>().OnMouseDown();
                    uiButton.SetActive(false);
                    break;
                }
            }
            this.GetComponent<MGGuideDarkLayer>().destoryDarkLayer();
            
        }
        else if ((guideEndMask & MGGuideManagerState.sprint) == 0)
        {
            Time.timeScale = 1;
            MGNotificationCenter.defaultCenter().postNotification(SkillActEventEnum.sprint, null);
            this.GetComponent<MGGuideDarkLayer>().destoryDarkLayer();
            roadblockGCDTimer = 0;
        }
        else if ((guideEndMask & MGGuideManagerState.beatback) == 0)
        {
            Time.timeScale = 1;
            foreach (GameObject uiButton in UIButtons)
            {
                if (uiButton.name == "beatbackButton(Clone)")
                {
                    MGNotificationCenter.defaultCenter().postNotification(SkillActEventEnum.beatback, null);
                    uiButton.SetActive(false);
                    break;
                }
            }
            this.GetComponent<MGGuideDarkLayer>().destoryDarkLayer();
            guideEndMask |= MGGuideManagerState.beatback;
            guideLastStepTimer = guideDelayTimer;
        }
        else if (isGuideEnd)
        {
            guideEndClickOper();
        }
        guideLabel.SetActive(false);
        skillObjc = null;
        
    }
    public void roleLaterGuideClick()
    {
        if ((guideEndMask & MGGuideManagerState.jump) == 0)
        {
            Time.timeScale = 1;
            MGNotificationCenter.defaultCenter().postNotification(RoleActEventEnum.jumpLatterEventId, null);
            this.GetComponent<MGGuideDarkLayer>().destoryDarkLayer();
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
                this.GetComponent<MGGuideDarkLayer>().destoryDarkLayer();
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
            this.GetComponent<MGGuideDarkLayer>().destoryDarkLayer();
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
            this.GetComponent<MGGuideDarkLayer>().destoryDarkLayer();
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
            this.GetComponent<MGGuideDarkLayer>().destoryDarkLayer();
            foreach (GameObject uiButton in UIButtons)
            {
                if (uiButton.name == "blinkButton(Clone)")
                {
                    uiButton.SetActive(false);
                    break;
                }
            }
            guideEndMask |= MGGuideManagerState.blink;
            roadblockGCDTimer = 0;
        }
        else if ((guideEndMask & MGGuideManagerState.beatback) == 0)
        {
            Time.timeScale = 1;
            roleFrontJumpScript.skillsBeatback();
            roadblockGCDTimer = 0;
            this.GetComponent<MGGuideDarkLayer>().destoryDarkLayer();
        }
        else if ((guideEndMask & MGGuideManagerState.sprint) == 0)
        {
            Time.timeScale = 1;
            roleLaterJumpScript.skillsSprint();
            MGNotificationCenter.defaultCenter().postNotification(RoleActEventEnum.jumpFormerEventId, null);
            MGNotificationCenter.defaultCenter().postNotification(RoleActEventEnum.jumpLatterEventId, null);
            this.GetComponent<MGGuideDarkLayer>().destoryDarkLayer();
            foreach (GameObject uiButton in UIButtons)
            {
                if (uiButton.name == "sprintButton(Clone)")
                {
                    uiButton.SetActive(false);
                    break;
                }
            }
            
            roadblockGCDTimer = 0;
        }
        else if (isGuideEnd)
        {
            guideEndClickOper(); 
        }
        guideLabel.SetActive(false);
        skillObjc = null;
    }
    void guideEndClickOper()
    {
        Time.timeScale = 1;
        isGuideEnd = false;
        MGGlobalDataCenter.defaultCenter().totalGameTime = 60;
        this.GetComponent<MGGuideDarkLayer>().destoryDarkLayer();
        Debug.Log("show all buttons");
        foreach (GameObject uiButton in UIButtons)
        {
            uiButton.SetActive(true);
            uiButton.GetComponent<UIButton>().enabled = true;
        }
        gameTimer.SetActive(true);
        stopButton.SetActive(true);
        roadblockGCDTimer = 0;
        isReStart = true;
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
        if (isReStart)
        {
            isReStart = false;
            Invoke("delaySetGameOver", 0.02f);
        }
        if (isGuideEnd)
        {
            roadblockGCDTimer += Time.deltaTime;

            if (roadblockGCDTimer > 2f && guideMask!=0xFFFF)
            {
                roadblockGCDTimer = 0;
                guideMask = guideEndMask = 0xFFFF;
                //重置位置
                roleFrontJumpScript.initRoleJumpScript();
                roleFront.GetComponent<RoleAnimController>().initRoleAnimController();
                roleLaterJumpScript.initRoleJumpScript();
                roleLater.GetComponent<RoleAnimController>().initRoleAnimController();

                roleLater.transform.position = new Vector3(roleLaterStartPos.x, MGGlobalDataCenter.defaultCenter().roadOrignY, roleLaterStartPos.z);
                roleFront.transform.position = new Vector3(roleFrontStartPos.x, MGGlobalDataCenter.defaultCenter().roadOrignY, roleFrontStartPos.z);
                roleFront.GetComponent<RoleAnimController>().toNomalRun();
                roleFront.GetComponent<RoleAnimController>().animStateToRun();
                roleLater.GetComponent<RoleAnimController>().toNomalRun();
                roleLater.GetComponent<RoleAnimController>().animStateToRun();
                roleFront.GetComponent<RoleAnimController>().isDead = false;

                GameObject.Find("MGSkillEffect").GetComponent<MGSkillEffect>().speedSwitch = 1;
                roleFront.rigidbody2D.velocity = Vector3.zero;
                if (MGGlobalDataCenter.defaultCenter().isLaterRoler)
                {
                    //添加AI脚本
                    roleFront.AddComponent<MGRoleActAIController>();
                    roleFront.AddComponent<MGRoleFrontSkillAIController>();
                }
                else
                {
                    //添加AI脚本
                    roleLater.AddComponent<MGRoleActAIController>();
                    roleLater.AddComponent<MGRoleLaterSkillAIController>();
                }
                

                this.GetComponent<MGGuideDarkLayer>().createAllDarkLayerInPos();

                if(MGGlobalDataCenter.defaultCenter().isFrontRoler)
                    guideLabel.GetComponent<UILabel>().text = "现在来试试吧~请在60秒内不被追上";
                else
				    guideLabel.GetComponent<UILabel>().text = "现在你来试试吧~请在60秒内追上明月~";
                guideLabel.SetActive(true);
                Debug.Log("定格准备重新开始");
                Time.timeScale = 0;
            }
        }
        if (MGGlobalDataCenter.defaultCenter().isSingle && MGGlobalDataCenter.defaultCenter().isLaterRoler)
        {
            roleLaterGuideJump();
            roleLaterGuideSecondJump();
            roleLaterGuideDownToLine();
            roleLaterGuideUp();
            roleLaterGuideBones();
            roleLaterGuideBlink();
            roleLaterGuideBeatback();
            roleLaterGuideSprint();
        }
        else if (MGGlobalDataCenter.defaultCenter().isSingle && MGGlobalDataCenter.defaultCenter().isFrontRoler)
        {
            guideDelayTimer += Time.deltaTime;
            if(guideDelayTimer>guideLastStepTimer + 0.5f)
                roleFrontGuideJump();
            roleFrontGuideSecondJump();
            if (guideDelayTimer > guideLastStepTimer + 1.2f)
                roleFrontGuideDownToLine();
            if (guideDelayTimer > guideLastStepTimer + 0.6f)
                roleFrontGuideUp();
            if (guideDelayTimer > guideLastStepTimer + 0.6f)
                roleFrontGuideDart();
            roleFrontGuideRoadblock();
            if (guideDelayTimer > guideLastStepTimer + 0.6f)
                roleLaterSprint();
            if (guideDelayTimer > guideLastStepTimer + 1f)
                roleFrontGuideBeatback();
            if (guideDelayTimer > guideLastStepTimer + 0.2f)
                roleFrontGuideEnd();
        }
        
	}
    void roleFrontGuideJump()
    {
        //一段跳引导
        if ((guideMask & MGGuideManagerState.jump) == 0)
        {
            if (roleFrontJumpScript.isGround)
            {
                //GameObject.Find("log").GetComponent<UIInput>().label.text= MGFoundtion.getInternIP();
                guideMask |= MGGuideManagerState.jump;
                flag = true;
            }
        }
        if ((guideEndMask & MGGuideManagerState.jump) == 0)
        {
            //Debug.Log("guideJump已经开始");
            if (flag && roleFrontJumpScript.isGround)
            {
                flag = false;
                showButtonAndLabel("upButton(Clone)", "让我们来学习一下明月的基本操作，点击“跳跃”可向上跳跃");
            }
        }
    }
    void roleFrontGuideSecondJump()
    {
        //二段跳引导
        if ((guideEndMask & MGGuideManagerState.jump) != 0 && (guideMask & MGGuideManagerState.secondJump) == 0)
        {
            if (roleFrontJumpScript.isGround == false && roleFront.rigidbody2D.velocity.y < 0f)
            {
                guideMask |= MGGuideManagerState.secondJump;
                flag = true;
            }
        }
        if ((guideEndMask & MGGuideManagerState.secondJump) == 0 && (guideMask & MGGuideManagerState.secondJump) != 0)
        {
            if (flag && roleFrontJumpScript.isGround == false && roleFront.rigidbody2D.velocity.y < 0f)
            {
                //Time.timeScale = 0;
                flag = false;
                showButtonAndLabel("upButton(Clone)","继续点击“跳跃”可二段跳");
            }
        }
    }
    void roleFrontGuideDownToLine()
    {
        if ((guideEndMask & MGGuideManagerState.secondJump) != 0 && (guideMask & MGGuideManagerState.downToLine) == 0)
        {
            if (roleFrontJumpScript.isGround)
            {
                guideMask |= MGGuideManagerState.downToLine;
                flag = true;
            }
        }
        if ((guideEndMask & MGGuideManagerState.downToLine) == 0 && (guideMask & MGGuideManagerState.downToLine) != 0)
        {
            if (flag && roleFrontJumpScript.isGround)
            {
                flag = false;
                showButtonAndLabel("downButton(Clone)", "点击“下翻”可向下翻滚");
            }
        }
    }
    void roleFrontGuideUp()
    {
        if ((guideEndMask & MGGuideManagerState.downToLine) != 0 && (guideMask & MGGuideManagerState.up) == 0)
        {
            if (roleFrontJumpScript.isGround && roleFront.transform.localScale.y < 0)
            {
                guideMask |= MGGuideManagerState.up;
                flag = true;
            }
        }
        if ((guideEndMask & MGGuideManagerState.up) == 0 && (guideMask & MGGuideManagerState.up) != 0)
        {
            if (flag && roleFrontJumpScript.isGround && roleFront.transform.localScale.y < 0)
            {
                flag = false;
                showButtonAndLabel("downButton(Clone)", "处于下翻状态时，按钮会变成“上翻”，此时点击可向上翻滚");
            }
        }
    }
    void roleFrontGuideDart()
    {
        if ((guideEndMask & MGGuideManagerState.up) != 0 && (guideMask & MGGuideManagerState.dart) == 0)
        {
            if (roleFrontJumpScript.isGround && roleFront.transform.localScale.y > 0)
            {
                guideMask |= MGGuideManagerState.dart;
                flag = true;
            }
        }
        if ((guideEndMask & MGGuideManagerState.dart) == 0 && (guideMask & MGGuideManagerState.dart) != 0)
        {
            roadblockGCDTimer += Time.deltaTime;
            if (guideDartStep==1 && roleFrontJumpScript.isGround && roleFront.transform.localScale.y > 0)
            {
                ++guideDartStep;
                showButtonAndLabel("dartButton(Clone)", "让我们试试技能吧，点击“飞镖”可向后发射飞镖");
                roadblockGCDTimer = 0;
            }
            else if (roadblockGCDTimer > 0.02f && guideDartStep == 2 && roleFrontJumpScript.isGround && roleFront.transform.localScale.y > 0)
            {
                ++guideDartStep;
                showButtonAndLabel("downButton(Clone)", "教您一个小技巧，此时点击“下翻”可向下翻滚");
                roadblockGCDTimer = 0;
            }
            else if (roadblockGCDTimer > 0.22f && guideDartStep == 3 && roleFrontJumpScript.isGround && roleFront.transform.localScale.y < 0)
            {
                ++guideDartStep;
                showButtonAndLabel("dartButton(Clone)", "处于下方时再发送一个飞镖吧~");
                roadblockGCDTimer = 0;
            }
            else if (roadblockGCDTimer > 0.02f && guideDartStep == 4 && roleFrontJumpScript.isGround && roleFront.transform.localScale.y < 0)
            {
                ++guideDartStep;
                showButtonAndLabel("downButton(Clone)", "让我们再翻上来发射一个飞镖吧");
                roadblockGCDTimer = 0;
            }
            else if (roadblockGCDTimer > 0.22f && guideDartStep == 5 && roleFrontJumpScript.isGround && roleFront.transform.localScale.y > 0)
            {
                ++guideDartStep;
                showButtonAndLabel("dartButton(Clone)", "通过控制明月的位置，可以释放不同的飞镖组合");
                roadblockGCDTimer = 0;
            }
        }
    }
    void roleFrontGuideRoadblock()
    {
        if ((guideEndMask & MGGuideManagerState.dart) != 0 && (guideMask & MGGuideManagerState.roadblock) == 0)
        {
            if (roleFrontJumpScript.isGround && guideDelayTimer > guideLastStepTimer + 1.0f)
            {
                guideMask |= MGGuideManagerState.roadblock;
                guideDartStep = 1;
            }
        }
        if ((guideEndMask & MGGuideManagerState.roadblock) == 0 && (guideMask & MGGuideManagerState.roadblock) != 0)
        {
            if (roadblockHoldLevel > 0)
            {
                roadblockGCDTimer += Time.deltaTime;
                if (roadblockGCDTimer > MGSkillRoadblockInfo.skillGCD)
                {
                    roadblockGCDTimer = 0;
                    --roadblockHoldLevel;
                    roleFrontJumpScript.skillsRoadblock();
                    if (roadblockHoldLevel == 0)
                    {
                        guideEndMask |= MGGuideManagerState.roadblock;
                        guideLastStepTimer = guideDelayTimer;
                    }
                }
            }
            guideStepTimer += Time.deltaTime;
            if (guideDartStep == 1 && roleFrontJumpScript.isGround && roleFront.transform.localScale.y > 0)
            {
                ++guideDartStep;
                showButtonAndLabel("roadblockButton(Clone)", "来试试第二个技能“地刺”吧~");
                guideStepTimer = 0;
            }
            else if (guideStepTimer > 0.1f && guideDartStep == 2 && roleFrontJumpScript.isGround && roleFront.transform.localScale.y > 0)
            {
                ++guideDartStep;
                showButtonAndLabel("downButton(Clone)", "此时点击“下翻”可以改变地刺出现的方向");
                guideStepTimer = 0;
            }
            else if (guideStepTimer > 0.35f && guideDartStep == 3 && roleFrontJumpScript.isGround && roleFront.transform.localScale.y < 0)
            {
                ++guideDartStep;
                showButtonAndLabel("downButton(Clone)", "让我们再翻上来试试~");
                guideStepTimer = 0;
            }
        }
    }

    void roleLaterSprint()
    {
        if ((guideEndMask & MGGuideManagerState.roadblock) != 0 && (guideMask & MGGuideManagerState.sprint) == 0)
        {
            guideMask |= MGGuideManagerState.sprint;
            guideLabel.GetComponent<UILabel>().text = "天涯要放大招啦！！！";
            guideLabel.SetActive(true);
            this.GetComponent<MGGuideDarkLayer>().createAllDarkLayerInPos();
            Time.timeScale = 0;
        }
        if ((guideEndMask & MGGuideManagerState.sprint) == 0 && (guideMask & MGGuideManagerState.sprint) != 0)
        {
            roadblockGCDTimer += Time.deltaTime;
            if (roadblockGCDTimer >= MGSkillSprintInfo.durationTime + 0.5f)
            {
                guideEndMask |= MGGuideManagerState.sprint;
                guideLastStepTimer = guideDelayTimer;
            }
        }
    }

    void roleFrontGuideBeatback()
    {
        if ((guideEndMask & MGGuideManagerState.sprint) != 0 && (guideMask & MGGuideManagerState.beatback) == 0)
        {
            if (roleFront.transform.localScale.y > 0 && roleFrontJumpScript.isGround)
            {
                guideMask |= MGGuideManagerState.beatback;
                flag = true;
            }
        }
        if ((guideEndMask & MGGuideManagerState.beatback) == 0 && (guideMask & MGGuideManagerState.beatback) != 0)
        {
            if (flag && roleFront.transform.localScale.y > 0 && roleFrontJumpScript.isGround)
            {
                flag = false;
                showButtonAndLabel("beatbackButton(Clone)", "您已经学会了基本操作\r\n让我们释放大招\r\n“沧海月明”\r\n给与对方致命一击吧~");
            }
        }
    }
    void roleFrontGuideEnd()
    {
        if (!isGuideEnd && (guideEndMask & MGGuideManagerState.beatback) != 0)
        {
            isGuideEnd = true;
            roadblockGCDTimer = 0;
        }
    }

    void roleLaterGuideJump()
    {
        //一段跳引导
        if ((guideMask & MGGuideManagerState.jump) == 0)
        {
            if (roleFrontJumpScript.isGround)
            {
                //GameObject.Find("log").GetComponent<UIInput>().label.text= MGFoundtion.getInternIP();
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
                showButtonAndLabel("upButton(Clone)", "点击“跳跃”可躲避飞镖");
            }
        }
    }
    void roleLaterGuideSecondJump()
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
                showButtonAndLabel("upButton(Clone)", "试试二段跳吧，点击“跳跃”来躲避第一个路障");
            }
            if (flag == true && roleLater.rigidbody2D.velocity.y < -2f && Time.timeScale != 0)
            {
                Debug.Log("second jump");
                showButtonAndLabel("upButton(Clone)", "点击“跳跃”进行二段跳");
            }
        }
    }
    void roleLaterGuideDownToLine()
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
                showButtonAndLabel("downButton(Clone)", "明月放了好多技能，点击“下翻”来躲避障碍");
            }
        }
    }
    void roleLaterGuideUp()
    {
        
    }
    void roleLaterGuideBones()
    {
        if ((guideEndMask & MGGuideManagerState.downToLine) != 0 && (guideMask & MGGuideManagerState.bones) == 0)
        {
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
                showButtonAndLabel("bonesButton(Clone)", "基础操作已经学会啦~\r\n我们来试试技能吧！\r\n点击“金钟罩”可弹开障碍");
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
    void roleLaterGuideBlink()
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
                showButtonAndLabel("blinkButton(Clone)", "点击“闪烁”可躲避障碍哦");
            }
        }
    }
    void roleLaterGuideBeatback()
    {
        if ((guideEndMask & MGGuideManagerState.blink) != 0 && (guideMask & MGGuideManagerState.beatback) == 0)
        {
            roadblockGCDTimer += Time.deltaTime;
            if (roadblockGCDTimer > 2f)
            {
                guideMask |= MGGuideManagerState.beatback;
                guideLabel.GetComponent<UILabel>().text = "明月要放大招啦！！！";
                guideLabel.SetActive(true);
                this.GetComponent<MGGuideDarkLayer>().createAllDarkLayerInPos();
                Time.timeScale = 0;
                roadblockGCDTimer = 0;
            }
        }
        if ((guideEndMask & MGGuideManagerState.beatback) == 0 && (guideMask & MGGuideManagerState.beatback) != 0)
        {
            roadblockGCDTimer += Time.deltaTime;
            if (roadblockGCDTimer >= MGSkillBeatbackInfo.durationTime+0.5f)
            {
                guideEndMask |= MGGuideManagerState.beatback;
                guideLabel.SetActive(false);
            }
        }
    }
    void roleLaterGuideSprint()
    {
        if ((guideEndMask & MGGuideManagerState.beatback) != 0 && (guideMask & MGGuideManagerState.sprint) == 0)
        {
            guideMask |= MGGuideManagerState.sprint;
            showButtonAndLabel("sprintButton(Clone)", "我们也来试试\r\n天涯的大招吧~");
        }
        if ((guideEndMask & MGGuideManagerState.sprint) == 0 && (guideMask & MGGuideManagerState.sprint) != 0)
        {
            roadblockGCDTimer += Time.deltaTime;
            if (roadblockGCDTimer >= MGSkillSprintInfo.durationTime + 0.5f)
            {
                guideEndMask |= MGGuideManagerState.sprint;
                isGuideEnd = true;
                roadblockGCDTimer = 0;
            }
        }
    }
}
