using UnityEngine;
using System.Collections;
public static class RoleButtonEvent
{
    public static string upFormerEventId = "RoleButtonEvent_up";
    public static string upLatterEventId = "1RoleButtonEvent_up";
    public static string downFormerEventId = "RoleButtonEvent_down";
    public static string downLatterEventId = "1RoleButtonEvent_down";
}
public class RoleAnimController : MonoBehaviour {

	//控制角色动作
	private Animator jumpAnim;
    public bool isSprint;//判断冲刺动画 在MGSkillEffect里面用
	public bool isFallDown; //是否正常下落
	int countJumpFrame;//用来计算跳跃
	Rigidbody2D player;
	public bool isSecondJump,isFirstJump;//是否采用二段跳翻滚的动作
	public bool isRoll;//是否切换到上下翻滚的动画
    public bool isPressDown;//在空中按了下
	float timeSum;
	float timeRoll;
	float speed;
    private float downSpeed,rollBackOrignY,distance;
    GameObject road;
	
	// Use this for initialization
	void Start () {
        isFirstJump = false;
        isPressDown = false;
		isFallDown = true;
		isSecondJump = false;
        isRoll = false;

		timeSum = 0f;
		timeRoll = 0.2f;
		speed = 5.0f;
        downSpeed = 5.0f;
        road = GameObject.Find("road");
        rollBackOrignY = -10000;

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
    public void upButtonClick(MGNotification notification)
    {
        Debug.Log(this.gameObject.name + " upButtonClick");
        if (isRoll) return;
        if (isFirstJump == false)//一段跳
        {
            isFirstJump = true;
            jumpAnim.SetBool("FallDownToRun", false);
            jumpAnim.SetBool("RunToFirstJump", true);
        }
        else if(isSecondJump == false)//二段跳
        {
            isSecondJump = true;
            jumpAnim.SetBool("RunToFirstJump", false);
            jumpAnim.SetBool("FirstJumpToFallDown", false);
            jumpAnim.SetBool("FirstJumpToSecondJump", true);
            jumpAnim.SetBool("FallDownToRoll", true);
        }
    }
    public void downButtonClick(MGNotification notification)
    {
        Debug.Log(this.gameObject.name + " downButtonClick");
        if (isFirstJump)//在空中
        {
            isPressDown = true;
        }
        else//在绳子上
        {
            isRoll = true;
            jumpAnim.SetBool("FallDownToRun", false);
            jumpAnim.SetBool("RunToRoll", true);
        }
    }
	// Update is called once per frame
	void Update () {
		//检测角色的动作
		//动作切换
        //通过速度判断是否下落,下落有2中情况，正常下落和加速下落 对应不同动画,前提条件是必须在空中，否则isRollBack表示上下翻滚。
        if (this.GetComponent<Jump>().isGround == false && player.velocity.y < -0.01f)
        {
            if (isPressDown)//加速下落，切换到翻滚动作
            {
                Debug.Log("切换加速下落动画"); 
            }
            else if(isSecondJump == false && isFirstJump == true)//正常下落
            {
                Debug.Log("一段跳后切换正常下落动画");
                isFallDown = true;
                jumpAnim.SetBool("RunToFirstJump", false);
                jumpAnim.SetBool("FirstJumpToFallDown", true);
            }
            else if (isSecondJump == true && player.velocity.y < -8f)
            {
                Debug.Log("二段跳后切换正常下落动画");
                isFallDown = true;
                jumpAnim.SetBool("FirstJumpToSecondJump", false);
                jumpAnim.SetBool("FallDownToRoll", false);
                jumpAnim.SetBool("RunToRoll", false);
                jumpAnim.SetBool("RollToFallDown", true);
            }
        }
        //正常下落后回到地面需要切换回正常动画
        if (this.GetComponent<Jump>().isGround == true && isFallDown == true && !isPressDown)
        {
            Debug.Log("切换回正常动画");
            isFallDown = false;
            isFirstJump = false;
            isSecondJump = false;
            jumpAnim.SetBool("RollToFallDown", false);
            jumpAnim.SetBool("FirstJumpToFallDown", false);
            jumpAnim.SetBool("FallDownToRun", true);
        }
        
        //说明正在上下翻滚
        if (isRoll == true && this.GetComponent<Jump>().isGround == true)
        {
            //记录上下翻滚的初始Y值
            if (rollBackOrignY == -10000)
            {
                rollBackOrignY = transform.position.y;
                distance = this.GetComponent<SpriteRenderer>().sprite.bounds.size.y + 0.1f;
            }
            //判断翻滚是否结束
            if (Mathf.Abs(rollBackOrignY - transform.position.y) > distance)
            {
                changeRollBackState();
            }
            else
            {
                if (transform.localScale.y > 0)//正在下落
                {
                    transform.Translate(Vector3.down * downSpeed * Time.deltaTime);
                }
                else//正在上翻
                {
                    transform.Translate(Vector3.up * downSpeed * Time.deltaTime);
                }
            }
        }
	}
    public void changeRollBackState()
    {
        rollBackOrignY = -10000;
        float flag = transform.localScale.y;
        transform.localScale = new Vector3(1, -1 * flag, 1);
        transform.position = new Vector3(transform.position.x, transform.position.y + flag * this.GetComponent<SpriteRenderer>().sprite.bounds.size.y, transform.position.z);
        rigidbody2D.gravityScale = flag > 0 ? 0 : 5;
        rigidbody2D.velocity = Vector3.zero;
        collider2D.isTrigger = false;
        isRoll = false;
        jumpAnim.SetBool("RunToRoll", false);
        jumpAnim.SetBool("RollToRun", true);
    }
}
