using UnityEngine;
using System.Collections;

public class MGSkillBeatback : MGSkillsBase
{
    public int speed;
    private GameObject roleLater, roleFront;
    private float timer;
    private bool isEndedFreeze;
    public GameObject wordSprite;
    private int wordMask;//wordMask用来做word字体出现的掩码，对应index出现后相应位置1，防止重复实例化
    public Sprite[] wordSpriteArrayList;
    public Vector3[] wordPosArrayList;
    public float[] wordDelayTimeArrayList;
    public float duration;

    void Awake()
    {
        this.releaseSkillObjectName = null;
    }
    // Use this for initialization
    void Start()
    {
        speed = 40;
        MGGlobalDataCenter.defaultCenter().isBigSkilling = true;
        UILabel label = GameObject.Find("log").GetComponent<UIInput>().label;
        label.text += "\r\nbeatback start";
        timer = 0f;
        isEndedFreeze = false;
        Time.timeScale = 0.05f;
        duration *= Time.timeScale;
        wordMask = 0;
        int length = wordDelayTimeArrayList.Length;
        for (int i = length - 1; i >= 0; --i)
        {
            wordDelayTimeArrayList[i] *= Time.timeScale;
        }

        this.releaseSkillObjectName = "role";
        roleLater = GameObject.Find("role1");
        roleFront = GameObject.Find("role");
        transform.parent = roleFront.transform;
        this.gameObject.layer = 9;
        base.scaleAnimationFofBigSkill();
    }
    public override Object createSkillSprite(Vector3 pos)
    {
        base.createSkillSprite(pos);
        return GameObject.Instantiate(this, pos, Quaternion.Euler(0, 0, 0));
    }
    public override void playSkillAnimation()
    {
        base.playSkillAnimation();
        transform.Translate(Vector3.left * speed * Time.deltaTime/Time.timeScale);
    }
    public override void playSkillSound()
    {
        base.playSkillSound();
    }
    // Update is called once per frame
    void Update()
    {
        if (!isEndedFreeze)
        {
            timer += Time.deltaTime;
            int length = wordDelayTimeArrayList.Length;
            for (int i = length - 1; i >= 0; --i)
            {
                //Debug.Log("time=" + timer.ToString() + ";wordDelayTimeArrayList[i]=" + wordDelayTimeArrayList[i]);
                int isInstance = (wordMask >> i) & 1;
                if (isInstance == 0 && timer >= wordDelayTimeArrayList[i])
                {
                    //出现一个字
                    Debug.Log("此处应该出现一个字:" + wordSpriteArrayList[i]);
                    wordMask += 1 << i;
                    GameObject oneWord = GameObject.Instantiate(this.wordSprite) as GameObject;
                    oneWord.transform.parent = this.plane.transform;
                    oneWord.transform.position = wordPosArrayList[i];
                    oneWord.transform.rotation = Quaternion.Euler(0, 0, 0);
                    oneWord.transform.localScale = new Vector3(0.5f, 1f, 1f);
                    oneWord.layer = bigSkillPlaneLayer;
                    oneWord.GetComponent<SpriteRenderer>().sortingLayerID = 3;
                    oneWord.GetComponent<SpriteRenderer>().sprite = wordSpriteArrayList[i];
                    break;
                }
            }
            if ((wordMask & 0xF) == 0xF)
            {
                duration /= Time.timeScale;
                Time.timeScale = 1;
                playSkillAnimation();
            }
            if (timer >= duration)
            {
                DestroySelf();
            }
        }
    }
    public void DestroySelf()
    {
        MGGlobalDataCenter.defaultCenter().isBigSkilling = false;
        UILabel label = GameObject.Find("log").GetComponent<UIInput>().label;
        label.text += "\r\nbeatback end";
        isEndedFreeze = true;
        timer = 0;
        Time.timeScale = 1f;
        GameObject releaseRole = GameObject.Find("role");
        releaseRole.layer = 9;//gamelayer
        DestroyImmediate(this.m_cloneCamera, true);
        Destroy(this.gameObject);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "role1")
        {
            print("技能名：击退。被击中的是" + other.name + "，释放技能的是" + this.releaseSkillObjectName + ";gameobjc:" + other.gameObject);
            MGGlobalDataCenter.defaultCenter().isBigSkilling = false;
            UILabel label = GameObject.Find("log").GetComponent<UIInput>().label;
            label.text += "\r\n Skill：" + other.name;
            //发送给自己
            MGMsgModel skillModel = new MGMsgModel();
            skillModel.eventId = SkillEffectEnum.beatback;
            skillModel.gameobjectName = other.name;
            MGNotificationCenter.defaultCenter().postNotification(SkillEffectEnum.beatback, skillModel);
        }
    }
}
