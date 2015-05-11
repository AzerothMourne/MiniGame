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
    private Socket syncSock;
    private IPEndPoint syncIEP;
    private EndPoint syncEP;
    private static MGInitGameData instance;
	private bool isReceiveSync;
    private UILabel label;
    private float receivePosX,receivePosY;
    private Thread syncThread;
    void Awake()
    {
        initGameData();
    }
    void Start()
    {
        isReceiveSync = false;
        receivePosX = 0f;
        label = GameObject.Find("log").GetComponent<UIInput>().label;
    }
	public void initGameData(){
        //MGGlobalDataCenter.defaultCenter().backToDefaultValues();
        if (NetworkPeerType.Disconnected != Network.peerType)
        {
			Debug.Log("startThreadForSync");
            startThreadForSync();
        }
    }
    void startThreadForSync()
    {
        syncSock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);//初始化一个Scoket实习,采用UDP传输
        if (MGGlobalDataCenter.defaultCenter().isFrontRoler)
        {
			syncIEP = new IPEndPoint(IPAddress.Parse(MGGlobalDataCenter.defaultCenter().clientIP), MGGlobalDataCenter.defaultCenter().SyncPort);//初始化一个发送广播和指定端口的网络端口实例
            //syncSock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);//设置该scoket实例的发送形式
            InvokeRepeating("syncNetwork", 0.1f, 0.010f);
        }
        else
        {
            syncIEP = new IPEndPoint(IPAddress.Parse(MGGlobalDataCenter.defaultCenter().serverIp), MGGlobalDataCenter.defaultCenter().SyncPort);//初始化一个侦听局域网内部所有IP和指定端口
            syncEP = (EndPoint)syncIEP;
            syncSock.Bind(syncIEP);//绑定这个实例
            syncThread = new Thread(syncToReceive);
			syncThread.IsBackground = true;
            syncThread.Start();
            
        }
    }
    //线程函数
    public void syncToReceive()
    {
        string receiveString = null;
        while (true)
        {
            byte[] buffer = new byte[1024];//设置缓冲数据流
            syncSock.ReceiveFrom(buffer, ref syncEP);//接收数据,并确把数据设置到缓冲流里面
            receiveString = Encoding.ASCII.GetString(buffer);
            if (receiveString.Length > 0)
            {
                isReceiveSync = true;
				string[] arr = receiveString.Split(new char[]{'#'});
                receivePosX = float.Parse(arr[0]);
				receivePosY = float.Parse(arr[1]);
            }
        }
    }
    public void syncNetwork()//主机向客户端发送UDP包让客户端同步主机的数据
    {
        if (MGGlobalDataCenter.defaultCenter().isFrontRoler && MGGlobalDataCenter.defaultCenter().roleLater!=null)
        {
			Debug.Log("syncNetwork;"+MGGlobalDataCenter.defaultCenter().clientIP);
            Vector3 roleLaterPos = MGGlobalDataCenter.defaultCenter().roleLater.transform.position;
			byte[] buffer = Encoding.ASCII.GetBytes(roleLaterPos.x.ToString()+"#"+roleLaterPos.y.ToString());
            syncSock.SendTo(buffer, syncIEP);
        }
    }
    void Update()
    {
        if (isReceiveSync)
        {
            if (MGGlobalDataCenter.defaultCenter().roleLater != null)
            {
                isReceiveSync = false;
                //label.text += "receiveString ";
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

}
