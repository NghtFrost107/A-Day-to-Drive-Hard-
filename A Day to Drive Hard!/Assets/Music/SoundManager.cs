using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour {

    public AudioSource carEngine;                   //Drag a reference to the audio source which will play the sound effects.
    public AudioSource mainMenuMusic;                 //Drag a reference to the audio source which will play the music.
    public AudioSource inGameMusic;
    public AudioSource easterEggMusic;
    public Slider carEngineSlide;
    public Slider musicSlide;
    private static SoundManager instance = null;     //Allows other scripts to call functions from SoundManager.             

    private void Start()
    {
        instance.musicSlide.value = MMBGM.Instance().gameObject.GetComponent<AudioSource>().volume;
        instance.carEngineSlide.value = carEngineSlide.value;

        instance.carEngine.volume = instance.carEngineSlide.value;
        MMBGM.Instance().gameObject.GetComponent<AudioSource>().volume = instance.musicSlide.value;
        GPBGM.Instance().gameObject.GetComponent<AudioSource>().volume = instance.musicSlide.value;
        EEBGM.Instance().gameObject.GetComponent<AudioSource>().volume = instance.musicSlide.value;
    }

    void Awake()
    {
        //Check if there is already an instance of SoundManager
        if (instance == null)
        {
            //if not, set it to this.
            instance = this;
        }
        //If instance already exists:
        else if (instance != this && instance != null)
        {
            //Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
            Destroy(gameObject);
            return;
        }

        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }

    public static SoundManager Instance()
    {
        return instance;
    }

    void Update()
    {
        instance.carEngine.volume = instance.carEngineSlide.value;
        MMBGM.Instance().gameObject.GetComponent<AudioSource>().volume = instance.musicSlide.value;
        GPBGM.Instance().gameObject.GetComponent<AudioSource>().volume = instance.musicSlide.value;
        EEBGM.Instance().gameObject.GetComponent<AudioSource>().volume = instance.musicSlide.value;
    }
}
