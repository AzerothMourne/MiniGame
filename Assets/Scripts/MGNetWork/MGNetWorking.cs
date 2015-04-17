using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class MGNetWorking : MonoBehaviour {

    void Start()
    {
        print("Init GlobalData");
        MGGlobalDataCenter.defaultCenter();
    }
	[DllImport("__Internal")]
	private static extern void _findHost();
	[DllImport("__Internal")]
	private static extern void _createHost();
	[DllImport( "__Internal" )]
	private static extern void _testUnityToiOS ( string msg);
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
    /// <summary>
    /// 支持iOS的Multipeer Connectivity，在没有局域网的时候使用蓝牙
    /// </summary>
    /// <param name="msg"></param>
    public static void sendMessageToPeer(string msg)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            print("sendMessageToPeer:" + msg);
            _sendMessageToPeer(msg);
        }

    }
    /// <summary>
    /// 重载sendMessageToPeer支持unity提供的NetworkView，方便在局域网条件下跨平台
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="networkView"></param>
	public static void sendMessageToPeer (string msg,NetworkView networkView){
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
            sendMessageToPeer(msg);
        }	
	}
    public static void sendMessageToPeer(string name,string msg, NetworkView networkView)
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
            sendMessageToPeer(msg);
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
		print ("receiverMessageFromPeer:"+msg+";"+MGGlobalDataCenter.timestamp());
		MGMsgModel msgModel = JsonMapper.ToObject<MGMsgModel>(msg);
		MGNotificationCenter.defaultCenter().postNotification(msgModel.eventId,msgModel);
	}
}
