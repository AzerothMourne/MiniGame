using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using LitJson;

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
    private Thread loomThread;
	public bool isSync;
    void Awake()
    {
        initGameData();
    }
	public void initGameData(){
        //MGGlobalDataCenter.defaultCenter().backToDefaultValues();
        if (NetworkPeerType.Disconnected != Network.peerType && isSync)
        {
			Debug.Log("startThreadForSync");
            startThreadForSync();
        }
    }
    void startThreadForSync()
    {
        syncSock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);//初始化一个Scoket实习,采用UDP传输
        if (MGGlobalDataCenter.defaultCenter().isHost)
        {
            syncIEP = new IPEndPoint(IPAddress.Broadcast, MGGlobalDataCenter.defaultCenter().SyncPort);//初始化一个发送广播和指定端口的网络端口实例
            syncSock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);//设置该scoket实例的发送形式
            InvokeRepeating("syncNetwork", 1f, 0.016f);
        }
        else
        {
            syncIEP = new IPEndPoint(IPAddress.Any, MGGlobalDataCenter.defaultCenter().SyncPort);//初始化一个侦听局域网内部所有IP和指定端口
            syncEP = (EndPoint)syncIEP;
            syncSock.Bind(syncIEP);//绑定这个实例
            //Run the action on a new thread
            loomThread = Loom.RunAsync(() =>
            {
                string receiveString = null;
                while (true)
                {
                    byte[] buffer = new byte[1024];//设置缓冲数据流
                    syncSock.ReceiveFrom(buffer, ref syncEP);//接收数据,并确把数据设置到缓冲流里面
                    receiveString = Encoding.ASCII.GetString(buffer);
                    if (receiveString.Length > 0)
                    {
                        //Run some code on the main thread
                        Loom.QueueOnMainThread(() =>
                        {
                            /*
                            Debug.Log(receiveString);
                            MGSyncMsgModel model = JsonMapper.ToObject<MGSyncMsgModel>(receiveString);
                            Debug.Log("model:" + model.roleLaterPosX+";"+model.gameTime);
                            */
                            if (MGGlobalDataCenter.defaultCenter().roleLater != null)
                            {
                                Vector3 pos = MGGlobalDataCenter.defaultCenter().roleLater.transform.position;
                                MGGlobalDataCenter.defaultCenter().roleLater.transform.position = new Vector3(float.Parse(receiveString), pos.y, pos.z);
                                //MGGlobalDataCenter.defaultCenter().totalGameTime = float.Parse(model.gameTime);
                            }
                        });
                    }
                }
            });
        }
    }
    public void syncNetwork()//主机向客户端发送UDP包让客户端同步主机的数据
    {
        if (MGGlobalDataCenter.defaultCenter().isHost && MGGlobalDataCenter.defaultCenter().roleLater!=null)
        {
            //Debug.Log("syncNetwork");
            Vector3 roleLaterPos = MGGlobalDataCenter.defaultCenter().roleLater.transform.position;
            /*
            MGSyncMsgModel model = new MGSyncMsgModel();
            model.roleLaterPosX = roleLaterPos.x.ToString();
            model.gameTime = MGGlobalDataCenter.defaultCenter().totalGameTime.ToString();
            byte[] buffer = Encoding.ASCII.GetBytes(JsonMapper.ToJson(model));
            */
            byte[] buffer = Encoding.ASCII.GetBytes(roleLaterPos.x.ToString());
            syncSock.SendTo(buffer, syncIEP);
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
        if (loomThread!=null)
        {
            loomThread.Abort();
            while (loomThread.ThreadState != System.Threading.ThreadState.Stopped)//必须等线程完全停止了，否则会出现冲突。  
            {
                Thread.Sleep(1000);
            }
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
