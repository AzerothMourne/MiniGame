using UnityEngine;
using System.Collections;

public class MGSkillRoadblock : MGSkillsBase{

    public string releaseSkillObjcName;
    public int speed;

    void Awake()
    {
        releaseSkillObjcName = null;
    }
    // Use this for initialization
    void Start()
    {

    }
    public override void createSkillSprite(Vector3 pos)
    {
        base.createSkillSprite(pos);
        GameObject.Instantiate(this, pos, Quaternion.Euler(0, 0, 0));
    }
    public override void createSkillSprite(Vector3 pos, Quaternion rotation)
    {
        base.createSkillSprite(pos, rotation);
        GameObject.Instantiate(this, pos, rotation);
    }
    public override void playSkillAnimation()
    {
        base.playSkillAnimation();
        if (posY == -10000)
        {
            posY = transform.position.y;
        }
        transform.Translate(-Vector3.right * speed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x,posY,transform.position.z);
    }
    public override void playSkillSound()
    {
        base.playSkillSound();
    }
    // Update is called once per frame
    void Update()
    {
        playSkillAnimation();
        if (transform.position.x < MGGlobalDataCenter.defaultCenter().screenLiftX)
        {
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player")
            return;
        if (other.name != releaseSkillObjcName && releaseSkillObjcName != null)
        {
            print("技能名：路障。被打中的是" + other.name + "，释放技能的是" + releaseSkillObjcName);
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
