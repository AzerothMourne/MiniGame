using UnityEngine;
using System.Collections;

public class CreatStartUI : MonoBehaviour {
	public GameObject startObj;
	public int width, height;
	public float x_k,y_k;

	void Awake(){
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
	}
	// Use this for initialization
	void Start () {
		x_k = MGGlobalDataCenter.defaultCenter ().pixelWidth / width;
		y_k = MGGlobalDataCenter.defaultCenter ().pixelHight / height;

		startObj = GameObject.Find("start");
		startObj.transform.localScale = new Vector3 (x_k,y_k,1);
		Debug.Log (startObj.transform.localScale);
	}

	// Update is called once per frame
	void Update () {
	
	}


}
