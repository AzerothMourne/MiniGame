using UnityEngine;
using System.Collections;

public class CubeController : MonoBehaviour {
	private GUIText accelText;
	
	void Start()
	{
		accelText = GameObject.FindGameObjectWithTag("AccelTip").GetComponent<GUIText>() as GUIText;
		accelText.text = "";
	}
	
	void Update()
	{
		if(Network.isClient)
		{
			Vector3 acceleration = Input.acceleration;
			accelText.text = "" + acceleration;
			networkView.RPC("UpdateAcceleration", RPCMode.Others, acceleration);
		}
		
		Vector3 moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		Vector3 cubescreenPos =  Camera.main.WorldToScreenPoint(transform.position);
		if (Input.GetMouseButton(0))
		{
			moveDir = new Vector3(Input.mousePosition.x - cubescreenPos.x, Input.mousePosition.y - cubescreenPos.y, 0f).normalized;
		}
		Debug.Log("moveDir: " + moveDir);
		float speed = 5;
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
	
	[RPC]
	void UpdateAcceleration(Vector3 acceleration)
	{
		accelText.text = "" + acceleration;
	}
	
}