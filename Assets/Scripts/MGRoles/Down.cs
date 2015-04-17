using UnityEngine;
using System.Collections;

public class Down : MonoBehaviour {
	public Sprite upWardSprite;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnMouseDown () {

        if(MGGlobalDataCenter.defaultCenter().isHost==1)
            MGNotificationCenter.defaultCenter().postNotification("downToLine", null);
        else
            MGNotificationCenter.defaultCenter().postNotification("1downToLine", null);

		//按向下后调出向上按钮
		GameObject.Find ("上").GetComponent<SpriteRenderer>().sprite=upWardSprite;
	}
}
