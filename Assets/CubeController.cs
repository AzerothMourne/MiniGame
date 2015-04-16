using UnityEngine;
using System.Collections;

public class CubeController : MonoBehaviour {
	private UILabel cubeLabel;
	void Start()
	{
        cubeLabel = GameObject.Find("cubeLabel").GetComponent<UILabel>();
	}

	void Update()
	{
		if(Network.isClient)
		{
			Vector3 acceleration = Input.acceleration;
            //networkView.RPC("ProcessMove", RPCMode.Others, "msg");
		}
		
		Vector3 moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		Vector3 cubescreenPos =  Camera.main.WorldToScreenPoint(transform.position);
		if (Input.GetMouseButton(0))
		{
			moveDir = new Vector3(Input.mousePosition.x - cubescreenPos.x, Input.mousePosition.y - cubescreenPos.y, 0f).normalized;
		}

		float speed = 15;
		transform.Translate(speed * moveDir * Time.deltaTime);
	}
	
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		if (stream.isWriting)
		{
			Vector3 pos = transform.position;
			stream.Serialize(ref pos);
		}
		else
		{
			Vector3 receivedPosition = Vector3.zero;
			stream.Serialize(ref receivedPosition);
			transform.position = receivedPosition;
		}
	}
	
    //接收请求的方法. 注意要在上面添加[RPC]  
    [RPC]
    void ProcessMove1(string msg, NetworkMessageInfo info)
    {
        cubeLabel.text += "\r\n"+msg;
    }  
}