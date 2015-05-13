using UnityEngine;
using System.Collections;

public class MGCGController: MonoBehaviour
{
	
	//电影纹理
	//	public MovieTexture movTexture;
	private GameObject uiRoot,Audio,StartScript,CreatStartUI,BirdController;
	private bool isEnd;
	void Start()
	{
		isEnd = false;
		//		//设置当前对象的主纹理为电影纹理
		//		renderer.material.mainTexture = movTexture;
		//		//设置电影纹理播放模式为循环
		//		//movTexture.loop = true;
		//		if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
		//        {
		//            movTexture.Play();
		//        }
		//        else if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
		//        {
		//            Handheld.PlayFullScreenMovie("kaichangshipin.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);
		//        }
		try
		{
			uiRoot = GameObject.Find("UI Root");
			Audio = GameObject.Find("Audio");
			StartScript= GameObject.Find("StartScript");
			CreatStartUI=GameObject.Find("CreatStartUI");
			BirdController=GameObject.Find("BirdController");
			uiRoot.SetActive(false);
			Audio.SetActive(false);
			StartScript.SetActive(false);
			CreatStartUI.SetActive(false);
			BirdController.SetActive(false);
		}
		catch { }
		Handheld.PlayFullScreenMovie("kaichangshipin.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);

		
		
	}
	void Update()
	{
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
		//        if (!movTexture.isPlaying)
		//        {
		//            try
		//            {
		//                uiRoot.SetActive(true);
		//				Audio.SetActive(true);
		//            }
		//            catch { }
		//            Debug.Log("Destroy mov");
		//            Destroy(this.gameObject);
		//        }
	}
}