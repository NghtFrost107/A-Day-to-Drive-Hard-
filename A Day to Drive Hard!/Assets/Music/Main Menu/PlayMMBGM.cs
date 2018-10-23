using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMMBGM : MonoBehaviour {
    
    // Use this for initialization
    void Start ()
    {
        if (!MMBGM.Instance().gameObject.GetComponent<AudioSource>().isPlaying)
        {
            MMBGM.Instance().gameObject.GetComponent<AudioSource>().Play();
            MMBGM.Instance().gameObject.GetComponent<AudioSource>().volume = SoundManager.Instance().gameObject.GetComponent<SoundManager>().musicSlide.value;
            GPBGM.Instance().gameObject.GetComponent<AudioSource>().Stop();
            EEBGM.Instance().gameObject.GetComponent<AudioSource>().Stop();
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        
    }
}
