using UnityEngine;
using System.Collections;

public class MGNotification{
	public readonly string name;
	public readonly object objc;
	public readonly Hashtable userInfo;
}

public delegate void MGNotificationSelector(MGNotification notification);


public class MGNotificationCenter {

	private static MGNotificationCenter instance;
	private Hashtable nameHastable;//name sels hastable
	private ArrayList objcNamesSel;// objc contains names,name contains sel.

	private MGNotificationCenter(){
		this.nameHastable = new Hashtable ();
		this.objcNamesSel = new ArrayList ();
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

	}
	/// <summary>
	/// Removes the observer.
	/// </summary>
	/// <param name="observer">Observer.</param>
	public void removeObserver(object observer){

	}
	/// <summary>
	/// Removes the observer.
	/// </summary>
	/// <param name="observer">Observer.</param>
	/// <param name="name">Name.</param>
	public void removeObserver(object observer,string name){
		
	}
	/// <summary>
	/// Posts the notification.
	/// </summary>
	/// <param name="name">Name.</param>
	/// <param name="objc">Objc.</param>
	public void postNotification(string name,object objc){

	}
	/// <summary>
	/// Posts the notification.
	/// </summary>
	/// <param name="name">Name.</param>
	/// <param name="objc">Objc.</param>
	/// <param name="userInfo">User info.</param>
	public void postNotification(string name,object objc,Hashtable userInfo){
		
	}
}
