﻿using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Net.NetworkInformation;
using System.Threading;
/// <summary>
/// 还需要建立一个误差校正机制，保证客户机以服务器端的数据为主（重要）
/// </summary>
public class MGNetWorking : MonoBehaviour {
    private Socket mainSocket,syncSocket;
    private IPEndPoint mainSocketIPE;
    private EndPoint mainSocketEP;
    public static void sendMsgToPeer(string ip, int port)
    {

    }
    public void connectToServer(string ip,int port)
    {

    }
    //线程函数
    public void mainSocketListen()
    {
        string receiveString = null;
        while (true)
        {
            byte[] buffer = new byte[1024];//设置缓冲数据流
            mainSocket.ReceiveFrom(buffer, ref mainSocketEP);//接收数据,并确把数据设置到缓冲流里面
            receiveString = Encoding.ASCII.GetString(buffer);
            if (receiveString.Length > 0)
            {
               
                

            }
        }
    }
	public static NetworkConnectionError findHost()
	{
        MGGlobalDataCenter.defaultCenter().isFrontRoler = false;
        MGGlobalDataCenter.defaultCenter().isLaterRoler = true;
        return Network.Connect(MGGlobalDataCenter.defaultCenter().serverIp, MGGlobalDataCenter.defaultCenter().listenPort);
		
	}
	public static void createHost()
	{
        MGGlobalDataCenter.defaultCenter().isFrontRoler = true;
        MGGlobalDataCenter.defaultCenter().isLaterRoler = false;
        Network.InitializeServer(MGGlobalDataCenter.defaultCenter().connecttions, MGGlobalDataCenter.defaultCenter().listenPort, false);

	}
    public static void disconnect()
    {
        Network.Disconnect();
    }
    /// <summary>
    /// 重载sendMessageToPeer支持unity提供的NetworkView，方便在局域网条件下跨平台
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="networkView"></param>
	public void sendMessageToPeer (string msg){
        if (NetworkPeerType.Disconnected != Network.peerType)
        {
            //print("sendMessageToPeer:" + msg);
            networkView.RPC("RPCReceiverMessageFromPeer", RPCMode.Others, msg);
        }
	}
    public void sendMessageToPeer(string name,string msg)
    {
        if (NetworkPeerType.Disconnected != Network.peerType)
        {
            //print("sendMessageToPeer:" + msg);
            networkView.RPC(name, RPCMode.Others, msg);
        }
    }
    [RPC]
    public void RPCReceiverMessageFromPeer(string msg, NetworkMessageInfo info)
    {
        //刚从网络接收的数据的相关信息,会被保存到NetworkMessageInfo这个结构中  
        string sender = info.sender.ToString();
        //看脚本运行在什么状态下  
        NetworkPeerType status = Network.peerType;
        if (status != NetworkPeerType.Disconnected && sender!="-1")
        {
            receiverMessageFromPeer(msg);
        }
    }
    
	public void receiverMessageFromPeer ( string msg)
	{
		//Debug.Log ("receiverMessageFromPeer:"+msg+";"+MGGlobalDataCenter.timestamp());
		MGMsgModel msgModel = JsonMapper.ToObject<MGMsgModel>(msg);
        if (msgModel.eventId == RoleActEventEnum.gameoverEventId)
        {
            MGGlobalDataCenter.defaultCenter().overSenceUIName = msgModel.gameobjectName;
            if (Application.loadedLevelName != "overSence")
            {
                Application.LoadLevel("overSence");
            }
            return;
        }
		MGNotificationCenter.defaultCenter().postNotification(msgModel.eventId,msgModel);
	}
    public Object Instantiate(UnityEngine.Object prefab,Vector3 position,Quaternion rotation,int group)
    {
        return Network.Instantiate(prefab, position, rotation, group);
    }
}
