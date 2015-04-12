using UnityEngine;
using System.Collections;

public class MGNetWorking : MonoBehaviour {

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
		print ("receiverMessageFromPeer:"+msg);

	}
}
