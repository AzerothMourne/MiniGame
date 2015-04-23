﻿using UnityEngine;
using System.Collections;
using LitJson;
public class MGSkillBones : MGSkillsBase 
{
    public string releaseSkillObjcName;
    private float holdTimer;
    private bool isEnded;
    private GameObject releaseObject;
    void Awake()
    {
        releaseSkillObjcName = null;
    }
    // Use this for initialization
    void Start()
    {
        holdTimer = 0f;
        isEnded = false;
        releaseObject = GameObject.Find(releaseSkillObjcName);
        if (!releaseObject) releaseObject = GameObject.Find("role1");
    }
    public override Object createSkillSprite(Vector3 pos)
    {
        base.createSkillSprite(pos);
        return GameObject.Instantiate(this, pos, Quaternion.Euler(0, 0, 0));
    }
    public override void playSkillAnimation()
    {
        base.playSkillAnimation();
        if (!isEnded)
        {
            holdTimer += Time.deltaTime;
            if (releaseObject) { 
                Vector3 pos = new Vector3(releaseObject.transform.position.x + 3 * releaseObject.renderer.bounds.size.x / 16, 
                    releaseObject.transform.position.y + (releaseObject.GetComponent<Jump>().isDown == 0 ? 1 : -1) * releaseObject.renderer.bounds.size.y / 2, 
                    releaseObject.transform.position.z);
                transform.position = pos;
            }
            if (holdTimer >= MGSkillBonesInfo.durationTime)
            {
                isEnded = true;
                holdTimer = 0;
                Destroy(this.gameObject);
            }
        }
    }
    public override void playSkillSound()
    {
        base.playSkillSound();
    }
    // Update is called once per frame
    void Update()
    {
        playSkillAnimation();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "MGSkill")
            return;
        print("技能名：金钟罩。打在上面的是" + other.name + "，释放技能的是" + releaseSkillObjcName + ";gameobjc:" + other.gameObject);
        Destroy(GameObject.Find(other.name));
    }
}