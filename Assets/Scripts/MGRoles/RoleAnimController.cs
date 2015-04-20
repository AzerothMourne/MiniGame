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


	// Use this for initialization
	void Start () {
		isPressJumpButton = false;
		isFallDown = false;
		isSecondJump = false;
		
		jumpAnim = this.GetComponent<Animator> ();
		player = this.GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		//检测角色的动作
		//动作切换
		if (isSecondJump == false) {
			if (player.velocity.y < -0.01f) {
				//print ("velocity.y : " + GameObject.Find ("role1").GetComponent<Rigidbody2D> ().velocity.y);
				isPressJumpButton = false;
				isFallDown = true;
				jumpAnim.SetBool ("jumpUP", isPressJumpButton);
				jumpAnim.SetBool ("fallDown", isFallDown);
				//print ("*****isPressJumpButton : " + isPressJumpButton);
				//print ("*****isFallDown : " + isFallDown);
			}
		} else if (isSecondJump == true && player.velocity.y < -10f ) {	
			isSecondJump = false;
			isFallDown = true;
			//print ("*****isSecondJump : " + isSecondJump);
			jumpAnim.SetBool ("fallDown", isFallDown);
			jumpAnim.SetBool ("secondJump", isSecondJump);
		}
		
		if (this.GetComponent<Jump>().isGround == true && isFallDown == true) {
			isFallDown = false;	
			jumpAnim.SetBool ("fallDown", isFallDown);
			//print ("*****isground isFallDown : "+isFallDown);
		}
	}
}
