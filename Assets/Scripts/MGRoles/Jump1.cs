using UnityEngine;
using System.Collections;
using LitJson;

public class Jump1 : MonoBehaviour {

	public float forceMove ;
	private bool isGround = false;
	public float jumpVelocity ;
	public float jumpSecond ;
	public float jumpCount ;
	private float timer=0;
	public int isJump,isDown;
    public MGskillDrat drat;
	public UIInput log;
	public int isReceiveFlag;
	// Use this for initialization
	void Start () {
		timer = 0;
		isJump = 0;
        isDown = 0;
		isReceiveFlag = 0;
		log.label.text = "";
		//forceMove = 50;
		//jumpVelocity = 25;
		//jumpSecond = 15;
        MGNotificationCenter.defaultCenter().addObserver(this, useSkillsDart, "1useSkillsDart");
        MGNotificationCenter.defaultCenter().addObserver(this, firstJump, "1firstJump");
        MGNotificationCenter.defaultCenter().addObserver(this, secondJump, "1secondJump");
        MGNotificationCenter.defaultCenter().addObserver(this, downToLine, "1downToLine");
	}
    public void useSkillsDart(MGNotification notification)
    {
        GameObject role1 = this.gameObject;
        drat.createSkillSprite(new Vector3(role1.transform.position.x, role1.transform.position.y + (isDown==0?1:-1)*role1.renderer.bounds.size.y / 2, role1.transform.position.z));

		if (notification.objc == null) {
			MGMsgModel msgModel=new MGMsgModel();
			msgModel.eventId="1useSkillsDart";
			msgModel.timestamp=MGGlobalDataCenter.timestamp();
			MGNetWorking.sendMessageToPeer (JsonMapper.ToJson(msgModel));
		}
    }
    public void firstJump(MGNotification notification)
    {
        
            isJump = 0;
            Vector2 velocity = rigidbody2D.velocity;
            velocity.y = 0;
            print("1:" + velocity.y);
            velocity.y = jumpVelocity;
            rigidbody2D.velocity = velocity;
            print("1:" + rigidbody2D.velocity.y);
            jumpCount = 1;
		if (notification.objc == null) {
			//log.label.text+="jump send:" + MGGlobalDataCenter.timestamp ()+"\n";

			MGMsgModel msgModel=new MGMsgModel();
			msgModel.eventId="1firstJump";
			msgModel.timestamp=MGGlobalDataCenter.timestamp();
			MGNetWorking.sendMessageToPeer (JsonMapper.ToJson(msgModel));
			//P2PBinding.sendMessageToPeer ("1firstJump ");
		} else {
			//log.label.text+="jump receive:" + MGGlobalDataCenter.timestamp ()+"\n";
			//isReceiveFlag=1;
			long time1=MGGlobalDataCenter.timestamp();
			long time2=((MGMsgModel)notification.objc).timestamp;
			print(time1+";"+time2);
			log.label.text+=(Mathf.Abs(time1-time2)).ToString()+"\r\n";
		}

    }
    public void secondJump(MGNotification notification)
    {
        if (!isGround && (Input.GetKeyDown(KeyCode.Space) || isJump == 1) && jumpCount == 1)
        {
            isJump = 0;
            Vector2 velocity = rigidbody2D.velocity;
            if (velocity.y < -1.0f) velocity.y = jumpSecond + 3;
            else velocity.y = jumpSecond;
            rigidbody2D.velocity = velocity;
            jumpCount = 2;
        }
    }
    public void downToLine(MGNotification notification)
    {
        //角色会根据上下键反转
        isDown = (isDown + 1) & 1;
        if (isDown==0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            rigidbody2D.gravityScale = 5;
        }
        else if (isDown==1)
        {
            rigidbody2D.gravityScale = 0;
            transform.localScale = new Vector3(1, -1, 1);

        }
		if (notification.objc == null) {
			MGMsgModel msgModel=new MGMsgModel();
			msgModel.eventId="1downToLine";
			msgModel.timestamp=MGGlobalDataCenter.timestamp();
			MGNetWorking.sendMessageToPeer (JsonMapper.ToJson(msgModel));
		}
    }
	// Update is called once per frame
	void Update () {
		if (isReceiveFlag == 1) {
			log.label.text+="role1 Update :"+MGGlobalDataCenter.timestamp()+"\r\n";
			isReceiveFlag=0;
		}

		//判断键盘输入左右键， 用来说明位移
		float h = Input.GetAxis ("Horizontal");
		//float h = 0.1f;
		if (h > 0.05f) {
			//rigidbody2D.AddForce (Vector2.right * forceMove);
			if(rigidbody2D.gravityScale==0)
				rigidbody2D.AddForce (Vector2.right * (forceMove-40));
			else rigidbody2D.AddForce (Vector2.right * forceMove);
		} else if (h < -0.05f) {
			if(rigidbody2D.gravityScale==0)
				rigidbody2D.AddForce (-Vector2.right * (forceMove-40));
			else rigidbody2D.AddForce (-Vector2.right * forceMove);
		}


		//角色会根据左右键反转
		if (h > 0.05f) {
			transform.localScale = new Vector3(1,rigidbody2D.gravityScale==0?-1:1,1);
		} else if (h < -0.05f) {
			transform.localScale = new Vector3(-1,rigidbody2D.gravityScale==0?-1:1,1);
		}


	}

	//判断角色是否在地面上
	public void OnCollisionEnter2D() {
		isGround = true;
	}

	public void OnCollisionExit2D() {
		isGround = false;
	}


}
