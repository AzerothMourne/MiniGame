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
		//以下未添加
		if (MGGlobalDataCenter.defaultCenter().isDartRelease){
			music.playoneshot("Sound/dart_relase");
			MGGlobalDataCenter.defaultCenter ().isDartRelease = false;
		}
		if (MGGlobalDataCenter.defaultCenter ().isKillMingyue) {
			music.playoneshot("Sound/kill_mingyue");
			MGGlobalDataCenter.defaultCenter().isKillMingyue = false;
		}
		if (MGGlobalDataCenter.defaultCenter ().isWin) {
			music.play("Sound/win");
			MGGlobalDataCenter.defaultCenter().isWin = false;
		}
		if (MGGlobalDataCenter.defaultCenter ().isLose) {
			music.play("Sound/lose");
			MGGlobalDataCenter.defaultCenter().isLose = false;
		}

	}
}
