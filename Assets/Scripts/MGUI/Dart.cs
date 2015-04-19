using UnityEngine;
using System.Collections;

public class Dart : MonoBehaviour {

	private Animator dartAnim; 
	public bool isPressDartButton;
	int countDartFrame;

	// Use this for initialization
	void Start () {
		isPressDartButton = false;
		countDartFrame = 0;
	}

	void Awake() {
		dartAnim = GameObject.Find("role").GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {

		//触发放飞镖的动作
		dartAnim.SetBool ("pressDartButton", isPressDartButton);

		if (isPressDartButton == true) {
			countDartFrame += 1;
			print ("countFrame : " + countDartFrame);
		}
		if (countDartFrame == 30) {
			isPressDartButton = false;
			countDartFrame = 0;
		//	print ("update ********isPressDartButton : " + isPressDartButton);
		}

	}

	public void OnMouseDown () {
		print ("Dart OnMouseDown ");
		isPressDartButton = true;
		//print (".....OnMouseDown isPressDartButton : " + isPressDartButton);

        if (MGGlobalDataCenter.defaultCenter().isHost == true)
            MGNotificationCenter.defaultCenter().postNotification("useSkillsDart", null);
        else
            MGNotificationCenter.defaultCenter().postNotification("1useSkillsDart", null);
	}
}
