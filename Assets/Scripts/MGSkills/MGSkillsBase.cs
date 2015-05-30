using UnityEngine;
using System.Collections;

public static class SkillEnum
{
    public static string dart = "SkillEnum_dart";
    public static string roadblock = "SkillEnum_roadblock";
}
public static class MGSkillDartInfo
{
	public static string skillId = SkillEffectEnum.dart;
	public static int skillCD = 1;
	public static int skillHoldLevel = 10;
	public static int skillGCD = 0;
	public static float SkillEffectSpeed = 1f/8f;
	public static float durationTime = 0.1f;
}
public static class MGSkillRoadblockInfo
{
	public static string skillId = SkillEffectEnum.roadblock;
	public static float skillCD = 5f;
	public static int skillHoldLevel = 3;
	public static float skillGCD = 0.4f;
	public static float SkillEffectSpeed = 3f/16f;
	public static float durationTime = 0.1f;
}
public static class MGSkillBlinkInfo
{
	public static string skillId = SkillEffectEnum.blink;
	public static int skillCD = 8;
	public static int skillHoldLevel = 1;
	public static int skillGCD = 0;
	public static float SkillEffectSpeed = 1f / 4f;
	public static float durationTime = 0.1f;
}
public static class MGSkillBonesInfo
{
	public static string skillId = SkillEffectEnum.bones;
	public static float skillCD = 7.5f;
	public static int skillHoldLevel = 1;
	public static int skillGCD = 0;
	public static float SkillEffectSpeed = 1f / 4f;
	public static float durationTime = 1.5f;
}
public static class MGSkillSprintInfo
{
	public static string skillId = SkillEffectEnum.sprint;
	public static int skillCD = 10;
	public static int skillHoldLevel = 1;
	public static int skillGCD = 0;
	public static float SkillEffectSpeed = 3f / 8f;
	public static float durationTime = 2.0f;
}
public static class MGSkillBeatbackInfo
{
	public static string skillId = SkillEffectEnum.beatback;
	public static float skillCD = 6f;
	public static int skillHoldLevel = 1;
	public static int skillGCD = 0;
	public static float SkillEffectSpeed = 3f / 8f;
	public static float durationTime = 0.5f;
}

public static class SkillActEventEnum
{
    //技能事件
    public static string dart = "SkillActEventEnum_dart";
    public static string blink = "SkillActEventEnum_blink";
    public static string roadblock = "SkillActEventEnum_roadblock";
    public static string bones = "SkillActEventEnum_bones";
    public static string sprint = "SkillActEventEnum_sprint";
    public static string beatback = "SkillActEventEnum_beatback";
}
public class MGSkillsBase : MonoBehaviour {
    protected GameObject m_cloneCamera;
    public GameObject plane;
    public int bigSkillPlaneLayer=12;
    public string releaseSkillObjectName;
    public MGNetWorking mgNetWorking;
    /// <summary>
    /// 虚函数 
    /// 在屏幕上创建技能精灵
    /// </summary>
    public virtual Object createSkillSprite(Vector3 pos) { return null; }
    /// <summary>
    /// 虚函数 
    /// 在屏幕上创建技能精灵
    /// </summary>
    public virtual Object createSkillSprite(Vector3 pos, Quaternion rotation) { return null; }
    /// <summary>
    /// 虚函数
    /// 播放技能动画
    /// </summary>
    public virtual void playSkillAnimation() { }
    /// <summary>
    /// 虚函数
    /// 播放技能音效
    /// </summary>
    public virtual void playSkillSound() { }
    public void scaleAnimationFofBigSkill()
    {
        GameObject cameraObj = GameObject.Find("Main Camera");
        //将摄像机特效渲染层从主摄像机渲染中剔除
        cameraObj.GetComponent<Camera>().cullingMask -= (1 << bigSkillPlaneLayer);
        //复制摄像机对象，作为辅助渲染摄像机
        this.m_cloneCamera = GameObject.Instantiate(cameraObj) as GameObject;
        this.m_cloneCamera.transform.parent = cameraObj.transform;
        Camera component = this.m_cloneCamera.GetComponent<Camera>();
        component.clearFlags = CameraClearFlags.Nothing; //不剔除，保存深度等信息
        component.depth += 1;                            //效果摄像机深度高于主摄像机
        component.cullingMask = 1 << bigSkillPlaneLayer;  //辅助渲染摄像机只渲染指定的层
        //防止复制多个声音侦听
		AudioListener component2 = this.m_cloneCamera.GetComponent<AudioListener>();
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
        this.plane.layer = bigSkillPlaneLayer;

        GameObject releaseRole = GameObject.Find(releaseSkillObjectName);
        if (releaseRole)
        {
            releaseRole.layer = bigSkillPlaneLayer;
            int childCount = releaseRole.transform.childCount;
            Transform childObject = null;
            for (int i = 0; i < childCount; ++i)
            {
                childObject = releaseRole.transform.GetChild(i);
                if (childObject)
                {
                    childObject.gameObject.layer = bigSkillPlaneLayer;
                }
            }
        }
    }
    public void flyDuang(Collider2D other,GameObject releaseRole)
    {
        GameObject otherObject = other.gameObject;
        otherObject.GetComponent<Collider2D>().enabled = false;

        float otherObjectY = otherObject.transform.position.y;
        float releaseObjectY = releaseRole.transform.position.y + releaseRole.GetComponent<SpriteRenderer>().bounds.size.y / 2;
        int angle = Random.Range(0, 25) + 135;//随机生成135到160度的角度
        Vector3 direction = otherObject.GetComponent<MGskillDrat>().direction;
        if (otherObjectY >= releaseObjectY)
        {
            //向上飞
            otherObject.GetComponent<MGskillDrat>().direction = new Vector3(direction.x, Mathf.Abs(direction.x) * Mathf.Tan(Mathf.PI * (angle - 90) / 180f), direction.z);
        }
        else
        {
            //向下飞
            otherObject.GetComponent<MGskillDrat>().direction = new Vector3(direction.x, -1 * Mathf.Abs(direction.x) * Mathf.Tan(Mathf.PI * (angle - 90) / 180f), direction.z);
        }
    }
}
