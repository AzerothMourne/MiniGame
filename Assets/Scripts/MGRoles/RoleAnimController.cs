using UnityEngine;
using System.Collections;

public class RoleAnimController : MonoBehaviour {

	//控制角色动作
	public bool isPressJumpButton; //是否跳跃
	private Animator jumpAnim; 
	public bool isFallDown; //是否下落
	int countJumpFrame;//用来计算跳跃
	Rigidbody2D player;
	public bool isSecondJump;//是否采用二段跳翻滚的动作
	public bool isRollBack;//上下线时是否翻滚
	float timeSum;
	float timeRoll;
	float speed;
	
	// Use this for initialization
	void Start () {
		isPressJumpButton = false;
		isFallDown = false;
		isSecondJump = false;
		isRollBack = false;
		timeSum = 0f;
		timeRoll = 0.2f;
		speed = 5.0f;
		
		jumpAnim = this.GetComponent<Animator> ();
		player = this.GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		//检测角色的动作
		//动作切换
		//如果是一段跳，则只采用起跳和下落的动作
		if (isSecondJump == false && player.velocity.y < -0.01f && isRollBack == false) {
				isPressJumpButton = false;
				isFallDown = true;
				jumpAnim.SetBool ("jumpUP", isPressJumpButton);
				jumpAnim.SetBool ("fallDown", isFallDown);
		}
		//如果是二段跳，则采用起跳后翻滚再下落的动作
		else if (isSecondJump == true && player.velocity.y < -10f ) {	
			isSecondJump = false;
			isFallDown = true;
			isRollBack = false;
			jumpAnim.SetBool ("fallDown", isFallDown);
			jumpAnim.SetBool ("secondJump", isSecondJump);
			jumpAnim.SetBool ("rollBack", isRollBack);
		}

		//如果起跳后回到地面，则置为跑步动作
		if (this.GetComponent<Jump>().isGround == true && isFallDown == true) {
			isFallDown = false;	
			jumpAnim.SetBool ("fallDown", isFallDown);
		}

		//向下翻滚动作，给角色一个向下的位移1s，再给角色向上的位移，直到角色碰到地面
		if (isRollBack == true && isSecondJump == false) {

			//先给角色向下的位移，时长为timeRoll
			if (timeSum < timeRoll) {
				if(this.GetComponent<Jump>().isDown == 1) {
					transform.localScale = new Vector3 (1, -1, 1);
					transform.Translate (-Vector3.up * speed * Time.deltaTime);
				}
				else if(this.GetComponent<Jump>().isDown == 0) {
					transform.localScale = new Vector3 (1, 1, 1);
					transform.Translate (Vector3.up * speed *2 * Time.deltaTime);
				}
				this.GetComponent<Jump> ().isGround = false;
				timeSum += Time.deltaTime;
			}

			//再给角色向上的位移，在没有触碰到地面之前
			if (timeSum > timeRoll && this.GetComponent<Jump> ().isGround == false) {
				timeSum += Time.deltaTime;
				//翻滚到线上之后有重力，所以不用再加向下的位移了
				if(this.GetComponent<Jump>().isDown == 1) {
					transform.Translate (Vector3.up * speed * Time.deltaTime);
				}
			} 

			//最后当碰撞到地面后，则将角色的动作置为跑步动作
			if (timeSum > timeRoll && this.GetComponent<Jump> ().isGround == true) {
				isRollBack = false;
				jumpAnim.SetBool ("rollBack", isRollBack);
				timeSum = 0f;
			}
		}
	}
}
