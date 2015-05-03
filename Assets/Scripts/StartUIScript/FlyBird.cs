using UnityEngine;
using System.Collections;

public class FlyBird : MonoBehaviour
{
    public GameObject xiaoniao;
    public GameObject bird1;
    private float screenlen;
    public float timeSum;
   
    public int i;
    public GameObject[] birds;
    //public float 
    // Use this for initialization
    public float topY;
    public float bottomY;
    public float rightX;
    public float leftX;

    public float height;
    public float width;
    public int count;


    public Camera nguiCamera;
    void Start()
    {
        width = MGGlobalDataCenter.defaultCenter().screenLiftX;
        height = MGGlobalDataCenter.defaultCenter().screenTopY/2f;
        count = 0;

    }

    // Update is called once per frame
    void Update() {
        
        timeSum += Time.deltaTime;
        if(timeSum > 0 && count == 0) {
           print("creat*******");
           CreatBirds();
           count += 1;
       
        }
       if (timeSum > 8 ) {
            CreatBirds();
            timeSum = 0;
        }
	}

    public void CreatBirds() {
        for (int j = 0; j < 10; j++) {
            print("birds**");
            birds[i] = GameObject.Instantiate(xiaoniao,
                new Vector3(Random.Range(width,width+3), Random.Range(height - 2, height), Random.Range(-10, 0)),
                Quaternion.Euler(0, 0, 0)) as GameObject;
            j = (j++) % 10;
        }
    }

}
