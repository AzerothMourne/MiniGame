using UnityEngine;
using System.Collections;

public static class SkillEnum
{
    public static string dart = "dart";
    public static string roadblock = "roadblock";
    public static string bones = "bones";
    public static string blink = "blink";
}
public static class MGSkillDartInfo
{
    public static string skillId = SkillEnum.dart;
    public static int skillCD = 2;
    public static int skillHoldLevel = 3;
    public static int skillGCD = 0;
    public static int SkillEffectSpeed = 10;
    public static float durationTime = 0.1f;
}
public static class MGSkillRoadblockInfo
{
    public static string skillId = SkillEnum.roadblock;
    public static int skillCD = 1;
    public static int skillHoldLevel = 1;
    public static int skillGCD = 0;
    public static int SkillEffectSpeed = 10;
    public static float durationTime = 0.1f;
}
public static class MGSkillBlinkInfo
{
    public static string skillId = SkillEnum.blink;
    public static int skillCD = 3;
    public static int skillHoldLevel = 1;
    public static int skillGCD = 0;
    public static int SkillEffectSpeed = 10;
    public static float durationTime = 0.1f;
}
public class MGSkillsBase : MonoBehaviour {

    public float posY = -10000;
    /// <summary>
    /// 虚函数 
    /// 在屏幕上创建技能精灵
    /// </summary>
    public virtual void createSkillSprite(Vector3 pos) { }
    /// <summary>
    /// 虚函数 
    /// 在屏幕上创建技能精灵
    /// </summary>
    public virtual void createSkillSprite(Vector3 pos, Quaternion rotation) { }
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
