﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class MGFoundtion  {

	public static Vector3 WorldPointToNGUIPoint(Vector3 worldPos,Camera uiCamera){
		Debug.Log ("worldPos:"+worldPos);
		Vector3 pos = Camera.main.WorldToScreenPoint(worldPos);
		Debug.Log ("pos:"+pos);
		pos.z = 0f;   //z一定要为0.
		Vector3 returnPos = uiCamera.ScreenToWorldPoint (pos);
		Debug.Log ("returnPos:"+returnPos);
		return returnPos;
	}
	public static Vector3 NGUIPointToWroldPoint(Vector3 NGUIPos){
		Vector3 pos = UICamera.currentCamera.ScreenToWorldPoint (NGUIPos);
		return Camera.main.WorldToScreenPoint(pos);
	}
    /// <summary>
    /// 尝试将键和值添加到字典中：如果不存在，才添加；存在，不添加也不抛导常
    /// </summary>
    public static Dictionary<TKey, TValue> TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
    {
        if (dict.ContainsKey(key) == false)
            dict.Add(key, value);
        return dict;
    }

    /// <summary>
    /// 将键和值添加或替换到字典中：如果不存在，则添加；存在，则替换
    /// </summary>
    public static Dictionary<TKey, TValue> AddOrPeplace<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
    {
        dict[key] = value;
        return dict;
    }

    /// <summary>
    /// 获取与指定的键相关联的值，如果没有则返回输入的默认值
    /// </summary>
    public static TValue GetValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue defaultValue)
    {
        return dict.ContainsKey(key) ? dict[key] : defaultValue;
    }

    /// <summary>
    /// 向字典中批量添加键值对
    /// </summary>
    /// <param name="replaceExisted">如果已存在，是否替换</param>
    public static Dictionary<TKey, TValue> AddRange<TKey, TValue>(this Dictionary<TKey, TValue> dict, IEnumerable<KeyValuePair<TKey, TValue>> values, bool replaceExisted)
    {
        foreach (var item in values)
        {
            if (dict.ContainsKey(item.Key) == false || replaceExisted)
                dict[item.Key] = item.Value;
        }
        return dict;
    }


}
