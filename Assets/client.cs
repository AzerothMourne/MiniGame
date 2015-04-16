using UnityEngine;
using System.Collections;

public class client : MonoBehaviour {

    public GameObject cube;
	private string IP = "10.67.82.166";
	private string clientIp;
	private string clientIpSplite;
	private Vector3 acceleration;
	private bool cubeInitialed = false;
	//Connet port 
	private int Port = 25000;
	public UILabel logLabel;
	
	void Awake()
	{
		clientIp = Network.player.ipAddress;
		string[] tmpArray = clientIp.Split('.');
		clientIpSplite = tmpArray[0] + "." + tmpArray[1] + "." + tmpArray[2] + ".";
	}
    
	public void OnMouseDown(){

		switch (Network.peerType)
		{
		case NetworkPeerType.Disconnected:
			StartConnect();
			break;
		case NetworkPeerType.Server:
			logLabel.text += "\r\n" + "NetworkPeerType.Server:" ;
			break;
		case NetworkPeerType.Client:
			OnConnect();
			break;
		case NetworkPeerType.Connecting:
			logLabel.text += "\r\n" + "NetworkPeerType.Connecting:" ;
			break;
		}
	}
	
	void StartConnect()
	{
		NetworkConnectionError error = Network.Connect(IP, Port);
		Debug.Log("connect status:" + error);
		logLabel.text += "\r\n" + "StartConnect:" + error;
	}
	
	void OnConnect()
	{
		logLabel.text += "\r\n" + "OnConnect" ;
		if(!cubeInitialed)
		{
			logLabel.text += "\r\n" + "Network.Instantiate" ;
			Network.Instantiate(cube, transform.position, transform.rotation, 0);
			cubeInitialed = true;
		}
	}
}
