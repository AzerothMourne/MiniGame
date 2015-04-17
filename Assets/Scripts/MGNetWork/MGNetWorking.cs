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
		if(Application.platform==RuntimePlatform.IPhonePlayer)
			_findHost();
	}
	public static void createHost()
	{
		if(Application.platform==RuntimePlatform.IPhonePlayer)
			_createHost();
	}
	
	public static void sendMessageToPeer (string msg){
		print ("sendMessageToPeer:"+msg);
		if(Application.platform==RuntimePlatform.IPhonePlayer)
			_sendMessageToPeer(msg);
	}
	public void receiverMessageFromPeer ( string msg)
	{
		print ("receiverMessageFromPeer:"+msg+";"+MGGlobalDataCenter.timestamp());
		MGMsgModel msgModel = JsonMapper.ToObject<MGMsgModel>(msg);
		MGNotificationCenter.defaultCenter().postNotification(msgModel.eventId,msgModel);
	}
}
