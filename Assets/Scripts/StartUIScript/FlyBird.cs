using UnityEngine;
using System.Collections;

public class FlyBird : MonoBehaviour
{
    public GameObject xiaoniao;    
    public float timeSum;
    public GameObject bird;
    public float height;
    public float width;
    public int count;
   
    void Start()
    {
        width = MGGlobalDataCenter.defaultCenter().screenLiftX;
        height = MGGlobalDataCenter.defaultCenter().screenTopY/2f;
        count = 0;
    }

    // Update is called once per frame
    void Update() 
    {        
        timeSum += Time.deltaTime;
        if(timeSum > 0 && count == 0) {         
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
            bird = GameObject.Instantiate(xiaoniao,
                new Vector3(Random.Range(width,width+3), Random.Range(height - 2, height), Random.Range(-10, 0)),
                Quaternion.Euler(0, 0, 0)) as GameObject;            
        }
    }
}
