using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;
using System.Net.NetworkInformation;

public delegate string UDPDelegate(int port);
public class MyNetworkTest : MonoBehaviour {
    public int connecttions = 10;
    public int listenPort = 8899 , mySocketPort=10000;
    public UILabel log;
    private Vector3 acceleration;
    public GameObject cube;
	private string ip = "127.0.0.1";
    private UDPDelegate udpReceive;
    void Start()
    {
        NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
        foreach (NetworkInterface adapter in adapters)
        {
            if (adapter.Supports(NetworkInterfaceComponent.IPv4))
            {
                UnicastIPAddressInformationCollection uniCast = adapter.GetIPProperties().UnicastAddresses;
                if (uniCast.Count > 0)
                {
                    foreach (UnicastIPAddressInformation uni in uniCast)
                    {
                        //得到IPv4的地址。 AddressFamily.InterNetwork指的是IPv4
                        if (uni.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            ip = uni.Address.ToString();
                        }
                    }
                }
            }
        }
        MGGlobalDataCenter.defaultCenter().serverIp = ip;
        udpReceive = new UDPDelegate(UDPStartToReceive);
    }
    public void createHost()
    {
        if (NetworkPeerType.Disconnected == Network.peerType)
        {
            MGNetWorking.createHost();
            InvokeRepeating("UDPSendBroadcast", 0f, 1f);
        }
    }
    public void findHost()
    {
        if (NetworkPeerType.Disconnected == Network.peerType)
        {
            //Debug.Log(MGGlobalDataCenter.defaultCenter().serverIp);
            //MGNetWorking.findHost();
            udpReceive.BeginInvoke(mySocketPort, UDPStartToReceiveCallback, null);
        }
    }
    void Update()
    {
        if (Network.peerType == NetworkPeerType.Server)
        {
            if (Network.connections.Length == 1)
                OnConnect();
        }
        else if (Network.peerType == NetworkPeerType.Client)
        {
            OnConnect();
        }
    }
    public void UDPSendBroadcast()
    {
        Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);//初始化一个Scoket实习,采用UDP传输
        IPEndPoint iep = new IPEndPoint(IPAddress.Broadcast, mySocketPort);//初始化一个发送广播和指定端口的网络端口实例
        sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);//设置该scoket实例的发送形式
        byte[] buffer = Encoding.Unicode.GetBytes(ip);
        sock.SendTo(buffer, iep);
        sock.Close();
    }
    //线程函数
    public string UDPStartToReceive(int port)
    {
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);//初始化一个Scoket协议
        IPEndPoint iep = new IPEndPoint(IPAddress.Any, port);//初始化一个侦听局域网内部所有IP和指定端口
        EndPoint ep = (EndPoint)iep;
        socket.Bind(iep);//绑定这个实例
        byte[] buffer = new byte[1024];//设置缓冲数据流
        string ip = null;
        while (true)
        {
            socket.ReceiveFrom(buffer, ref ep);//接收数据,并确把数据设置到缓冲流里面
            ip = Encoding.Unicode.GetString(buffer);
            if (ip.Length>0) break;
        }
        return ip;
    }

    //异步回调函数
    public void UDPSendBroadcastCallback(IAsyncResult data)
    {

    }
    public void UDPStartToReceiveCallback(IAsyncResult data)
    {
        //异步执行完成
        string resultstr = udpReceive.EndInvoke(data);
        if (resultstr.Length == 0)
        {
            udpReceive.BeginInvoke(mySocketPort, UDPStartToReceiveCallback, null);
        }
        else
        {

            //收到IP，连接主机
            MGGlobalDataCenter.defaultCenter().serverIp = resultstr;
            MGNetWorking.findHost();
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
        Application.LoadLevel("gameScene1");
    }
}
