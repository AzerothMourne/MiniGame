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
        }
    }
    void startThreadForSync()
    {
        syncSock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);//初始化一个Scoket实习,采用UDP传输
        
        if (MGGlobalDataCenter.defaultCenter().isFrontRoler)
        {
            label.text += "\r\n self " + MGFoundtion.getInternIP() + ";serverip " + MGGlobalDataCenter.defaultCenter().serverIp;
            IPAddress clientAddress = IPAddress.Broadcast;
            syncIEP = new IPEndPoint(clientAddress, MGGlobalDataCenter.defaultCenter().SyncPort);//初始化一个发送广播和指定端口的网络端口实例
            syncSock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);//设置该scoket实例的发送形式
            InvokeRepeating("syncNetwork", 0.1f, 0.010f);
        }
        else
        {
            label.text += "\r\n self " + MGFoundtion.getInternIP() + ";serverip " + MGGlobalDataCenter.defaultCenter().serverIp;
            Debug.Log("serverIp="+MGGlobalDataCenter.defaultCenter().serverIp);
            IPAddress serverAddress = IPAddress.Any;
            syncIEP = new IPEndPoint(serverAddress, MGGlobalDataCenter.defaultCenter().SyncPort);//初始化一个侦听局域网内部所有IP和指定端口
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
        string receiveString = null,serverIp=MGGlobalDataCenter.defaultCenter().serverIp;
        while (true)
        {
            byte[] buffer = new byte[1024*1024];//设置缓冲数据流
            syncSock.ReceiveFrom(buffer, ref syncEP);//接收数据,并确把数据设置到缓冲流里面
            receiveString = Encoding.Unicode.GetString(buffer);
            isR = true;
            //Rstr = receiveString;
            //Rstr = receiveString;
            RstrArr = receiveString.Split(new char[]{'#'});
            //Rstr = RstrArr[0];
            if ((RstrArr[0] as string).CompareTo("192.168.123.1")==0)
            {
                Rstr = "123";
            }
            /*
            if (receiveString.Length > 0)
            {
                isReceiveSync = true;
                receivePosX = float.Parse(receiveString);
                
            }*/
        }
    }
    public void syncNetwork()//主机向客户端发送UDP包让客户端同步主机的数据
    {
        if (MGGlobalDataCenter.defaultCenter().isFrontRoler && MGGlobalDataCenter.defaultCenter().roleLater!=null)
        {
			Debug.Log("syncNetwork;"+MGGlobalDataCenter.defaultCenter().clientIP);
            //label.text = MGGlobalDataCenter.defaultCenter().clientIP + ";" + MGGlobalDataCenter.defaultCenter().serverIp;
            Vector3 roleLaterPos = MGGlobalDataCenter.defaultCenter().roleLater.transform.position;
            byte[] buffer = Encoding.Unicode.GetBytes(MGGlobalDataCenter.defaultCenter().serverIp + "#" + roleLaterPos.x.ToString());
            syncSock.SendTo(buffer, syncIEP);
        }
    }
    void Update()
    {
        if (isR)
        {
            isR = false;
            Debug.Log(Rstr);
            label.text += Rstr+"   ";
        }
        if (isReceiveSync)
        {
            if (MGGlobalDataCenter.defaultCenter().roleLater != null)
            {
				//Debug.Log("收到同步数据");
                isReceiveSync = false;
                label.text += "receiveString ";
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
