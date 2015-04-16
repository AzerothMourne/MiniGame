using UnityEngine;
using System.Collections;

public class MyNetworkTest : MonoBehaviour {

    public int connecttions = 10;
    public int listenPort = 8899;
    public UILabel clientLog, serverLog;
    public string ip = "127.0.0.1";
	void OnGUI()
    {
        if (NetworkPeerType.Disconnected == Network.peerType)
        {
            if (GUILayout.Button("创建服务器"))
            {
                NetworkConnectionError error = Network.InitializeServer(connecttions, listenPort, false);
                serverLog.text += "\r\n" + "Network.InitializeServer:" + error;
            }
            if (GUILayout.Button("连接服务器"))
            {
                NetworkConnectionError error = Network.Connect(ip, 8899);
                clientLog.text += "\r\n" + "Network.Connect:" + error;
            }
        }
        else if(Network.peerType == NetworkPeerType.Server)
        {
            GUILayout.Label("服务器创建完成.");
        }
        else if (Network.peerType == NetworkPeerType.Client)
        {
            GUILayout.Label("服务器连接成功");
        }
    }
}
