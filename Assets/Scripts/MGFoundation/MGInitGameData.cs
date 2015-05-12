using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using LitJson;
using System.Net.NetworkInformation;
using System;
public class MGSyncMsgModel
{
	public string roleLaterPosX { get; set; }
	public string gameTime { get; set; }
}
public class MGInitGameData : MonoBehaviour {
	private Socket syncSock,syncSockTCP,clientSockTCP;
	private IPEndPoint syncIEP;
	private EndPoint syncEP;
	private static MGInitGameData instance;
	private bool isReceiveSync,isR;
	private UILabel label;
	private float receivePosX,receivePosY;
	private Thread syncThread;
	private string serverIp;
	private string Rstr;
	private string[] RstrArr;
	
	void Start()
	{
		isR = false;
		isReceiveSync = false;
		receivePosX = 0f;
		serverIp = MGGlobalDataCenter.defaultCenter().serverIp;
		label = GameObject.Find("log").GetComponent<UIInput>().label;
		initGameData();
	}
	public void initGameData(){
		//MGGlobalDataCenter.defaultCenter().backToDefaultValues();
		if (NetworkPeerType.Disconnected != Network.peerType)
		{
			Debug.Log("startThreadForSync");
			label.text += "startThreadForSync";
			startThreadForSync();
			//startThreadForSyncTCP();
		}
	}
	void startThreadForSync()
	{
		syncSock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);//初始化一个Scoket实习,采用UDP传输
		label.text += "\r\n self " + MGFoundtion.getInternIP() + ";serverip " + MGGlobalDataCenter.defaultCenter().serverIp + ";clientIP" + MGGlobalDataCenter.defaultCenter().clientIP;
		if (MGGlobalDataCenter.defaultCenter().isFrontRoler)
		{
			//label.text += "\r\n self " + MGFoundtion.getInternIP() + ";serverip " + MGGlobalDataCenter.defaultCenter().serverIp;
			IPAddress clientAddress = IPAddress.Broadcast;
			syncIEP = new IPEndPoint(clientAddress, MGGlobalDataCenter.defaultCenter().SyncPort);//初始化一个发送广播和指定端口的网络端口实例
			syncEP = (EndPoint)syncIEP;
			//syncSock.Bind(syncEP);//绑定这个实例
			syncSock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);//设置该scoket实例的发送形式
			InvokeRepeating("syncNetwork", 0.1f, 0.016f);
		}
		else
		{
			//label.text += "\r\n self " + MGFoundtion.getInternIP() + ";serverip " + MGGlobalDataCenter.defaultCenter().serverIp;
			//Debug.Log("serverIp="+MGGlobalDataCenter.defaultCenter().serverIp);
			IPAddress serverAddress = IPAddress.Any;
			syncIEP = new IPEndPoint(serverAddress, MGGlobalDataCenter.defaultCenter().SyncPort);//初始化一个侦听局域网内部所有IP和指定端口
			syncEP = (EndPoint)syncIEP;
			syncSock.Bind(syncEP);//绑定这个实例
			syncThread = new Thread(syncToReceive);
			syncThread.IsBackground = true;
			syncThread.Start();
			
		}
	}
	//线程函数
	public void syncToReceive()
	{
		string receiveString = null, serverIp1 = Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(MGGlobalDataCenter.defaultCenter().serverIp));
		while (true)
		{
			byte[] buffer = new byte[1024];//设置缓冲数据流
			syncSock.ReceiveFrom(buffer, ref syncEP);//接收数据,并确把数据设置到缓冲流里面
			receiveString = Encoding.ASCII.GetString(buffer);
			string[] strArray=new string[2];
			strArray = receiveString.Split(new char[] { '#' });
			//Debug.Log(strArray.Length);
			//Debug.Log(strArray[0] + ";" + serverIp1 + ";" + strArray.Length.ToString());
			//Debug.Log(strArray[0].Length + ";" + serverIp1.Length);
			if (strArray.Length > 1 && serverIp1.Substring(0,strArray[0].Length)==strArray[0])
			{
				isReceiveSync = true;
				receivePosX = float.Parse(strArray[1]);
				//Debug.Log(receivePosX);
			}
		}
	}
	public void syncNetwork()//主机向客户端发送UDP包让客户端同步主机的数据
	{
		if (MGGlobalDataCenter.defaultCenter().isFrontRoler && MGGlobalDataCenter.defaultCenter().roleLater!=null)
		{
			//Debug.Log("syncNetwork;"+MGGlobalDataCenter.defaultCenter().clientIP);
			//label.text = MGGlobalDataCenter.defaultCenter().clientIP + ";" + MGGlobalDataCenter.defaultCenter().serverIp;
			Vector3 roleLaterPos = MGGlobalDataCenter.defaultCenter().roleLater.transform.position;
			byte[] buffer = Encoding.ASCII.GetBytes(MGGlobalDataCenter.defaultCenter().serverIp + "#" + roleLaterPos.x.ToString());
			syncSock.SendTo(buffer, syncEP);
		}
	}
	void Update()
	{
		if (isReceiveSync)
		{
			if (MGGlobalDataCenter.defaultCenter().roleLater != null)
			{
				//Debug.Log("收到同步数据");
				isReceiveSync = false;
				//label.text += receivePosX.ToString();
				Vector3 pos = MGGlobalDataCenter.defaultCenter().roleLater.transform.position;
				MGGlobalDataCenter.defaultCenter().roleLater.transform.position = new Vector3(receivePosX, pos.y, pos.z);
			}
		}
	}
	public void destroyGameData()
	{
		Debug.Log("destroyGameDate");
		MGNotificationCenter.defaultCenter().removeAllObserver();
		if (syncSock != null)
		{
			syncSock.Close();
		}
		if (syncSockTCP != null)
		{
			syncSockTCP.Close();
		}
		if (clientSockTCP != null)
		{
			clientSockTCP.Close();
		}
		if (syncThread != null)
		{
			syncThread.Abort();
		}
		CancelInvoke("syncNetwork");
		MGNetWorking.disconnect();
		MGGlobalDataCenter.defaultCenter().backToDefaultValues();
	}
	void OnDestroy()
	{
		destroyGameData();
	}
	void startThreadForSyncTCP()
	{
		syncSockTCP = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		label.text += "\r\n self " + MGFoundtion.getInternIP() + ";serverip " + MGGlobalDataCenter.defaultCenter().serverIp + ";clientIP" + MGGlobalDataCenter.defaultCenter().clientIP;
		if (MGGlobalDataCenter.defaultCenter().isFrontRoler)
		{
			syncIEP = new IPEndPoint(IPAddress.Parse(MGGlobalDataCenter.defaultCenter().clientIP), MGGlobalDataCenter.defaultCenter().SyncPort);//服务器的IP和端口
			try
			{
				//因为客户端只是用来向特定的服务器发送信息，所以不需要绑定本机的IP和端口。不需要监听。
				syncSockTCP.Connect(syncIEP);
			}
			catch (SocketException e)
			{
				label.text += " unable to connect to server" + e.ToString();
			}
			InvokeRepeating("syncNetworkTCP", 0.1f, 0.016f);
		}
		else
		{
			syncIEP = new IPEndPoint(IPAddress.Any, MGGlobalDataCenter.defaultCenter().SyncPort);//本机预使用的IP和端口
			syncSockTCP.Bind(syncIEP);
			syncThread = new Thread(syncToReceiveTCP);
			syncThread.IsBackground = true;
			syncThread.Start();
		}
	}
	void syncToReceiveTCP()
	{
		string receiveString = null;
		syncSockTCP.Listen(10);//监听
		clientSockTCP = syncSockTCP.Accept();
		//IPEndPoint clientip = (IPEndPoint)clientSockTCP.RemoteEndPoint;
		//Debug.Log("connect with client:" + clientip.Address + " at port:" + clientip.Port);
		while (true)
		{//用死循环来不断的从客户端获取信息
			byte[] buffer = new byte[1024];//设置缓冲数据流
			clientSockTCP.Receive(buffer);
			receiveString = Encoding.ASCII.GetString(buffer);
			isReceiveSync = true;
			if (receiveString.Length>0)
			{
				isReceiveSync = true;
				receivePosX = float.Parse(receiveString);
			}
		}
	}
	void syncNetworkTCP()
	{
		if (MGGlobalDataCenter.defaultCenter().isFrontRoler && MGGlobalDataCenter.defaultCenter().roleLater != null)
		{
			//Debug.Log("syncNetwork;"+MGGlobalDataCenter.defaultCenter().clientIP);
			//label.text = MGGlobalDataCenter.defaultCenter().clientIP + ";" + MGGlobalDataCenter.defaultCenter().serverIp;
			Vector3 roleLaterPos = MGGlobalDataCenter.defaultCenter().roleLater.transform.position;
			byte[] buffer = Encoding.ASCII.GetBytes(roleLaterPos.x.ToString());
			try
			{
				syncSockTCP.Send(buffer);
			}
			catch (SocketException e)
			{
				//label.text += " send execption" + e.ToString();
				try
				{
					//因为客户端只是用来向特定的服务器发送信息，所以不需要绑定本机的IP和端口。不需要监听。
					syncSockTCP.Connect(syncIEP);
				}
				catch 
				{
					label.text = "syncNetworkTCP unable to connect to server" + e.ToString();
				}
			}
		}
	}
}
