using UnityEngine;
using System.Collections;

public class addObserver : MonoBehaviour {

    public MGskillDrat drat;
    public GameObject role1;
    public MGskillDrat roadblock;
	// Use this for initialization
	void Start () {
	
	}
    public void onClick() {
        role1 = GameObject.Find("role1");
        MGNotificationCenter.defaultCenter().addObserver(this, observerSel, "testNotification");
        MGNotificationCenter.defaultCenter().addObserver(this, observerSel1, "testNotification");
        MGNotificationCenter.defaultCenter().addObserver(this, observerSel2, "testNotification");
        MGNotificationCenter.defaultCenter().addObserver(this, observerSel3, "testNotification");
        drat.createSkillSprite(new Vector3(role1.transform.position.x,role1.transform.position.y+role1.renderer.bounds.size.y/2,role1.transform.position.z));
        roadblock.createSkillSprite(role1.transform.position);
    }
    void LateUpdate()
    {
        
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
