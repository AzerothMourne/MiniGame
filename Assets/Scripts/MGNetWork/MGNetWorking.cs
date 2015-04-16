using UnityEngine;
using System.Collections;
using LitJson;

public class MGNetWorking : MonoBehaviour {

    void Start()
    {
        print("Init GlobalData");
        MGGlobalDataCenter.defaultCenter();
    }
	public void findHost ()
	{
		print ("findHost");
		P2PBinding.findHost();
		
	}
	public void createHost ()
	{
		print ("createHost");
		P2PBinding.createHost();
		
	}
	public void sendMsgToPeer ( string idStr)
	{
		print ("In Unity sendMsgToPeer:"+idStr);
		P2PBinding.sendMessageToPeer (idStr);
	}
	public void receiverMessageFromPeer ( string msg)
	{
		print ("receiverMessageFromPeer:"+msg+";"+MGGlobalDataCenter.timestamp());
		MGMsgModel msgModel = JsonMapper.ToObject<MGMsgModel>(msg);
		MGNotificationCenter.defaultCenter().postNotification(msgModel.eventId,msgModel);
	}
}
