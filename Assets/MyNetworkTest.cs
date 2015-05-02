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
public delegate void listenForServerDelegate();
public class MyNetworkTest : MonoBehaviour {
    public int connecttions = 1;
    public UILabel log;
    private Vector3 acceleration;
    public GameObject cube;
    private listenForServerDelegate udpListen;
    private bool isCancelListen;
    private Socket listenSocket;
    void Start()
    {
        isCancelListen = false;
        udpListen = new listenForServerDelegate(listenForServer);
        MGGlobalDataCenter.defaultCenter().serverIp = MGFoundtion.getInternIP();
        listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);//初始化一个Scoket协议
    }
    public void createHost()
    {
        if (NetworkPeerType.Disconnected == Network.peerType)
        {
            MGNetWorking.createHost();
            InvokeRepeating("UDPSendBroadcast", 0f, 0.1f);
        }
    }
    public void findHost()
    {
        if (NetworkPeerType.Disconnected == Network.peerType)
        {
            Debug.Log("find host");
            InvokeRepeating("tryToConnect", 0.1f, 0.1f);
            isCancelListen = false;
            udpListen.BeginInvoke(new AsyncCallback(callbackMethod), null);
        }
    }
    void tryToConnect()
    {
        Debug.Log("tryToConnect");
        if(isCancelListen == false)
            MGNetWorking.findHost();
    }
    void listenForServer()
    {
        IPEndPoint receiveIEP = new IPEndPoint(IPAddress.Any, MGGlobalDataCenter.defaultCenter().mySocketPort);//初始化一个侦听局域网内部所有IP和指定端口
        EndPoint ep = (EndPoint)receiveIEP;
        listenSocket.Bind(receiveIEP);//绑定这个实例
        string modelJson = null;
        while (true)
        {
            byte[] buffer = new byte[1024];//设置缓冲数据流
            listenSocket.ReceiveFrom(buffer, ref ep);//接收数据,并确把数据设置到缓冲流里面
            modelJson = Encoding.Unicode.GetString(buffer);
            if (isCancelListen) break;
            if (modelJson.Length > 0)
            {
                MGGlobalDataCenter.defaultCenter().serverIp = modelJson;
                break;
            }
        }
        listenSocket.Close();
    }
    //回调方法
    static void callbackMethod(IAsyncResult Ias)
    {
        listenForServerDelegate Md = (listenForServerDelegate)Ias.AsyncState;
        Md.EndInvoke(Ias);
    }
    public void cancelConnect()
    {
        isCancelListen = true;
        CancelInvoke("UDPSendBroadcast");
        CancelInvoke("tryToConnect");
        if (listenSocket != null)
        {
            Debug.Log("close socket");
            try
            {
                listenSocket.Shutdown(SocketShutdown.Receive);
            }
            catch
            {
                Debug.Log("关闭receive失败");
            }
            listenSocket.Close();
        }
    }
    void Update()
    {
        if (Network.peerType == NetworkPeerType.Server)
        {
            if (Network.connections.Length == 1)
            {
                CancelInvoke();
                OnConnect();
            }
                
        }
        else if (Network.peerType == NetworkPeerType.Client)
        {
            CancelInvoke();
            OnConnect();
        }
    }
    public void UDPSendBroadcast()
    {
		Debug.Log("UDPSendBroadcast"+MGGlobalDataCenter.defaultCenter().serverIp);
        if (isCancelListen) return;
        Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);//初始化一个Scoket实习,采用UDP传输
        IPEndPoint iep = new IPEndPoint(IPAddress.Broadcast, MGGlobalDataCenter.defaultCenter().mySocketPort);//初始化一个发送广播和指定端口的网络端口实例
        sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);//设置该scoket实例的发送形式
        /*
        MGConnectModel model = new MGConnectModel();
        model.ip = MGGlobalDataCenter.defaultCenter().serverIp;
        model.timestamp = MGGlobalDataCenter.timestamp();
        */
        byte[] buffer = Encoding.Unicode.GetBytes(MGGlobalDataCenter.defaultCenter().serverIp);
        sock.SendTo(buffer, iep);
        sock.Close();
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
        Application.LoadLevel("gameScene1");
    }
}
