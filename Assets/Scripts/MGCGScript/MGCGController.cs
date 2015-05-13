
//#define PCORMOBILE
using UnityEngine;
using System.Collections;

public class MGCGController: MonoBehaviour
{
	
	//电影纹理
#if PCORMOBILE
	public MovieTexture movTexture;
#endif
    private GameObject uiRoot,Audio,StartScript,CreatStartUI,BirdController;
	private bool isEnd;
	void Start()
	{
		isEnd = false;
        try
        {
            uiRoot = GameObject.Find("UI Root");
            Audio = GameObject.Find("Audio");
            StartScript = GameObject.Find("StartScript");
            CreatStartUI = GameObject.Find("CreatStartUI");
            BirdController = GameObject.Find("BirdController");
            uiRoot.SetActive(false);
            Audio.SetActive(false);
            StartScript.SetActive(false);
            CreatStartUI.SetActive(false);
            BirdController.SetActive(false);
        }
        catch { }
#if PCORMOBILE
        //设置当前对象的主纹理为电影纹理
        renderer.material.mainTexture = movTexture;
        //设置电影纹理播放模式为循环
        //movTexture.loop = true;
        movTexture.Play();
#else
        
		Handheld.PlayFullScreenMovie("kaichangshipin.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);

#endif
		
		
	}
	void Update()
	{
#if PCORMOBILE
        if (!movTexture.isPlaying)
        {
            try
            {
                uiRoot.SetActive(true);
                Audio.SetActive(true);
                StartScript.SetActive(true);
                CreatStartUI.SetActive(true);
                BirdController.SetActive(true);
            }
            catch { }
            Debug.Log("Destroy mov");
            Destroy(this.gameObject);
        }
#else
        
		if(!isEnd){
			isEnd=true;
			try
			{
				uiRoot.SetActive(true);
				Audio.SetActive(true);
				StartScript.SetActive(true);
				CreatStartUI.SetActive(true);
				BirdController.SetActive(true);
			}
			catch { }
			Debug.Log("Destroy mov");
			Destroy(this.gameObject);
		}

#endif
        
	}
    
}