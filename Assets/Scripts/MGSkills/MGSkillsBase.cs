using UnityEngine;
using System.Collections;

public class MGSkillsBase : MonoBehaviour {

	public int skillId;
    public int skillCD,skillHoldLevel,skillGCD;

    /// <summary>
    /// 虚函数
    /// 在屏幕上创建技能精灵
    /// </summary>
    public virtual void createSkillSprite(Vector3 pos) { }
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
