using UnityEngine;
using System.Collections;

public class Upward : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}


	
	// Update is called once per frame
	public void OnMouseDown () {
		
		if(MGGlobalDataCenter.defaultCenter().isHost==1)
			MGNotificationCenter.defaultCenter().postNotification("upwardToLine", null);
		else
			MGNotificationCenter.defaultCenter().postNotification("1upwardToLine", null);
		/*print (".........is Down : " + GameObject.Find ("role").GetComponent<Jump> ().isDown);
		if (GameObject.Find ("role").GetComponent<Jump> ().isDown == 0) {
			GameObject.Find ("上").SetActive (false);
			print ("---------------");
			GameObject.Find ("按钮跳跃").SetActive (true);
		} else {
			GameObject.Find ("上").SetActive (true);
			GameObject.Find ("按钮跳跃").SetActive (false);
		} 
		*/

	}
}
