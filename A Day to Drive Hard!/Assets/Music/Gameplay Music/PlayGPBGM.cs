using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGPBGM : MonoBehaviour {

    // Use this for initialization
    void Start ()
    {
        //if (!GPBGM.Instance().gameObject.GetComponent<AudioSource>().isPlaying)
        //{
            MMBGM.Instance().gameObject.GetComponent<AudioSource>().Stop();
            GPBGM.Instance().gameObject.GetComponent<AudioSource>().Play();
            EEBGM.Instance().gameObject.GetComponent<AudioSource>().Stop();
        //}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
