﻿using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;
using System;

public class JumpFront : MonoBehaviour
{

    public float forceMove;
    private bool isGround;
    public float jumpVelocity;
    public float jumpSecond;
    public float jumpCount;
    public int isJump, isDown;
    public MGskillDrat drat;
    public UIInput log;
    private int isReceiveFlag;
    private MGNetWorking mgNetWorking;
    private bool isPressDown;
    // Use this for initialization
    void Start()
    {
        MGGlobalDataCenter.defaultCenter().roleLater = this.gameObject;
        isJump = 0;
        isDown = 0;
        isGround = true;
        isPressDown = false;
        isReceiveFlag = 0;
        mgNetWorking = GameObject.Find("Main Camera").GetComponent<MGNetWorking>();
        //forceMove = 50;
        //jumpVelocity = 25;
        //jumpSecond = 15;
        MGNotificationCenter.defaultCenter().addObserver(this, useSkillsDart, "useSkillsDart");
        MGNotificationCenter.defaultCenter().addObserver(this, firstJump, "jump");
        MGNotificationCenter.defaultCenter().addObserver(this, upwardToLine, EventEnum.upwardToLineFormerEventId);
        MGNotificationCenter.defaultCenter().addObserver(this, downToLine, "downToLine");
        switch (Network.peerType)
        {
            case NetworkPeerType.Disconnected:
                log.label.text = "NetworkPeerType.Disconnectd";
                break;
            case NetworkPeerType.Client:
                log.label.text = "NetworkPeerType.Client";
                break;
            case NetworkPeerType.Server:
                log.label.text = "NetworkPeerType.Server";
                break;
        }
    }
    public void useSkillsDart(MGNotification notification)
    {
        GameObject role1 = this.gameObject;
        if (notification.objc == null)
        {
            MGMsgModel msgModel = new MGMsgModel();
            msgModel.eventId = "useSkillsDart";
            msgModel.timestamp = MGGlobalDataCenter.timestamp();
            Vector3 pos = new Vector3(role1.transform.position.x, role1.transform.position.y + (isDown == 0 ? 1 : -1) * role1.renderer.bounds.size.y / 2, role1.transform.position.z);
            if (Network.peerType != NetworkPeerType.Disconnected)
            {
                GameObject.Instantiate(drat, pos, Quaternion.Euler(0, 0, -1));
                //drat.createSkillSprite(pos);
                //mgNetWorking.Instantiate(drat, pos, new Quaternion(), 0);
            }
            else
                drat.createSkillSprite(pos);
        }
    }
    public void firstJump(MGNotification notification)
    {
        print("is ground " + isGround);
        //如果在地面上，则一段跳
        //print ("time : " + System.DateTime.Now);
        //print (" the role is : " + this.gameObject.name);
        if (isDown == 1)
        {
            upwardToLine(notification);
            return;
        }
        if (isGround)
        {
            Vector2 velocity = rigidbody2D.velocity;
            velocity.y = jumpVelocity;
            rigidbody2D.velocity = velocity;
            jumpCount = 1;
            //如果没有发送给对方，则发送消息
            if (notification.objc == null)
            {
                //log.label.text+="jump send:" + MGGlobalDataCenter.timestamp ()+"\r\n";
                MGMsgModel msgModel = new MGMsgModel();
                msgModel.eventId = "jump";
                //print("eventId : "+msgModel.eventId);
                msgModel.timestamp = MGGlobalDataCenter.timestamp();
                mgNetWorking.sendMessageToPeer(JsonMapper.ToJson(msgModel));
            }
            else
            {
                //log.label.text+="jump receive:" + MGGlobalDataCenter.timestamp ()+"\r\n";
                //isReceiveFlag=1;
                long time1 = MGGlobalDataCenter.timestamp();
                long time2 = ((MGMsgModel)notification.objc).timestamp;
                //print (time1 + ";" + time2);
                log.label.text += (Mathf.Abs(time1 - time2)).ToString() + "\r\n";
            }
        }
        //如果不在地面上，且一段跳了，则二段跳
        else if (!isGround && jumpCount == 1)
        {
            Vector2 velocity = rigidbody2D.velocity;
            if (velocity.y < -1.0f) velocity.y = jumpSecond + 3;
            else velocity.y = jumpSecond;
            rigidbody2D.velocity = velocity;
            jumpCount = 2;
        }
        /*
        isJump = 1;
        Vector2 velocity = rigidbody2D.velocity;
        velocity.y = jumpVelocity;
        rigidbody2D.velocity = velocity;
        jumpCount = 1;
        if (notification.objc == null)
        {
            //log.label.text+="jump send:" + MGGlobalDataCenter.timestamp ()+"\r\n";
            MGMsgModel msgModel = new MGMsgModel();
            msgModel.eventId = "jump";
            msgModel.timestamp = MGGlobalDataCenter.timestamp();
            mgNetWorking.sendMessageToPeer(JsonMapper.ToJson(msgModel));
        }
        else
        {
            //log.label.text+="jump receive:" + MGGlobalDataCenter.timestamp ()+"\r\n";
            //isReceiveFlag=1;
            long time1 = MGGlobalDataCenter.timestamp();
            long time2 = ((MGMsgModel)notification.objc).timestamp;
            print(time1 + ";" + time2);
            log.label.text += (Mathf.Abs(time1 - time2)).ToString() + "\r\n";
        }
        */

    }

    public void downToLine(MGNotification notification)
    {
        //角色会根据下按钮，翻转到线下
        //print ("gravityScale : " + rigidbody2D.gravityScale);
        print("is ground " + isGround);
        if (isDown == 0)
        {
            //print("downToLine isDown : "+isDown);
            if (isGround)
            {
                rigidbody2D.gravityScale = 0;
                transform.localScale = new Vector3(1, -1, 1);
                isDown = 1;
            }
            else if (!isGround)
            {
                print ("not on the ground");
                rigidbody2D.gravityScale = 10;
                isPressDown = true;
            }
            
        }

        if (notification.objc == null)
        {
            MGMsgModel msgModel = new MGMsgModel();
            msgModel.eventId = "downToLine";
            msgModel.timestamp = MGGlobalDataCenter.timestamp();
            mgNetWorking.sendMessageToPeer(JsonMapper.ToJson(msgModel));
        }
        //print("downToLine receive time : " + MGGlobalDataCenter.timestamp());
        /*
        //角色会根据上下键反转
        isDown = (isDown + 1) & 1;
        if (isDown == 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            rigidbody2D.gravityScale = 5;
        }
        else if (isDown == 1)
        {
            rigidbody2D.gravityScale = 0;
            transform.localScale = new Vector3(1, -1, 1);

        }
        if (notification.objc == null)
        {
            MGMsgModel msgModel = new MGMsgModel();
            msgModel.eventId = "downToLine";
            msgModel.timestamp = MGGlobalDataCenter.timestamp();
            mgNetWorking.sendMessageToPeer(JsonMapper.ToJson(msgModel));
        }*/
    }
    public void upwardToLine(MGNotification notification)
    {
        //角色会根据上按钮从线下到线上
        //print ("up  gravityScale : " + rigidbody2D.gravityScale);
        if (isDown == 1)
        {
            //print("upwardToLine isDown : "+isDown);
            transform.localScale = new Vector3(1, 1, 1);
            rigidbody2D.gravityScale = 5;
            isDown = 0;
            isGround = true;
        }
        if (notification.objc == null)
        {
            MGMsgModel msgModel = new MGMsgModel();
            msgModel.eventId = EventEnum.upwardToLineFormerEventId;
            msgModel.timestamp = MGGlobalDataCenter.timestamp();
            mgNetWorking.sendMessageToPeer(JsonMapper.ToJson(msgModel));
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (isReceiveFlag == 1)
        {
            log.label.text += "Role Update :" + MGGlobalDataCenter.timestamp() + "\r\n";
            isReceiveFlag = 0;
        }
        //判断是否在地面上
        //RaycastHit hitinfo;
        //isGround = Physics.Raycast(transform.position + (isDown == 0 ? Vector3.up : Vector3.down) * 0.0f, (isDown == 1 ? Vector3.up : Vector3.down), out hitinfo, 0.1f, groundLayerMask);
        //判断键盘输入左右键， 用来说明位移
        float h = Input.GetAxis("Horizontal");
        //float h = 0.1f;
        if (h > 0.05f)
        {
            //rigidbody2D.AddForce (Vector2.right * forceMove);
            if (rigidbody2D.gravityScale == 0)
                rigidbody2D.AddForce(Vector2.right * (forceMove - 40));
            else rigidbody2D.AddForce(Vector2.right * forceMove);
        }
        else if (h < -0.05f)
        {
            if (rigidbody2D.gravityScale == 0)
                rigidbody2D.AddForce(-Vector2.right * (forceMove - 40));
            else rigidbody2D.AddForce(-Vector2.right * forceMove);
        }


        //角色会根据左右键反转
        if (h > 0.05f)
        {
            transform.localScale = new Vector3(1, rigidbody2D.gravityScale == 0 ? -1 : 1, 1);
        }
        else if (h < -0.05f)
        {
            transform.localScale = new Vector3(-1, rigidbody2D.gravityScale == 0 ? -1 : 1, 1);
        }
        
        //当下落过程中按了down按钮，则会在落地后下翻
        if (isPressDown && isGround)
        {
            print("isPressDown && isGround");
            rigidbody2D.gravityScale = 0;
            transform.localScale = new Vector3(1, -1, 1);
            isPressDown = false;
            isDown = 1;
        }
        

    }
    
    //判断角色是否在地面上
    public void OnCollisionEnter2D()
    {
        isGround = true;
        print("OnCollisionEnter2D" + "is ground " + isGround);
    }

    public void OnCollisionExit2D()
    {
        isGround = false;
        print("OnCollisionExit2D" + "is ground " + isGround);
    }


}
