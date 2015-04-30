using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class MGInitGameData : MonoBehaviour {
    private Socket syncSock;
    private IPEndPoint syncIEP;
    private EndPoint syncEP;
    private static MGInitGameData instance;
    private Thread loomThread;
    void Awake()
    {
        initGameData();
    }
	public void initGameData(){
        //MGGlobalDataCenter.defaultCenter().backToDefaultValues();

        Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        Debug.Log("pixelWidth:" + camera.pixelWidth + ",pixelHight" + camera.pixelHeight + ",pixelRect:" + camera.pixelRect);
        MGGlobalDataCenter singleInstance = MGGlobalDataCenter.defaultCenter();
        singleInstance.pixelHight = camera.pixelHeight;
        singleInstance.pixelWidth = camera.pixelWidth;
        //0,0在屏幕的左下方 即正常手机的左上
        Vector3 zeroPos = MGFoundtion.pixelToWroldPoint(camera.pixelWidth / 2.0f, camera.pixelHeight / 2.0f);
        Debug.Log("zeroPos:" + zeroPos);
        Vector3 leftTopPos = MGFoundtion.pixelToWroldPoint(0, singleInstance.pixelHight);
        Debug.Log("leftTopPos:" + leftTopPos);
        Vector3 rightTopPos = MGFoundtion.pixelToWroldPoint(singleInstance.pixelWidth, singleInstance.pixelHight);
        Debug.Log("rightTopPos:" + rightTopPos);
        Vector3 leftBottomPos = MGFoundtion.pixelToWroldPoint(0, 0);
        Debug.Log("leftBottomPos:" + leftBottomPos);
        Vector3 rightBottomPos = MGFoundtion.pixelToWroldPoint(singleInstance.pixelWidth, 0);
        Debug.Log("rightBottomPos:" + rightBottomPos);

        singleInstance.leftBottomPos = leftBottomPos;
        singleInstance.rightTopPos = rightTopPos;

        singleInstance.screenBottomY = leftBottomPos.y;
        singleInstance.screenTopY = -1 * singleInstance.screenBottomY;
        singleInstance.screenLiftX = leftBottomPos.x;
        singleInstance.screenRightX = -1 * singleInstance.screenLiftX;

        Vector3 pos = MGFoundtion.pixelToWroldPoint(88f, 88f);
        singleInstance.NGUI_ButtonWidth = (pos.x - singleInstance.screenLiftX) * MGGlobalDataCenter.defaultCenter().UIScale;
        Debug.Log(pos + "********" + singleInstance.NGUI_ButtonWidth);

        if (NetworkPeerType.Disconnected != Network.peerType)
        {
            startThreadForSync();
        }
    }
    void startThreadForSync()
    {
        syncSock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);//初始化一个Scoket实习,采用UDP传输
        if (MGGlobalDataCenter.defaultCenter().isHost)
        {
            syncIEP = new IPEndPoint(IPAddress.Broadcast, MGGlobalDataCenter.defaultCenter().mySocketPort);//初始化一个发送广播和指定端口的网络端口实例
            syncSock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);//设置该scoket实例的发送形式
            InvokeRepeating("syncNetwork", 0.05f, 0.032f);
        }
        else
        {
            syncIEP = new IPEndPoint(IPAddress.Any, MGGlobalDataCenter.defaultCenter().mySocketPort);//初始化一个侦听局域网内部所有IP和指定端口
            syncEP = (EndPoint)syncIEP;
            syncSock.Bind(syncIEP);//绑定这个实例
            //Run the action on a new thread
            loomThread = Loom.RunAsync(() =>
            {
                byte[] buffer = new byte[1024];//设置缓冲数据流
                string receiveString = null;
                while (true)
                {
                    syncSock.ReceiveFrom(buffer, ref syncEP);//接收数据,并确把数据设置到缓冲流里面
                    receiveString = Encoding.Unicode.GetString(buffer);
                    if (receiveString.Length > 0)
                    {
                        //Run some code on the main thread
                        Loom.QueueOnMainThread(() =>
                        {
                            if (MGGlobalDataCenter.defaultCenter().roleLater != null)
                            {
                                Vector3 pos = MGGlobalDataCenter.defaultCenter().roleLater.transform.position;
                                MGGlobalDataCenter.defaultCenter().roleLater.transform.position = new Vector3(float.Parse(receiveString), pos.y, pos.z);
                            }
                        });
                    }
                }
            });
            loomThread.IsBackground = true;
        }
    }
    public void syncNetwork()//主机向客户端发送UDP包让客户端同步主机的数据
    {
        if (MGGlobalDataCenter.defaultCenter().isHost && MGGlobalDataCenter.defaultCenter().roleLater!=null)
        {
            //Debug.Log("syncNetwork");
            Vector3 roleLaterPos = MGGlobalDataCenter.defaultCenter().roleLater.transform.position;
            byte[] buffer = Encoding.Unicode.GetBytes(roleLaterPos.x.ToString());
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
         
        CancelInvoke();
        MGGlobalDataCenter.defaultCenter().backToDefaultValues();
    }
    void OnDestroy()
    {
        destroyGameData();
    }
}
