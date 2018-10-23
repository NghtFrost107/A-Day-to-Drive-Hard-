using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayMMBGM : MonoBehaviour {
    
    // Use this for initialization
    void Start ()
    {
        if (!MMBGM.Instance().gameObject.GetComponent<AudioSource>().isPlaying)
        {
            MMBGM.Instance().gameObject.GetComponent<AudioSource>().Play();
            GPBGM.Instance().gameObject.GetComponent<AudioSource>().Stop();
            EEBGM.Instance().gameObject.GetComponent<AudioSource>().Stop();
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
    }
}
