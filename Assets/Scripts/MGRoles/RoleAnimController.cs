﻿using UnityEngine;
using System.Collections;
public static class RoleButtonEvent
{
    public static string upFormerEventId = "RoleButtonEvent_up";
    public static string upLatterEventId = "1RoleButtonEvent_up";
    public static string downFormerEventId = "RoleButtonEvent_down";
    public static string downLatterEventId = "1RoleButtonEvent_down";
}
//动画状态机需要注意的事
/// <summary>
/// 比如需要从A到B状态 需要把B状态的出口全关闭，防止到了B状态因为有的出口未关闭而瞬间跳到别的状态
/// </summary>
public class RoleAnimController : MonoBehaviour {

	//控制角色动作
	private Animator jumpAnim;
    private Jump jumpSprict;
    public bool isSprint;//判断冲刺动画 在MGSkillEffect里面用
	public bool isFallDown; //是否正常下落
	int countJumpFrame;//用来计算跳跃
	Rigidbody2D player;
	public bool isSecondJump,isFirstJump;//是否采用二段跳翻滚的动作
	public bool isRoll,downOrUp,isChangeDownOrUp;//是否切换到上下翻滚的动画
    public bool isPressDown,isPressDownToGround;//在空中按了下
	float rollTimer,rollDuration;
    private float downSpeed;
    GameObject road;
	
	// Use this for initialization
	void Start () {
        isFirstJump = false;
        isPressDown = false;
		isFallDown = true;
        isPressDownToGround = false;
		isSecondJump = false;
        isRoll = false;

        rollTimer = 0;
        rollDuration = 0.2f;
        road = GameObject.Find("road");

        jumpSprict = this.GetComponent<Jump>();
		jumpAnim = this.GetComponent<Animator> ();
		player = this.GetComponent<Rigidbody2D> ();
        if (this.gameObject.name == "role")
        {
            MGNotificationCenter.defaultCenter().addObserver(this, upButtonClick, RoleButtonEvent.upFormerEventId);
            MGNotificationCenter.defaultCenter().addObserver(this, downButtonClick, RoleButtonEvent.downFormerEventId);
        }
        //后面的角色动作
        else if (this.gameObject.name == "role1")
        {
            MGNotificationCenter.defaultCenter().addObserver(this, upButtonClick, RoleButtonEvent.upLatterEventId);
            MGNotificationCenter.defaultCenter().addObserver(this, downButtonClick, RoleButtonEvent.downLatterEventId);
        }

	}
    void animStateToRun()
    {
        jumpAnim.SetBool("FallDownToRun", true);
        jumpAnim.SetBool("RollToRun", true);
    }
    void animStateToFirstJump()
    {
        jumpAnim.SetBool("RunToFirstJump", true);
    }
    void animStateToFallDown()
    {
        jumpAnim.SetBool("FirstJumpToFallDown", true);
        jumpAnim.SetBool("RollToFallDown", true);
    }
    void animStateToRoll()
    {
        jumpAnim.SetBool("FirstJumpToSecondJump", true);
        jumpAnim.SetBool("RunToRoll", true);
        jumpAnim.SetBool("FallDownToRoll", true);
    }
    void setAllAnimStateToFalse()
    {
        jumpAnim.SetBool("RunToFirstJump", false);
        jumpAnim.SetBool("FirstJumpToSecondJump", false);
        jumpAnim.SetBool("FallDownToRun", false);
        jumpAnim.SetBool("RollToFallDown", false);
        jumpAnim.SetBool("FirstJumpToFallDown", false);
        jumpAnim.SetBool("RunToRoll", false);
        jumpAnim.SetBool("FallDownToRoll", false);
        jumpAnim.SetBool("RollToRun", false);
    }
    void toNomalRun()
    {
        rigidbody2D.gravityScale = transform.localScale.y > 0 ? 5 : 0;
        rigidbody2D.velocity = Vector3.zero;
        collider2D.isTrigger = false;
        isRoll = false;
        isPressDown = false;
        isFallDown = false;
        isFirstJump = false;
        isSecondJump = false;
        isPressDownToGround = false;
        setAllAnimStateToFalse();
    }
    public void upButtonClick(MGNotification notification)
    {
        if (isPressDown && !isPressDownToGround) return;
        Debug.Log(this.gameObject.name + " upButtonClick");
        if (downOrUp == true)//上翻
        {
            Debug.Log("上翻");
            if (isRoll && downOrUp == true) isChangeDownOrUp = true;
            isRoll = true;
            downOrUp = false;
            rigidbody2D.gravityScale = 0f;
            collider2D.isTrigger = true;
            setAllAnimStateToFalse();
            animStateToRoll();
            return;
        }
        if (isPressDown || isRoll) return;
        Debug.Log(this.gameObject.name + " jumpButtonClick");
        if (isFirstJump == false)//一段跳
        {
            isFirstJump = true;
            setAllAnimStateToFalse();
            animStateToFirstJump();
        }
        else if(isSecondJump == false)//二段跳
        {
            isSecondJump = true;
            setAllAnimStateToFalse();
            animStateToRoll();
        }
    }
    public void downButtonClick(MGNotification notification)
    {
        if (!isRoll && !isPressDown && transform.lossyScale.y < 0) return;
        Debug.Log(this.gameObject.name + " downButtonClick");
        collider2D.isTrigger = true;
        if (isFirstJump)//在空中
        {
            Debug.Log("在空中");
            isPressDown = true;
            downOrUp = true;
            rigidbody2D.gravityScale = 10f;
            rigidbody2D.velocity = new Vector3(0,-10,0);
            setAllAnimStateToFalse();
            animStateToRoll();
        }
        else//在绳子上
        {
            Debug.Log("在绳子上");
            if (isRoll && !downOrUp) isChangeDownOrUp = true;
            isRoll = true;
            downOrUp = true;
            rigidbody2D.gravityScale = 0f;
            setAllAnimStateToFalse();
            animStateToRoll();
        }
    }
	// Update is called once per frame
	void Update () {
		//检测角色的动作
		//动作切换
        //通过速度判断是否下落,下落有2中情况，正常下落和加速下落 对应不同动画,前提条件是必须在空中，否则isRollBack表示上下翻滚。
        if (isPressDown && isFirstJump)//加速下落，切换到翻滚动作
        {
            Debug.Log("切换加速下落动画");
            rigidbody2D.gravityScale = 10f;//保证加速下落过程中的重力
            //判断翻滚是否结束
            if (MGGlobalDataCenter.defaultCenter().roadOrignY - transform.position.y >= 0)
            {
                Debug.Log("pressDown过程中接触地面");
                rigidbody2D.gravityScale = 0f;
                rigidbody2D.velocity = Vector3.zero;
                transform.Translate(Vector3.down * this.GetComponent<SpriteRenderer>().sprite.bounds.size.y * Time.deltaTime / rollDuration);
                isPressDownToGround = true;
                isRoll = true;
                rollTimer += Time.deltaTime;
            }
            if (isPressDownToGround && isChangeDownOrUp)//如果接触地面 且切换了方向 则切换到上下翻滚判断逻辑
            {
                Debug.Log("接触地面后切换方向");
                isFirstJump = false;
                isPressDownToGround = false;
                rigidbody2D.gravityScale = 0.0f;
                rigidbody2D.velocity = Vector3.zero;
            }
            else if (transform.position.y < MGGlobalDataCenter.defaultCenter().roadOrignY && Mathf.Abs(MGGlobalDataCenter.defaultCenter().roadOrignY - transform.position.y) > this.GetComponent<SpriteRenderer>().sprite.bounds.size.y )
            {
                Debug.Log("MGGlobalDataCenter.defaultCenter().roadOrignY" + MGGlobalDataCenter.defaultCenter().roadOrignY);
                Debug.Log("transform.position.y" + transform.position.y);
                Debug.Log("this.GetComponent<SpriteRenderer>().sprite.bounds.size.y" + this.GetComponent<SpriteRenderer>().sprite.bounds.size.y);
                Debug.Log("加速下翻动作完成");
                toNomalRun();
                changeRollBackState();
            }
        }
        if (!isPressDown && isFirstJump && player.velocity.y <= -0.1f)
        {
            if(isSecondJump == false && isFirstJump == true)//正常下落
            {
                Debug.Log("一段跳后切换正常下落动画");
                isFallDown = true;
                setAllAnimStateToFalse();
                animStateToFallDown();
            }
            else if (isSecondJump == true && player.velocity.y < -8f)
            {
                Debug.Log("二段跳后切换正常下落动画");
                isFallDown = true;
                setAllAnimStateToFalse();
                animStateToFallDown();
            }
        }
        //正常下落后回到地面需要切换回正常动画
        if (this.GetComponent<Jump>().isGround == true && isFirstJump && !isPressDown)
        {
            Debug.Log("切换回正常动画");
            toNomalRun();
            animStateToRun();
        }
        
        //说明正在上下翻滚
        if (isRoll == true && !isFirstJump)
        {
            Debug.Log("正在上下翻滚：" + rollTimer);
            //判断翻滚是否结束
            float dis = this.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
            rollTimer += Time.deltaTime;
            if (isChangeDownOrUp)//是否在翻滚过程中切换了方向
            {
                isChangeDownOrUp = false;
                rollTimer = rollDuration - rollTimer;
            }
            if (rollTimer >= rollDuration)
            {
                changeRollBackState();
            }
            else
            {
                if (downOrUp)//正在下落
                {
                    transform.Translate(Vector3.down * dis * Time.deltaTime / rollDuration);
                }
                else//正在上翻
                {
                    transform.Translate(Vector3.up * dis * Time.deltaTime / rollDuration);
                }
            }
        }
	}
    public void changeRollBackState()
    {
        Debug.Log("翻滚结束切换回正常动画");
        rollTimer = 0f;
        float flag = downOrUp?1:-1;
        transform.localScale = new Vector3(1, -1 * flag, 1);
        transform.position = new Vector3(transform.position.x, MGGlobalDataCenter.defaultCenter().roadOrignY, transform.position.z);
        toNomalRun();
        animStateToRun();
    }
}
