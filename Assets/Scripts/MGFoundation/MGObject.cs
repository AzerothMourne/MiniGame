using UnityEngine;
using System.Collections;

public class MGObject : MonoBehaviour {
    public bool isRendering;
    private float lastTime;
    private float curtTime;
    void Awake()
    {
        isRendering = false;
        lastTime = 0;
        curtTime = 0;
    }
    void OnWillRenderObject()
    {
        curtTime=Time.time;
    }
	// Use this for initialization
	void Start () {

	}
	// Update is called once per frame
	void Update () {
        isRendering = curtTime != lastTime ? true : false;
        lastTime = curtTime;
	}
}
