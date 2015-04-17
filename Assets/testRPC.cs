using UnityEngine;
using System.Collections;

public class testRPC : MonoBehaviour {
    private UILabel cubeLabel;
	// Use this for initialization
	void Start () {
        cubeLabel = GameObject.Find("cubeLabel").GetComponent<UILabel>();
	}
    public void onClick()
    {
        if(NetworkPeerType.Disconnected != Network.peerType)
            networkView.RPC("ProcessMove", RPCMode.Others, "msg");
    }
	// Update is called once per frame
	void Update () {
	
	}
    [RPC]
    void ProcessMove(string msg, NetworkMessageInfo info)
    {
        cubeLabel.text += "\r\n" + msg;
    }  
}
