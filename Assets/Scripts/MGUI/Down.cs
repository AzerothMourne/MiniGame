using UnityEngine;
using System.Collections;

public class Down : MonoBehaviour {
	public Sprite upWardSprite;
<<<<<<< HEAD:Assets/Scripts/MGRoles/Down.cs
=======
    public GameObject upButton;
>>>>>>> CEQ_DEV:Assets/Scripts/MGUI/Down.cs
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
<<<<<<< HEAD:Assets/Scripts/MGRoles/Down.cs
		/*print (".........is Down : " + GameObject.Find ("role").GetComponent<Jump> ().isDown);
		if (GameObject.Find ("role").GetComponent<Jump> ().isDown == 0) {
			GameObject.Find ("上").SetActive (false);
			GameObject.Find ("按钮跳跃").SetActive (true);
		} else {
			GameObject.Find ("上").SetActive (true);
			GameObject.Find ("按钮跳跃").SetActive (false);
		}
		*/
		GameObject.Find ("上").GetComponent<SpriteRenderer>().sprite=upWardSprite;
=======

		//按向下后调出向上按钮
        upButton.GetComponent<SpriteRenderer>().sprite = upWardSprite;
>>>>>>> CEQ_DEV:Assets/Scripts/MGUI/Down.cs
	}
}
