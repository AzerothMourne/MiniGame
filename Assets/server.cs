using UnityEngine;
using System.Collections;

public class server : MonoBehaviour {
	private int serverPort; 
	public GUIText status;
	
	void Awake()
	{
		serverPort = 10000;
	}
	
	
	//OnGUI方法，所有GUI的绘制都需要在这个方法中实现  
	void OnGUI()
	{
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
		GUILayout.Label(Network.player.ipAddress);
	}
	
	void StartServer()
	{
		//当用户点击按钮的时候为true  
		if (GUILayout.Button("创建服务器"))
		{
			//初始化本机服务器端口，第一个参数就是本机接收多少连接  
			NetworkConnectionError error = Network.InitializeServer(12, serverPort, false);
			Debug.Log("错误日志" + error);
		}
	}
	
	void OnServer()
	{
		GUILayout.Label("服务端已经运行,等待客户端连接");  
		int length = Network.connections.Length;
		
		for(int i = 0; i < length; i++)
		{
			GUILayout.Label("客户端" + i);
			GUILayout.Label("客户端ip" + Network.connections[i].ipAddress);
			GUILayout.Label("客户端端口" + Network.connections[i].port);  
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
