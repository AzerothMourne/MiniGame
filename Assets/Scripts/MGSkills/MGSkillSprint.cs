using UnityEngine;
using System.Collections;
using LitJson;
public class MGSkillSprint : MGSkillsBase
{

    private MGNetWorking mgNetWorking;
    public string releaseSkillObjcName;
    public int speed;
    private GameObject roleLater, roleFront,m_cloneCamera;
    private float timer;
    private bool isEndedFreeze;
    public GameObject plane,wordSprite;
    private int sprintLayer;
    public Sprite[] wordSpriteArrayList;
    public Vector3[] wordPosArrayList;
    public float[] wordDelayTimeArrayList;
    public float duration;
    void Awake()
    {
        releaseSkillObjcName = null;
    }
    // Use this for initialization
    void Start()
    {
        UILabel label = GameObject.Find("log").GetComponent<UIInput>().label;
        label.text += "\r\nsprint start";
        timer = 0f;
        isEndedFreeze = false;
        Time.timeScale = 0.05f;
        duration *= Time.timeScale;
        sprintLayer = 12;
        int length = wordDelayTimeArrayList.Length;
        for (int i = length - 1; i >= 0; --i)
        {
            wordDelayTimeArrayList[i] *= Time.timeScale;
        }

        GameObject cameraObj = GameObject.Find("Main Camera");
        mgNetWorking = GameObject.Find("NetWork").GetComponent<MGNetWorking>();

        //将摄像机特效渲染层从主摄像机渲染中剔除
        cameraObj.GetComponent<Camera>().cullingMask -= (1 << sprintLayer);
        //复制摄像机对象，作为辅助渲染摄像机
        this.m_cloneCamera = GameObject.Instantiate(cameraObj) as GameObject;
        this.m_cloneCamera.transform.parent = cameraObj.transform;
        Camera component = this.m_cloneCamera.GetComponent<Camera>();
        component.clearFlags = CameraClearFlags.Nothing; //不剔除，保存深度等信息
        component.depth += 1;                            //效果摄像机深度高于主摄像机
        component.cullingMask = 1 << sprintLayer;  //辅助渲染摄像机只渲染指定的层
        //防止复制多个声音侦听
        AudioListener component2 = cameraObj.GetComponent<AudioListener>();
        if (component2 != null)
        {
            DestroyImmediate(component2);
        }
        //在辅助摄像机下方放置透明黑色遮罩，实现场景变暗效果
        this.plane = GameObject.Instantiate(this.plane) as GameObject;
        this.plane.transform.parent = this.m_cloneCamera.transform;
        this.plane.transform.localPosition = new Vector3(0f, 0f, 1f);
        this.plane.transform.rotation = Quaternion.Euler(90, 0, 0);
        this.plane.transform.localScale = new Vector3(2f, 1f, 1f);
        this.plane.layer = sprintLayer;

        GameObject releaseRole = GameObject.Find("role1");
        releaseRole.layer = sprintLayer;
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
                if (timer >= wordDelayTimeArrayList[i])
                {
                    //出现一个字
                    Debug.Log("此处应该出现一个字");
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
                UILabel label = GameObject.Find("log").GetComponent<UIInput>().label;
                label.text += "\r\nsprint end";
                isEndedFreeze = true;
                timer = 0;
                Time.timeScale = 1f;
                GameObject releaseRole = GameObject.Find("role1");
                releaseRole.layer = 9;//gamelayer
                DestroyImmediate(this.m_cloneCamera, true);
                Destroy(this.gameObject);
            }
        }
    }
}
