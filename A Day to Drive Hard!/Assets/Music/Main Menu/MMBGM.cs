using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMBGM : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    private static MMBGM instance = null;
    public static MMBGM Instance()
    {
        return instance;
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
