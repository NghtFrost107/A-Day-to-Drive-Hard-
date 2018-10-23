using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicSliderValueTranslate : MonoBehaviour {

    public Slider musicSlider;

	// Use this for initialization
	void Start ()
    {
        musicSlider = GetComponent<Slider>();
        musicSlider.value = MMBGM.Instance().gameObject.GetComponent<AudioSource>().volume;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
