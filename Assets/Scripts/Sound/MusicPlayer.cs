using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {

    public AudioSource music;

    public void play(string str)
    {
        music.clip = (AudioClip)Resources.Load(str, typeof(AudioClip));
        music.Play();
    }
	public void playoneshot(string str)
	{
		music.clip = (AudioClip)Resources.Load (str, typeof(AudioClip));
		music.PlayOneShot (music.clip);
	}
	// Use this for initialization
	public void LoadVoice (string str)
	{
		music.clip = (AudioClip)Resources.Load (str, typeof(AudioClip));
	}

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
