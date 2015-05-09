using UnityEngine;
using System.Collections;

public class MGScreenInfoController : MonoBehaviour {

    void Awake()
    {
        Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        MGGlobalDataCenter singleInstance = MGGlobalDataCenter.defaultCenter();
        singleInstance.pixelHight = camera.pixelHeight;
        singleInstance.pixelWidth = camera.pixelWidth;
        Vector3 rightTopPos = MGFoundtion.pixelToWroldPoint(singleInstance.pixelWidth, singleInstance.pixelHight);
        Vector3 leftBottomPos = MGFoundtion.pixelToWroldPoint(0, 0);

        singleInstance.leftBottomPos = leftBottomPos;
        singleInstance.rightTopPos = rightTopPos;

        singleInstance.screenBottomY = leftBottomPos.y;
        singleInstance.screenTopY = -1 * singleInstance.screenBottomY;
        singleInstance.screenLiftX = leftBottomPos.x;
        singleInstance.screenRightX = -1 * singleInstance.screenLiftX;

        Vector3 pos = MGFoundtion.pixelToWroldPoint(88f, 88f);
        singleInstance.NGUI_ButtonWidth = (pos.x - singleInstance.screenLiftX) * MGGlobalDataCenter.defaultCenter().UIScale;
    }
}
