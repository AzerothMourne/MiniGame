using UnityEngine;
using System.Collections;
using LitJson;
public class MGSkillSprint : MGSkillsBase
{

    private MGNetWorking mgNetWorking;
    public int speed;
    private GameObject releaseRole;
    private float timer;
    private bool isEndedFreeze;
    public GameObject wordSprite;
    private int sprintLayer,wordMask;//wordMask用来做word字体出现的掩码，对应index出现后相应位置1，防止重复实例化
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
        MGGlobalDataCenter.defaultCenter().isBigSkilling = true;
        releaseRole = GameObject.Find("role1");
        UILabel label = GameObject.Find("log").GetComponent<UIInput>().label;
        label.text += "\r\nsprint start";
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

        mgNetWorking = GameObject.Find("NetWork").GetComponent<MGNetWorking>();
        this.releaseSkillObjectName = "role1";
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
        
    }
    public override void playSkillSound()
    {
        base.playSkillSound();
    }
    // Update is called once per frame
    void Update()
    {
        playSkillAnimation();
        if (!isEndedFreeze)
        {
            timer += Time.deltaTime;
            int length = wordDelayTimeArrayList.Length;
            for (int i = length - 1; i >= 0; --i)
            {
                //Debug.Log("time=" + timer.ToString() + ";wordDelayTimeArrayList[i]=" + wordDelayTimeArrayList[i]);
                int isInstance=(wordMask>>i)&1;
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
                    oneWord.layer = sprintLayer;
                    oneWord.GetComponent<SpriteRenderer>().sortingLayerID = 3;
                    oneWord.GetComponent<SpriteRenderer>().sprite = wordSpriteArrayList[i];
                    break;
                }
            }
            if (timer >= duration)
            {
                GameObject roleLater = GameObject.Find("role1");
                roleLater.GetComponent<RoleAnimController>().isSprint = true;
                roleLater.GetComponent<Animator>().SetBool("sprint", true);
                roleLater.GetComponent<SpriteRenderer>().material = new Material(Shader.Find("long/ghost"));
                GameObject[] backgroundList = GameObject.FindGameObjectsWithTag("Background");
                for (int i = 0; i < backgroundList.Length; ++i)
                {
                    backgroundList[i].GetComponent<bgnear>().speed += 20;
                }
                backgroundList = GameObject.FindGameObjectsWithTag("Road");
                for (int i = 0; i < backgroundList.Length; ++i)
                {
                    backgroundList[i].GetComponent<bgnear>().speed += 20;
                }

                MGMsgModel skillModel = new MGMsgModel();
                skillModel.eventId = SkillEffectEnum.sprint;
                skillModel.gameobjectName = this.releaseSkillObjectName;
                //发送给自己
                MGNotificationCenter.defaultCenter().postNotification(SkillEffectEnum.sprint, skillModel);

                MGGlobalDataCenter.defaultCenter().isBigSkilling = false;
                UILabel label = GameObject.Find("log").GetComponent<UIInput>().label;
                label.text += "\r\nsprint end";
                isEndedFreeze = true;
                Time.timeScale = 1f;
                GameObject releaseRole = GameObject.Find("role1");
                releaseRole.layer = 9;//gamelayer
                DestroyImmediate(this.m_cloneCamera, true);
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "frontSkill")
        {
            print("技能名：冲刺。打在上面的是" + other.name + "，释放技能的是" + this.releaseSkillObjectName + ";gameobjc:" + other.gameObject);
            MGGlobalDataCenter.defaultCenter().isBigSkilling = false;
            UILabel label = GameObject.Find("log").GetComponent<UIInput>().label;
            label.text += "\r\n Skill："+other.name;
            GameObject otherObject = other.gameObject;
            otherObject.GetComponent<Collider2D>().enabled = false;
            
            float otherObjectY = otherObject.transform.position.y;
            float releaseObjectY = releaseRole.transform.position.y + releaseRole.GetComponent<SpriteRenderer>().bounds.size.y / 2;
            label.text += "\r\n otherObjectY：" + otherObjectY + ",releaseObjectY=" + releaseObjectY;
            int angle = Random.Range(0, 25) + 135;//随机生成135到160度的角度
            Vector3 direction = otherObject.GetComponent<MGskillDrat>().direction;
            if (otherObjectY >= releaseObjectY)
            {
                //向上飞
                
                otherObject.GetComponent<MGskillDrat>().direction = new Vector3(direction.x, Mathf.Abs(direction.x) * Mathf.Tan(Mathf.PI * (angle - 90) / 180f),direction.z);
                label.text += "\r\n up:" + angle + ";" + otherObject.GetComponent<MGskillDrat>().direction;
            }
            else
            {
                //向下飞
                otherObject.GetComponent<MGskillDrat>().direction = new Vector3(direction.x, -1 * Mathf.Abs(direction.x) * Mathf.Tan(Mathf.PI * (angle - 90) / 180f), direction.z);
                label.text += "\r\n down:" + angle + ";" + otherObject.GetComponent<MGskillDrat>().direction;
            }
        }
    }
}
