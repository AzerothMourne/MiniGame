using UnityEngine;
using System.Collections;

public class client : MonoBehaviour {
	
	private string IP = "10.66.208.191";
	private string clientIp;
	private string clientIpSplite;
	private Vector3 acceleration;
	public GameObject cube;
	private bool cubeInitialed = false;
	//Connet port 
	private int Port = 10000;
	
	void Awake()
	{
		clientIp = Network.player.ipAddress;
		string[] tmpArray = clientIp.Split('.');
		clientIpSplite = tmpArray[0] + "." + tmpArray[1] + "." + tmpArray[2] + ".";
	}
	
	
	void OnGUI()
	{
		switch (Network.peerType)
		{
		case NetworkPeerType.Disconnected:
			StartConnect();
			break;
		case NetworkPeerType.Server:
			break;
		case NetworkPeerType.Client:
			OnConnect();
			break;
		case NetworkPeerType.Connecting:
			break;
		}
	}
	
	
	void StartConnect()
	{

		if (GUILayout.Button("Connect Server"))
		{
			NetworkConnectionError error = Network.Connect(IP, Port);
			Debug.Log("connect status:" + error);	   
		}
	}
	
	void OnConnect()
	{
		if(!cubeInitialed)
		{
			Network.Instantiate(cube, transform.position, transform.rotation, 0);
			cubeInitialed = true;
		}
	}
}
