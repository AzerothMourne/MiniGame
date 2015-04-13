using UnityEngine;
using System.Collections;

public class addObserver : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
    public void onClick() {
        MGNotificationCenter.defaultCenter().addObserver(this, observerSel, "testNotification");
        MGNotificationCenter.defaultCenter().addObserver(this, observerSel1, "testNotification");
        MGNotificationCenter.defaultCenter().addObserver(this, observerSel2, "testNotification");
        MGNotificationCenter.defaultCenter().addObserver(this, observerSel3, "testNotification");
    }
    void observerSel(MGNotification notification)
    {
        print("收到通知："+notification.name+notification);
    }
    void observerSel1(MGNotification notification)
    {
        print("收到通知：" + notification.name + notification);
    }
    void observerSel2(MGNotification notification)
    {
        print("收到通知：" + notification.name + notification);
    }
    void observerSel3(MGNotification notification)
    {
        print("收到通知：" + notification.name + notification);
    }
    public void onClickRemove()
    {
        MGNotificationCenter.defaultCenter().removeObserver(this, "testNotification");
    }
	// Update is called once per frame
	void Update () {
	
	}
}
