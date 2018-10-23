using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayEEBGM : MonoBehaviour {

    public GameObject carModel;
    public float positionY;

	// Use this for initialization
	void Start ()
    {
        positionY = 0.0f;
	}

    // Update is called once per frame
    void Update()
    {
        positionY = carModel.transform.position.y;

        if (!EEBGM.Instance().gameObject.GetComponent<AudioSource>().isPlaying && positionY < -45)
        {
            MMBGM.Instance().gameObject.GetComponent<AudioSource>().Stop();
            GPBGM.Instance().gameObject.GetComponent<AudioSource>().Stop();
            EEBGM.Instance().gameObject.GetComponent<AudioSource>().Play();
        }
    }
}
