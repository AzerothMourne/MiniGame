using UnityEngine;
using System.Collections;

public class MGskillDrat : MGSkillsBase{

    public int speed = 3;
	// Use this for initialization
	void Start () {
        skillId = 1;
        skillCD = 2;
        skillGCD = 1;
        skillHoldLevel = 3;
	}
    public override void createSkillSprite(Vector3 pos)
    {
        base.createSkillSprite(pos);
        GameObject.Instantiate(this, pos, Quaternion.Euler(0, 0, -1));
    }
    public override void playSkillAnimation()
    {
        base.playSkillAnimation();
        transform.Translate(-Vector3.right * speed * Time.deltaTime);
    }
    public override void playSkillSound()
    {
        base.playSkillSound();
    }
	// Update is called once per frame
	void Update () {
        playSkillAnimation();
        if (transform.position.x < MGGlobalDataCenter.defaultCenter().screenLiftX)
        {
            this.Destroy();
        }
	}
    void Destroy()
    {
        DestroyImmediate(this.gameObject);
    }
	/*
	//同步gameobject的方法
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		if (stream.isWriting)
		{
			Vector3 pos = transform.position;
			stream.Serialize(ref pos);
		}
		else
		{
			Vector3 receivedPosition = Vector3.zero;
			stream.Serialize(ref receivedPosition);
			transform.position = receivedPosition;
		}
	}*/
}
