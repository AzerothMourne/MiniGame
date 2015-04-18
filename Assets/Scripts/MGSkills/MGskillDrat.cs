using UnityEngine;
using System.Collections;

public class MGskillDrat : MGSkillsBase{
    public string releaseSkillObjcName;
    public int speed;
    void Awake()
    {
        releaseSkillObjcName = null;
    }
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
            Destroy(this.gameObject);
        }
	}
    /*
    void Destroy()
    {
        Destroy(this.gameObject);
    }*/
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name != releaseSkillObjcName && releaseSkillObjcName != null)
        {
            print("被打中的是" + other.name+"，释放技能的是"+releaseSkillObjcName);
            Destroy(this.gameObject);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (releaseSkillObjcName == null)
        {
            releaseSkillObjcName = other.name;
        }
    }
}
