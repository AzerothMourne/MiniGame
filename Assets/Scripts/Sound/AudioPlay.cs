using UnityEngine;
using System.Collections;

public class AudioPlay : MonoBehaviour {

	private MusicPlayer music;

	// Use this for initialization
	void Start () {
		music = (GetComponent ("MusicPlayer") as MusicPlayer);
	}
	
	// Update is called once per frame
	void Update () {
		if (MGGlobalDataCenter.defaultCenter ().isDartHit) {
			//music.play ("Sound/dart_hit");
			music.playoneshot("Sound/dart_hit");
			//AudioSource newmusic = this.GetComponent<AudioSource>();
			//newmusic.clip = (AudioClip)Resources.Load("Sound/dart_hit", typeof(AudioClip));
			//newmusic.PlayOneShot(newmusic.clip);

			MGGlobalDataCenter.defaultCenter ().isDartHit = false;
		}
		if (MGGlobalDataCenter.defaultCenter ().isDartDefence) {
			music.playoneshot ("Sound/dart_defence");
			MGGlobalDataCenter.defaultCenter ().isDartDefence = false;
		}
		if (MGGlobalDataCenter.defaultCenter ().isRoadBlockHit) {
//			music.playoneshot("Sound/roadblock_hit");
			music.playoneshot("Sound/dart_hit");
			MGGlobalDataCenter.defaultCenter().isRoadBlockHit = false;
		}
		if (MGGlobalDataCenter.defaultCenter ().isRoadBlockDefence) {
			music.playoneshot("Sound/roadblock_defence");
			MGGlobalDataCenter.defaultCenter().isRoadBlockDefence = false;
		}
		//以下未添加
		if (MGGlobalDataCenter.defaultCenter ().isDartRelease){
			music.playoneshot("Sound/dart_release");
			MGGlobalDataCenter.defaultCenter ().isDartRelease = false;
		}

		if (MGGlobalDataCenter.defaultCenter ().isKillMingyue) {
            try
            {
                music.playoneshot("Sound/kill_mingyue");
            }
            catch { }
			MGGlobalDataCenter.defaultCenter().isKillMingyue = false;
		}
		//是否胜利
		if (MGGlobalDataCenter.defaultCenter ().isVictory) {
			music.playoneshot ("Sound/victory");
			MGGlobalDataCenter.defaultCenter ().isVictory = false;
		} 
		if(MGGlobalDataCenter.defaultCenter().isDefeat){
			music.playoneshot("Sound/defeat");
			MGGlobalDataCenter.defaultCenter().isDefeat = false;
		}
		if (MGGlobalDataCenter.defaultCenter ().isFlash) {
			music.playoneshot("Sound/flash");
			MGGlobalDataCenter.defaultCenter().isFlash = false;
		}
		if (MGGlobalDataCenter.defaultCenter ().isSprint) {
			music.playoneshot("Sound/sprint");
			MGGlobalDataCenter.defaultCenter().isSprint = false;
		}
		if (MGGlobalDataCenter.defaultCenter().isRun ){
			music.playoneshot("Sound/run");
			MGGlobalDataCenter.defaultCenter().isRun = false;
		}


	}
}
