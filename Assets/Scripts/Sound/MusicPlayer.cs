using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {

    public AudioSource music;

    public void play(string str)
    {
        music.clip = (AudioClip)Resources.Load(str, typeof(AudioClip));
        music.Play();
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
