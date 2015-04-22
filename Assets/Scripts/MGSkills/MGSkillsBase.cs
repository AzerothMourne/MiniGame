using UnityEngine;
using System.Collections;


public static class MGSkillDartInfo
{
    public static string skillId = SkillEffectEnum.dart;
    public static int skillCD = 1;
    public static int skillHoldLevel = 10;
    public static int skillGCD = 0;
    public static int SkillEffectSpeed = 10;
    public static float durationTime = 0.1f;
}
public static class MGSkillRoadblockInfo
{
    public static string skillId = SkillEffectEnum.roadblock;
    public static int skillCD = 1;
    public static int skillHoldLevel = 1;
    public static int skillGCD = 0;
    public static int SkillEffectSpeed = 10;
    public static float durationTime = 0.1f;
}
public static class MGSkillBlinkInfo
{
    public static string skillId = SkillEffectEnum.blink;
    public static int skillCD = 3;
    public static int skillHoldLevel = 1;
    public static int skillGCD = 0;
    public static float SkillEffectSpeed = 1f/4f;
    public static float durationTime = 0.1f;
}
public static class MGSkillBonesInfo
{
    public static string skillId = SkillEffectEnum.bones;
    public static int skillCD = 3;
    public static int skillHoldLevel = 1;
    public static int skillGCD = 0;
    public static float SkillEffectSpeed = 1f / 4f;
    public static float durationTime = 2.0f;
}
public class MGSkillsBase : MonoBehaviour {

    public float posY = -10000;
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
    
}
