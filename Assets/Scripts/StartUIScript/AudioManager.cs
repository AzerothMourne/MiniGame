using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
	public static AudioManager _instance;
	private AudioSource audio;
	public AudioClip background;

	void  Awake() {
		_instance = this;
		audio = this.GetComponent<AudioSource>();
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public  void  MusicBackground() {
		//audio.PlayOneShot(background);
	}
}
