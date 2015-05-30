using UnityEngine;
using System.Collections;

public class overUI : MonoBehaviour {
     
	// Use this for initialization
	void Start () {
        GameObject[] overUI = GameObject.FindGameObjectsWithTag("overUI");
        Debug.Log(MGGlobalDataCenter.defaultCenter().overSenceUIName);
        if (MGGlobalDataCenter.defaultCenter().isFrontRoler == false)
        {
            if (MGGlobalDataCenter.defaultCenter().overSenceUIName == "victoryFrontGameUI")
            {
                MGGlobalDataCenter.defaultCenter().overSenceUIName = "failLaterGameUI";
				MGGlobalDataCenter.defaultCenter().isDefeat = true;
            }
            else if (MGGlobalDataCenter.defaultCenter().overSenceUIName == "failFrontGameUI")
            {
                MGGlobalDataCenter.defaultCenter().overSenceUIName = "victoryLaterGameUI";
				MGGlobalDataCenter.defaultCenter().isVictory = true;
            }
        }
		else if(MGGlobalDataCenter.defaultCenter().overSenceUIName == "victoryFrontGameUI" )
		{
			MGGlobalDataCenter.defaultCenter().isVictory = true;
		}
		else
		{
			MGGlobalDataCenter.defaultCenter().isDefeat = true;
		}
		print ("win" + MGGlobalDataCenter.defaultCenter ().isVictory + "lose" +
		       MGGlobalDataCenter.defaultCenter ().isDefeat+";"+overUI.Length);
        for (int i = 0; i < overUI.Length; ++i)
        {
			Debug.Log("overUI[i].name:"+overUI[i].name);
			Debug.Log("MGGlobalDataCenter.defaultCenter().overSenceUIName:"+MGGlobalDataCenter.defaultCenter().overSenceUIName);
            if (overUI[i].name == MGGlobalDataCenter.defaultCenter().overSenceUIName){
				Debug.Log("12312321");
				overUI[i].SetActive(true);
			}
			else
                overUI[i].SetActive(false);
        }

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnOverButtonClick() {
		//isPressOverButton = true;
        Application.LoadLevel("startGameScene");
		print ("click over again button");
	}
}
