using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayEEBGM : MonoBehaviour {

    public GameObject carModel;
    public Text AHHH;
    public Transform parentCanvas;
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
        } else if (positionY < -850)
        {
            Text AH = Instantiate(AHHH, parentCanvas);
            AH.transform.SetAsFirstSibling();
            StartCoroutine(CombinedMovementScript.FadeInOutText(AH,90));
        }
    }
}
