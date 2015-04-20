using UnityEngine;
using System.Collections;

public class MyNetworkTest : MonoBehaviour {

    public int connecttions = 10;
    public int listenPort = 8899;
    public UILabel clientLog, serverLog;
    private Vector3 acceleration;
    public GameObject cube;
    private bool cubeInitialed = false;
	private string ip = "192.168.23.10";

	void OnGUI()
    {
        if (NetworkPeerType.Disconnected == Network.peerType)
        {
            if (GUILayout.Button("创建服务器"))
            {
                NetworkConnectionError error = Network.InitializeServer(connecttions, listenPort, false);
				serverLog.text += "\r\n" + "Network.InitializeServer:ip="+ip +";"+ error;
            }
            if (GUILayout.Button("连接服务器"))
            {
                NetworkConnectionError error = Network.Connect(ip, 8899);
				clientLog.text += "\r\n" + "Network.Connect:hostIp=" +ip+";"+error;
            }
        }
        else if(Network.peerType == NetworkPeerType.Server)
        {
            GUILayout.Label("服务器创建完成.");
        }
        else if (Network.peerType == NetworkPeerType.Client)
        {
            GUILayout.Label("服务器连接成功");
            Network.sendRate = 60;
            //clientLog.renderer.enabled = false;
            //serverLog.renderer.enabled = false;
            OnConnect();
        }
    }
    void OnConnect()
    {
        
        if (!cubeInitialed)
        {
            Network.Instantiate(cube, transform.position, transform.rotation, 0);
            cubeInitialed = true;
        }
    }
    /*
    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        // Always send transform (depending on reliability of the network view) 
        if (stream.isWriting)
        {
            MGMsgModel msgModel = new MGMsgModel();
            msgModel.eventId = "firstJump";
            msgModel.timestamp = MGGlobalDataCenter.timestamp();
            Vector3 pos = transform.localPosition;
            Quaternion rot = transform.localRotation;
            stream.Serialize(ref pos);
            stream.Serialize(ref rot);
        }
        // When receiving, buffer the information 
        else
        {
            // Receive latest state information 
            Vector3 pos = Vector3.zero;
            Quaternion rot = Quaternion.identity;
            stream.Serialize(ref pos);
            stream.Serialize(ref rot);
        }
    }*/
}
