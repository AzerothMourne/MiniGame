using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;
using System.Net.NetworkInformation;
using System.Threading;

public delegate void SyncNetworkDelegate();
public class MyNetworkTest : MonoBehaviour {
    public int connecttions = 1;
    public UILabel log;
    private Vector3 acceleration;
    public GameObject cube;
	private Thread loomThread;
    void Start()
    {
        
        MGGlobalDataCenter.defaultCenter().serverIp = MGFoundtion.getInternIP();
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
            //Debug.Log(MGGlobalDataCenter.defaultCenter().serverIp);
            //MGNetWorking.findHost();
			loomThread = Loom.RunAsync(() =>
			                           {
				Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);//初始化一个Scoket协议
				IPEndPoint receiveIEP = new IPEndPoint(IPAddress.Any, MGGlobalDataCenter.defaultCenter().mySocketPort);//初始化一个侦听局域网内部所有IP和指定端口
				EndPoint ep = (EndPoint)receiveIEP;
				socket.Bind(receiveIEP);//绑定这个实例
				byte[] buffer = new byte[1024];//设置缓冲数据流
				string ip = null;
				while (true)
				{
					socket.ReceiveFrom(buffer, ref ep);//接收数据,并确把数据设置到缓冲流里面
					ip = Encoding.Unicode.GetString(buffer);
					if (ip.Length>0){
						MGGlobalDataCenter.defaultCenter().serverIp = ip;
						Loom.QueueOnMainThread(() =>{MGNetWorking.findHost();});
						break;
					}
				}
				socket.Close();
			});
//			loomThread.IsBackground = true;
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
			if (loomThread!=null)
			{
				loomThread.Abort();
				while (loomThread.ThreadState != System.Threading.ThreadState.Stopped)//必须等线程完全停止了，否则会出现冲突。  
				{
					Thread.Sleep(1000);
				}
			}
            OnConnect();
        }
    }
    public void UDPSendBroadcast()
    {
		Debug.Log("UDPSendBroadcast"+MGGlobalDataCenter.defaultCenter().serverIp);
        Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);//初始化一个Scoket实习,采用UDP传输
        IPEndPoint iep = new IPEndPoint(IPAddress.Broadcast, MGGlobalDataCenter.defaultCenter().mySocketPort);//初始化一个发送广播和指定端口的网络端口实例
        sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);//设置该scoket实例的发送形式
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
