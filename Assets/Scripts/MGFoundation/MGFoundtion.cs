﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;
using System.Net.NetworkInformation;
public static class MGFoundtion  {
    public static string getInternIP(){
        string ip=null;
        NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
        foreach (NetworkInterface adapter in adapters)
        {
            if (adapter.Supports(NetworkInterfaceComponent.IPv4))
            {
                UnicastIPAddressInformationCollection uniCast = adapter.GetIPProperties().UnicastAddresses;
                if (uniCast.Count > 0)
                {
                    foreach (UnicastIPAddressInformation uni in uniCast)
                    {
                        //得到IPv4的地址。 AddressFamily.InterNetwork指的是IPv4
                        if (uni.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            ip = uni.Address.ToString();
                        }
                    }
                }
            }
        }
        return ip;
    }
	public static Vector3 WorldPointToNGUIPoint(Vector3 worldPos,Camera uiCamera){
		Vector3 pos = Camera.main.WorldToScreenPoint(worldPos);
		pos.z = 0f;   //z一定要为0.
		Vector3 returnPos = uiCamera.ScreenToWorldPoint (pos);
		return returnPos;
	}
    public static Vector3 pixelToWroldPoint(float pixelX,float pixelY)
    {
        Vector3 pos=Camera.main.ScreenToWorldPoint(new Vector3(pixelX,pixelY,0f));
        pos.z=0f;
        return pos;
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
