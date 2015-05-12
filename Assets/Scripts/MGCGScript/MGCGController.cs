using UnityEngine;
using System.Collections;
 
public class MGCGController: MonoBehaviour
{
 
	//电影纹理
	public MovieTexture movTexture;
    private GameObject uiRoot;
	void Start()
	{
		//设置当前对象的主纹理为电影纹理
		renderer.material.mainTexture = movTexture;
		//设置电影纹理播放模式为循环
		//movTexture.loop = true;
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            movTexture.Play();
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
        {
            Handheld.PlayFullScreenMovie("kaichangshipin.mp4", Color.black, FullScreenMovieControlMode.CancelOnInput);
        }
        try
        {
            uiRoot = GameObject.Find("UI Root");
            uiRoot.SetActive(false);
        }
        catch { }
        
        
	}
    void Update()
    {
        if (!movTexture.isPlaying)
        {
            try
            {
                uiRoot.SetActive(true);
            }
            catch { }
            Debug.Log("Destroy mov");
            Destroy(this.gameObject);
        }
    }
}