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
	}
}
