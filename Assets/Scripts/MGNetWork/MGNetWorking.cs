using UnityEngine;
using System.Collections;
using LitJson;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class MGNetWorking : MonoBehaviour {

	[DllImport("__Internal")]
	private static extern void _findHost();
	[DllImport("__Internal")]
	private static extern void _createHost();
	[DllImport( "__Internal" )]
	private static extern void _sendMessageToPeer ( string msg);
	public static void findHost()
	{
        MGGlobalDataCenter.defaultCenter().isHost = false;
        if (MGGlobalDataCenter.defaultCenter().isNetworkViewEnable == true)
        {
            Network.Connect(MGGlobalDataCenter.defaultCenter().serverIp, MGGlobalDataCenter.defaultCenter().listenPort);
        }
		else if(Application.platform==RuntimePlatform.IPhonePlayer)
			_findHost();
	}

	public static void createHost()
	{
        MGGlobalDataCenter.defaultCenter().isHost = true;
        if (MGGlobalDataCenter.defaultCenter().isNetworkViewEnable == true)
        {
            Network.InitializeServer(MGGlobalDataCenter.defaultCenter().connecttions, MGGlobalDataCenter.defaultCenter().listenPort, false);
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
			_createHost();
	}
    /// <summary>
    /// 重载sendMessageToPeer支持unity提供的NetworkView，方便在局域网条件下跨平台
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="networkView"></param>
	public void sendMessageToPeer (string msg){
        if (MGGlobalDataCenter.defaultCenter().isNetworkViewEnable == true)
        {
            if (NetworkPeerType.Disconnected != Network.peerType)
            {
                print("sendMessageToPeer:" + msg);
                networkView.RPC("RPCReceiverMessageFromPeer", RPCMode.Others, msg);
            }
                
        }
        else
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
                MGNetWorking._sendMessageToPeer(msg);
        }	
	}
    public void sendMessageToPeer(string name,string msg)
    {
        if (MGGlobalDataCenter.defaultCenter().isNetworkViewEnable == true)
        {
            if (NetworkPeerType.Disconnected != Network.peerType)
            {
                print("sendMessageToPeer:" + msg);
                networkView.RPC(name, RPCMode.Others, msg);
            }

        }
        else
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
                MGNetWorking._sendMessageToPeer(msg);
        }
    }
    [RPC]
    public void RPCReceiverMessageFromPeer(string msg, NetworkMessageInfo info)
    {
        //刚从网络接收的数据的相关信息,会被保存到NetworkMessageInfo这个结构中  
        string sender = info.sender.ToString();
        //看脚本运行在什么状态下  
        NetworkPeerType status = Network.peerType;
        if (status != NetworkPeerType.Disconnected && sender!="-1")
        {
            receiverMessageFromPeer(msg);
        }
    }
    //同步gameobject的方法
    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        UILabel label = GameObject.Find("log").GetComponent<UIInput>().label;
        label.text += "\r\nOnSerializeNetworkView";
        if (stream.isWriting)
        {
            Vector3 roleVelocity = MGGlobalDataCenter.defaultCenter().role.rigidbody2D.velocity;
            Vector3 roleLaterVelocity = MGGlobalDataCenter.defaultCenter().roleLater.rigidbody2D.velocity;
            Vector3 rolePos = MGGlobalDataCenter.defaultCenter().role.transform.position;
            Vector3 roleLaterPos = MGGlobalDataCenter.defaultCenter().roleLater.transform.position;
            stream.Serialize(ref roleVelocity);
            stream.Serialize(ref roleLaterVelocity);
            stream.Serialize(ref rolePos);
            stream.Serialize(ref roleLaterPos);
        }
        else
        {
            Vector3 roleVelocity = Vector3.zero;
            Vector3 roleLaterVelocity = Vector3.zero;
            Vector3 rolePos = Vector3.zero;
            Vector3 roleLaterPos = Vector3.zero;
            stream.Serialize(ref roleVelocity);
            stream.Serialize(ref roleLaterVelocity);
            stream.Serialize(ref rolePos);
            stream.Serialize(ref roleLaterPos);
            MGGlobalDataCenter.defaultCenter().role.rigidbody2D.velocity = roleVelocity;
            MGGlobalDataCenter.defaultCenter().roleLater.rigidbody2D.velocity = roleLaterVelocity;
            MGGlobalDataCenter.defaultCenter().role.transform.position = rolePos;
            MGGlobalDataCenter.defaultCenter().roleLater.transform.position = roleLaterPos;
        }
    }
	public void receiverMessageFromPeer ( string msg)
	{
		print ("receiverMessageFromPeer:"+msg+";"+MGGlobalDataCenter.timestamp());
		MGMsgModel msgModel = JsonMapper.ToObject<MGMsgModel>(msg);
		MGNotificationCenter.defaultCenter().postNotification(msgModel.eventId,msgModel);
	}
    public Object Instantiate(UnityEngine.Object prefab,Vector3 position,Quaternion rotation,int group)
    {
        return Network.Instantiate(prefab, position, rotation, group);
    }
    
    void Awake()
    {
        Camera camera=this.GetComponent<Camera>();
        Debug.Log("pixelWidth:" + camera.pixelWidth + ",pixelHight" + camera.pixelHeight + ",pixelRect:" + camera.pixelRect);
        MGGlobalDataCenter singleInstance = MGGlobalDataCenter.defaultCenter();
        singleInstance.pixelHight = camera.pixelHeight;
        singleInstance.pixelWidth = camera.pixelWidth;
        //0,0在屏幕的左下方 即正常手机的左上
        Vector3 zeroPos = MGFoundtion.pixelToWroldPoint(camera.pixelWidth / 2.0f, camera.pixelHeight/2.0f);
        Debug.Log("zeroPos:"+zeroPos);
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
        singleInstance.NGUI_ButtonWidth = pos.x - singleInstance.screenLiftX;
        Debug.Log(pos + "********" + singleInstance.NGUI_ButtonWidth);
    }
}
