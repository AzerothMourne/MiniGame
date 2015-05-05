using UnityEngine;
using System.Collections;

public static class MGGuideManagerState
{
    public static int jump = 1 << 0;
    
}
public class MGGuideManager : MonoBehaviour {
    private GameObject roleLater, roleFront;
    private Jump roleLaterJumpScript, roleFrontJumpScript;
    private int guideMask;
    private MGSkillsBase guideJump_Dart;
    private GameObject[] UIButtons;
    private bool isHiddenButtons;
	void Start () {
        roleLater = MGGlobalDataCenter.defaultCenter().roleLater;
        roleFront = MGGlobalDataCenter.defaultCenter().role;
        roleLaterJumpScript = roleLater.GetComponent<Jump>();
        roleFrontJumpScript = roleFront.GetComponent<Jump>();
        guideMask = 0;
        isHiddenButtons = false;
        UIButtons = null;
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
            }
        }
        if ((guideMask & MGGuideManagerState.jump) == 0)
        {
            guideJump();
        }
        else
        {
            //Debug.Log("guideJump已经开始");
            if (guideJump_Dart!=null && guideJump_Dart.transform.position.x - roleLater.transform.position.x <= 3f)
            {
                //Time.timeScale = 0;
                guideJump_Dart = null;
                MGNotificationCenter.defaultCenter().postNotification(RoleActEventEnum.jumpLatterEventId, null);
            }
        }
	}
    void guideJump()
    {
        if (roleFrontJumpScript.isGround)
        {
            guideMask |= MGGuideManagerState.jump;
            guideJump_Dart = roleFrontJumpScript.skillsDart();
        }
    }
    void guideSecondJump()
    {

    }
    void guideDownToLine()
    {

    }
    void guideUp()
    {

    }
    void guideBones()
    {

    }
    void guideBlink()
    {

    }
    void guideBeatback()
    {

    }
    void guideSprint()
    {

    }
}
