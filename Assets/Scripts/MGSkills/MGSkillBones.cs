using UnityEngine;
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
        transform.parent = releaseObject.transform;
		releaseObject.GetComponent<Jump> ().stateMask |= roleState.bone;
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
                    releaseObject.transform.position.y + (releaseObject.transform.localScale.y > 0 ? 1 : -1) * releaseObject.renderer.bounds.size.y / 2, 
                    releaseObject.transform.position.z);
                transform.position = pos;
            }
            if (holdTimer >= MGSkillBonesInfo.durationTime)
            {
                isEnded = true;
                holdTimer = 0;
                Debug.Log("releaseObject.stateMask:" + releaseObject.GetComponent<Jump>().stateMask);
				releaseObject.GetComponent<Jump>().stateMask &= ~roleState.bone;
                Debug.Log("releaseObject.stateMask:" + releaseObject.GetComponent<Jump>().stateMask);
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
		string strtmp = "dart(Clone)";
		int istrlen = strtmp.Length;
        if (other.tag == "frontSkill" && other.name != "beatback(Clone)") 
        {
            print("技能名：金钟罩。打在上面的是" + other.name + "，释放技能的是" + releaseSkillObjcName + ";gameobjc:" + other.gameObject);
			if(other.name.Substring(0,istrlen)== strtmp)
            	flyDuang(other, releaseObject);
        }
		if (other.name.Length>=istrlen && other.name.Substring(0,istrlen)== strtmp) {
			print("true");
			MGGlobalDataCenter.defaultCenter().isDartDefence = true;
		}
		strtmp = "roadblock(Clone)";
		istrlen = strtmp.Length;
		if (other.name.Length>=istrlen && other.name.Substring(0,istrlen)== strtmp) {
			print("true");
			MGGlobalDataCenter.defaultCenter().isRoadBlockDefence = true;
		}


    }
}
