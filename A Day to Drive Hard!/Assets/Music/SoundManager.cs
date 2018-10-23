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
    public static SoundManager instance;     //Allows other scripts to call functions from SoundManager.             

    private void Start()
    {
        instance.carEngineSlide.value = instance.carEngine.volume;
        instance.musicSlide.value = instance.mainMenuMusic.volume = instance.inGameMusic.volume = instance.easterEggMusic.volume;
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
        else if (instance != this)
        {
            //Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
            Destroy(gameObject);
        }

        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }

    public SoundManager Instance()
    {
        return instance;
    }

    void FixedUpdate()
    {
        instance.carEngine.volume = instance.carEngineSlide.value;
        instance.mainMenuMusic.volume = instance.inGameMusic.volume = instance.easterEggMusic.volume = instance.musicSlide.value;
    }
}
