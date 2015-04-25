using UnityEngine;
using System.Collections;

public class stop : MGSkillsBase
{
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void stopClick()
    {
        scaleAnimationFofBigSkill();
        Time.timeScale = 0;
    }
}
