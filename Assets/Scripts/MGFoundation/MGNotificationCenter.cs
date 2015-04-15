using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MGNotification{
	public readonly string name;
	public readonly object objc;
    public readonly Dictionary<object,object> userInfo;
    public MGNotification(string _name, object _objc, Dictionary<object, object> _userInfo)
    {
        this.name = _name;
        this.objc = _objc;
        this.userInfo = _userInfo;
    }
}

public delegate void MGNotificationSelector(MGNotification notification);


public class MGNotificationCenter  {
    
	private static MGNotificationCenter instance;
    private Dictionary<string, List<MGNotificationSelector>> nameHastable;//name sels hastable
    private Dictionary<object, Dictionary<string, List<MGNotificationSelector>>> objcNamesSel;// objc contains names,name contains sel.
	/// <summary>
	/// Initializes a new instance of the <see cref="MGNotificationCenter"/> class.
	/// </summary>
	private MGNotificationCenter(){
        this.nameHastable = new Dictionary<string, List<MGNotificationSelector>>();
        this.objcNamesSel = new Dictionary<object, Dictionary<string, List<MGNotificationSelector>>>();
	}
	/// <summary>
	/// Get the signle instance
	/// </summary>
	/// <returns>MGNotificationCenter instance</returns>
	public static MGNotificationCenter defaultCenter(){
		if (instance == null) {
			instance = new MGNotificationCenter();
		}
		return instance;
	}
    
	/// <summary>
	/// Adds the observer.
	/// </summary>
	/// <param name="observer">Observer.</param>
	/// <param name="sel">selector.</param>
	/// <param name="name">nitification Name.</param>
	public void addObserver(object observer,MGNotificationSelector sel,string name){
		if (observer==null || sel==null || name==null)
			return;
        if (!this.nameHastable.ContainsKey(name))
        {

        }
        List<MGNotificationSelector> selList = MGFoundtion.GetValue<string, List<MGNotificationSelector>>(this.nameHastable, name, null);
		if (selList == null) 
        {
			selList = new List<MGNotificationSelector>();
		}
        if (selList.Contains(sel)) return;
		selList.Add(sel);
        //Debug.Log("after add sel:"+selList.Count);
        MGFoundtion.AddOrPeplace<string, List<MGNotificationSelector>>(this.nameHastable, name, selList);
        Dictionary<string, List<MGNotificationSelector>> nameListDic = MGFoundtion.GetValue<object, Dictionary<string, List<MGNotificationSelector>>>(this.objcNamesSel, observer, null);
        if (nameListDic == null)
        {
            nameListDic = new Dictionary<string, List<MGNotificationSelector>>();
		}
        List<MGNotificationSelector> obj_name_selList = MGFoundtion.GetValue<string, List<MGNotificationSelector>>(nameListDic, name, null);
		if (obj_name_selList == null) {
            obj_name_selList = new List<MGNotificationSelector>();
		}
		obj_name_selList.Add (sel);
        MGFoundtion.AddOrPeplace<string, List<MGNotificationSelector>>(nameListDic, name, obj_name_selList);
        MGFoundtion.AddOrPeplace<object, Dictionary<string, List<MGNotificationSelector>>>(this.objcNamesSel, observer, nameListDic);
        //Debug.Log("objcNamesSel count:" + this.objcNamesSel.Count);
	}
	/// <summary>
	/// Removes the observer.
	/// </summary>
	/// <param name="observer">Observer.</param>
	public void removeObserver(object observer){
        Dictionary<string, List<MGNotificationSelector>> nameListDic = MGFoundtion.GetValue<object, Dictionary<string, List<MGNotificationSelector>>>(this.objcNamesSel, observer, null);
        if (nameListDic == null) return;
        foreach (KeyValuePair<string, List<MGNotificationSelector>> pair in nameListDic)
        {
            this.removeObserver(pair.Value, pair.Key);
        }
	}
	/// <summary>
	/// Removes the observer.
	/// </summary>
	/// <param name="observer">Observer.</param>
	/// <param name="name">Name.</param>
	public void removeObserver(object observer,string name){
        Dictionary<string, List<MGNotificationSelector>> nameListDic = MGFoundtion.GetValue<object, Dictionary<string, List<MGNotificationSelector>>>(this.objcNamesSel, observer, null);
        if (nameListDic == null) return;
       // Debug.Log("123123");
        this.removeObserver(nameListDic[name], name);
        nameListDic.Remove(name);
	}
    private void removeObserver(List<MGNotificationSelector> selList, string name)
    {
        if (selList == null) return;
        List<MGNotificationSelector>  notiSelList, needDelList;
        needDelList = new List<MGNotificationSelector>();
        notiSelList = MGFoundtion.GetValue<string, List<MGNotificationSelector>>(this.nameHastable, name, null);

        if (notiSelList == null) return;
        //Debug.Log("before remove:"+selList.Count);

        foreach (MGNotificationSelector eachSel in notiSelList)
        {
            foreach (MGNotificationSelector delSel in selList)
            {
                if (delSel == eachSel)
                    needDelList.Add(delSel);
            }
        }
        foreach (MGNotificationSelector needDelSel in needDelList)
        {
            notiSelList.Remove(needDelSel);
        }
       // Debug.Log("after remove:"+selList.Count);
        needDelList.Clear();
    }
	/// <summary>
	/// Posts the notification.
	/// </summary>
	/// <param name="name">Name.</param>
	/// <param name="objc">Objc.</param>
	public void postNotification(string name,object objc){
        this.postNotification(name,objc,null);
	}
	/// <summary>
	/// Posts the notification.
	/// </summary>
	/// <param name="name">Name.</param>
	/// <param name="objc">Objc.</param>
	/// <param name="userInfo">User info.</param>
	public void postNotification(string name,object objc,Dictionary<object,object> userInfo){
        List<MGNotificationSelector> selList =  MGFoundtion.GetValue<string, List<MGNotificationSelector>>(this.nameHastable, name, null);
        if (selList == null) return;
        foreach (MGNotificationSelector sel in selList)
        {
            sel(new MGNotification(name, objc, userInfo));
        }
	}
}
