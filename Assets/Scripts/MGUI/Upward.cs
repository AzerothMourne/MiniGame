using UnityEngine;
using System.Collections;

public class Upward : MonoBehaviour {
	private float timer;
    private float cameraMoveSpeed;
	private bool isClick,isMoveCamera;
    private Vector3 originVec3;
/*	private Animator jumpAnim; 
	public bool isPressJumpButton;
	public bool isFallDown;
	int countJumpFrame;
	Rigidbody2D player;
*/
	// Use this for initialization
	void Start () {
        cameraMoveSpeed = 8f;
		timer = 0.0f;
		isClick = false;
        isMoveCamera = false;
        originVec3 = transform.localScale;
		//isPressJumpButton = false;
/*		isFallDown = false;
		countJumpFrame = 0;
*/
	}


/*	void Awake() {
		jumpAnim = GameObject.Find("role1").GetComponent<Animator> ();
		player = GameObject.Find ("role1").GetComponent<Rigidbody2D> ();
	}

*/
	void Update(){
        
		if (isClick) {
			if(timer<=0.15f){
				transform.localScale=new Vector3((transform.localScale.x-0.01f),(transform.localScale.y-0.01f),(transform.localScale.z-0.01f));
				timer+=Time.deltaTime;
			}
			else if(timer<=0.3f) {
				transform.localScale=new Vector3((transform.localScale.x+0.01f),(transform.localScale.y+0.01f),(transform.localScale.z+0.01f));
				timer+=Time.deltaTime;
			}
			else if(timer>0.3f){
				isClick=false;timer=0.0f;
                transform.localScale = originVec3;
			}
		}
        if (isMoveCamera)
        {
            Vector3 pos = Camera.main.transform.position;
            pos.y +=cameraMoveSpeed*Time.deltaTime;
            Camera.main.transform.position = pos;
            if (pos.y >= 0f)
            {
                pos.y = 0f;
                Camera.main.transform.position = pos;
                isMoveCamera = false;
            }
        }
		//动作切换

/*		jumpAnim.SetBool ("jumpUP", isPressJumpButton);
		jumpAnim.SetBool ("fallDown", isFallDown);

		if (player.velocity.y < -0.01f) {
			print ("velocity.y : " + GameObject.Find ("role1").GetComponent<Rigidbody2D> ().velocity.y);
			isPressJumpButton = false;
			isFallDown = true;
			print ("*****isPressJumpButton : "+isPressJumpButton);
			print ("*****isFallDown : "+isFallDown);
		}

		if (GameObject.Find ("role1").GetComponent<Jump> ().isGround == true) {
			isFallDown = false;	
			//print ("*****isground isFallDown : "+isFallDown);
		}
*/
			
	}
	
	// Update is called once per frame
	public void OnMouseDown () {
		isClick = true;

//		isPressJumpButton = true;
//		print (".....OnMouseDown isPressJumpButton : " + isPressJumpButton);

		//将向上的按钮变为跳的按钮
        isMoveCamera = true;

		this.GetComponent<UISprite>().spriteName = "jump";
        this.GetComponent<UIButton>().normalSprite = "jump";
        if (MGGlobalDataCenter.defaultCenter().isHost == true)
            MGNotificationCenter.defaultCenter().postNotification("jump", null);
        else
            MGNotificationCenter.defaultCenter().postNotification("1jump", null);
	}
}
	