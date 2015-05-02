﻿using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;
using System.Runtime.InteropServices;

/// <summary>
/// 还需要建立一个误差校正机制，保证客户机以服务器端的数据为主（重要）
/// </summary>
public class MGNetWorking : MonoBehaviour {
	[DllImport("__Internal")]
	private static extern void _findHost();
	[DllImport("__Internal")]
	private static extern void _createHost();
	[DllImport( "__Internal" )]
	private static extern void _sendMessageToPeer ( string msg);
	public static void findHost()
	{
        MGGlobalDataCenter.defaultCenter().isHost = false;
        if (MGGlobalDataCenter.defaultCenter().isNetworkViewEnable == true)
        {
            Network.Connect(MGGlobalDataCenter.defaultCenter().serverIp, MGGlobalDataCenter.defaultCenter().listenPort);
        }
		else if(Application.platform==RuntimePlatform.IPhonePlayer)
			_findHost();
	}
	public static void createHost()
	{
        MGGlobalDataCenter.defaultCenter().isHost = true;
        if (MGGlobalDataCenter.defaultCenter().isNetworkViewEnable == true)
        {
            Network.InitializeServer(MGGlobalDataCenter.defaultCenter().connecttions, MGGlobalDataCenter.defaultCenter().listenPort, false);
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
			_createHost();
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
        if (MGGlobalDataCenter.defaultCenter().isNetworkViewEnable == true)
        {
            if (NetworkPeerType.Disconnected != Network.peerType)
            {
                print("sendMessageToPeer:" + msg);
                networkView.RPC("RPCReceiverMessageFromPeer", RPCMode.Others, msg);
            }
                
        }
        else
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
                MGNetWorking._sendMessageToPeer(msg);
        }	
	}
    public void sendMessageToPeer(string name,string msg)
    {
        if (MGGlobalDataCenter.defaultCenter().isNetworkViewEnable == true)
        {
            if (NetworkPeerType.Disconnected != Network.peerType)
            {
                print("sendMessageToPeer:" + msg);
                networkView.RPC(name, RPCMode.Others, msg);
            }

        }
        else
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
                MGNetWorking._sendMessageToPeer(msg);
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
		Debug.Log ("receiverMessageFromPeer:"+msg+";"+MGGlobalDataCenter.timestamp());
		MGMsgModel msgModel = JsonMapper.ToObject<MGMsgModel>(msg);
        if (msgModel.eventId == EventEnum.gameoverEventId)
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
