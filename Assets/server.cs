using UnityEngine;
using System.Collections;

public class server : MonoBehaviour {
	private int serverPort; 
	public UILabel logLabel;
	void Awake()
	{
		serverPort = 10000;
	}
	
	public void OnMouseDown(){
		//Network.peerType是端类型的状态:  
		//即disconnected, connecting, server 或 client四种  
		switch (Network.peerType)
		{
			//禁止客户端连接运行, 服务器未初始化  
		case NetworkPeerType.Disconnected:
			StartServer();
			break;
			//运行于服务器端  
		case NetworkPeerType.Server:
			OnServer();
			break;
			//运行于客户端  
		case NetworkPeerType.Client:
			break;
			//正在尝试连接到服务器  
		case NetworkPeerType.Connecting:
			break;
		}
	}
	
	void StartServer()
	{
			//初始化本机服务器端口，第一个参数就是本机接收多少连接  
			NetworkConnectionError error = Network.InitializeServer(12, serverPort, false);
			Debug.Log("StartServer:" + error);
		logLabel.text+="\r\n"+"StartServer:" + error;
	}
	
	void OnServer()
	{

		logLabel.text+="\r\n"+"server run,waiting connect";  
		int length = Network.connections.Length;
		
		for(int i = 0; i < length; i++)
		{
			logLabel.text+="\r\n"+"client:" + i.ToString();
			logLabel.text+="\r\n"+"clientIp:" + Network.connections[i].ipAddress;
			logLabel.text+="\r\n"+"clientPort:" + Network.connections[i].port.ToString();  
		}
	}
	
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		// Always send transform (depending on reliability of the network view) 
		if (stream.isWriting) 
		{
			Vector3 pos = transform.localPosition; 
			Quaternion rot = transform.localRotation; 
			stream.Serialize(ref pos); 
			stream.Serialize(ref rot); 
		}
		// When receiving, buffer the information 
		else {
			// Receive latest state information 
			Vector3 pos = Vector3.zero; 
			Quaternion rot = Quaternion.identity; 
			stream.Serialize(ref pos); 
			stream.Serialize(ref rot);
		}
	}
}
