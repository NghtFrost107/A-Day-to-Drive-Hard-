using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour {

    private static SoundManager instance = null;     //Allows other scripts to call functions from SoundManager. 

    public AudioClip mainMenuMusic;
                
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

    void Start()
    {
        GetComponent<AudioSource>().clip = mainMenuMusic;
        GetComponent<AudioSource>().Play();
    }

    public void PlayMusic(AudioClip music)
    {
        GetComponent<AudioSource>().clip = music;
        GetComponent<AudioSource>().Play();
    }
    public static SoundManager Instance()
    {
        return instance;
    }


}
