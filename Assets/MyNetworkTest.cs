using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;
using System.Net.NetworkInformation;
using System.Threading;
using LitJson;

public class MGConnectModel
{
	public string ip { get; set; }
	public long timestamp { get; set; }
}
public delegate string listenForServerDelegate(int port);
public class MyNetworkTest : MonoBehaviour {

	public int connecttions = 1;
	public UILabel log;
	private Vector3 acceleration;
	public GameObject cube;
	private listenForServerDelegate udpReceive;
	private bool isCancelListen,isReceiveIP;
	void Start()
	{
		isReceiveIP = false;
		isCancelListen = false;
		udpReceive = new listenForServerDelegate(UDPStartToReceive);
		MGGlobalDataCenter.defaultCenter().serverIp = MGFoundtion.getInternIP();
	}
	public void createHost()
	{
		if (NetworkPeerType.Disconnected == Network.peerType || NetworkPeerType.Server == Network.peerType) {
			MGNetWorking.createHost ();
			InvokeRepeating ("UDPSendBroadcast", 0f, 0.1f);
		}
	}
	public void findHost()
	{
		if (NetworkPeerType.Disconnected == Network.peerType) {
			Debug.Log ("find host");
			isCancelListen = false;
			isReceiveIP = false;
			udpReceive.BeginInvoke (MGGlobalDataCenter.defaultCenter ().mySocketPort, UDPStartToReceiveCallback, null);
		} 
	}
	
	//线程函数
	public string UDPStartToReceive(int port)
	{
		Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);//初始化一个Scoket协议
		IPEndPoint iep = new IPEndPoint(IPAddress.Any, MGGlobalDataCenter.defaultCenter().mySocketPort);//初始化一个侦听局域网内部所有IP和指定端口
		EndPoint ep = (EndPoint)iep;
		socket.Bind(iep);//绑定这个实例
		byte[] buffer = new byte[1024];//设置缓冲数据流
		string ip = null;
		while (true)
		{
			socket.ReceiveFrom(buffer, ref ep);//接收数据,并确把数据设置到缓冲流里面
			if(isCancelListen) break;
			ip = Encoding.Unicode.GetString(buffer);
			IPAddress myAddress = null;
			if (IPAddress.TryParse(ip,out myAddress))
			{
				break;
			}
		}
		socket.Close();
		return ip;
	}
	public void UDPStartToReceiveCallback(IAsyncResult data)
	{
		//异步执行完成
		string resultstr = udpReceive.EndInvoke(data);
		IPAddress myAddress = null;
		Debug.Log("resultstr:"+resultstr);
		if (IPAddress.TryParse(resultstr,out myAddress))
		{
			MGGlobalDataCenter.defaultCenter().serverIp = resultstr;
			isReceiveIP=true;
		}
		else
		{
			Debug.Log("ip不合法");
			if(!isCancelListen){
				isCancelListen = false;
				isReceiveIP = false;
				udpReceive.BeginInvoke(MGGlobalDataCenter.defaultCenter().mySocketPort, UDPStartToReceiveCallback, null);
			}
		}
	}
	public void cancelConnect()
	{
		isCancelListen = true;
		UDPSendBroadcast ();
		CancelInvoke("UDPSendBroadcast");
		Network.Disconnect ();
		Debug.Log (Network.peerType);
	}
	void Update()
	{
		if (isReceiveIP) {
			Debug.Log("isReceiveIP=true");
			isReceiveIP = false;
			try{
				NetworkConnectionError connectError = MGNetWorking.findHost();
				log.text += "\r\nconnect";
				Debug.Log("connect");
				if(connectError != NetworkConnectionError.NoError && !isCancelListen){
					log.text += "\r\nconnect faild";
					Debug.Log("connect faild");
					isCancelListen = false;
					isReceiveIP = false;
					udpReceive.BeginInvoke(MGGlobalDataCenter.defaultCenter().mySocketPort, UDPStartToReceiveCallback, null);
				}
			}catch{
				log.text += "\r\nconnect execption";
				Debug.Log("connect execpti");
				isCancelListen = false;
				isReceiveIP = false;
				udpReceive.BeginInvoke(MGGlobalDataCenter.defaultCenter().mySocketPort, UDPStartToReceiveCallback, null);
			}

		}
		if (Network.peerType == NetworkPeerType.Server)
		{
			if (Network.connections.Length == 1)
			{
				CancelInvoke("UDPSendBroadcast");
				MGGlobalDataCenter.defaultCenter().clientIP = Network.connections[0].ipAddress;
				Debug.Log("clientIP=" + MGGlobalDataCenter.defaultCenter().clientIP);
				OnConnect();
			}
		}
		else if (Network.peerType == NetworkPeerType.Client)
		{
			OnConnect();
		}
	}
	public void UDPSendBroadcast()
	{
		Socket sock = new Socket (AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);//初始化一个Scoket实习,采用UDP传输
		IPEndPoint iep = new IPEndPoint (IPAddress.Broadcast, MGGlobalDataCenter.defaultCenter ().mySocketPort);//初始化一个发送广播和指定端口的网络端口实例
		sock.SetSocketOption (SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);//设置该scoket实例的发送形式
		Debug.Log("UDPSendBroadcast"+MGGlobalDataCenter.defaultCenter().serverIp);
		log.text += "\r\n" + MGGlobalDataCenter.defaultCenter().serverIp;
		byte[] buffer = Encoding.Unicode.GetBytes(MGGlobalDataCenter.defaultCenter().serverIp);
		try{
			sock.SendTo(buffer, iep);
			sock.Close ();
		}catch{
		}
	}
	void OnConnect()
	{
		/*
        if (!cubeInitialed)
        {
            Network.Instantiate(cube, transform.position, transform.rotation, 0);
            cubeInitialed = true;
        }*/
		//连接成功 需要切换场景
		//MGNotificationCenter.defaultCenter().postNotification(LoadSenceEnum.LoadLevel, "gameScene1");
		MGGlobalDataCenter.defaultCenter().isSingle = false;
		Application.LoadLevel("gameScene1");
	}
	void OnApplicationQuit(){
		Debug.Log ("OnApplicationQuit");
		isCancelListen = true;
		UDPSendBroadcast ();
		CancelInvoke("UDPSendBroadcast");
	}
}
