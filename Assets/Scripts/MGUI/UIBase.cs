using UnityEngine;
using System.Collections;

public class UIBase : MonoBehaviour {
    protected bool isCD = false;
    protected bool holdCD = false;
    protected UISprite cdBack;
    public bool direction;// true 顺时针，false逆时针
    public bool addOrDec;// true 添加,false 减少
    protected GameObject cdBackObject;

}
