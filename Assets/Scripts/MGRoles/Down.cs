using UnityEngine;
using System.Collections;

public class Down : MonoBehaviour {
	public Sprite upWardSprite;
    public GameObject upButton;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnMouseDown () {
        //print("Down OnMouseDown time : " + MGGlobalDataCenter.timestamp());
        if(MGGlobalDataCenter.defaultCenter().isHost==true)
            MGNotificationCenter.defaultCenter().postNotification("downToLine", null);
        else
            MGNotificationCenter.defaultCenter().postNotification("1downToLine", null);

		//按向下后调出向上按钮
        upButton.GetComponent<SpriteRenderer>().sprite = upWardSprite;
	}
}
