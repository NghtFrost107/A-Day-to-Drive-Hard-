using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicSliderValueTranslate : MonoBehaviour {

    private static Slider musicSlider;
    public static Slider Instance()
    {
        return musicSlider;
    }

	// Use this for initialization
	void Start ()
    {
        musicSlider = GetComponent<Slider>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
