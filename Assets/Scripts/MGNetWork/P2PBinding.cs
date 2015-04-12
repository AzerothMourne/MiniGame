using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;


public class P2PBinding: MonoBehaviour{

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

	public static void sendMessageToPeer ( string msg){
		print ("sendMessageToPeer:"+msg);
		if(Application.platform==RuntimePlatform.IPhonePlayer)
			_sendMessageToPeer(msg);
	}

}
