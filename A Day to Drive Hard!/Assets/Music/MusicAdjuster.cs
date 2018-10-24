using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicAdjuster: MonoBehaviour {

    public SoundManager soundManager;
    public AudioClip music;

    // Use this for initialization
    void Start () {
        soundManager = SoundManager.Instance();
        if (music != null)
        {
            soundManager.PlayMusic(music);
        }
	}

    public void changeVolume()
    {
        soundManager.GetComponent<AudioSource>().volume = GetComponent<Slider>().value;
    }

}
