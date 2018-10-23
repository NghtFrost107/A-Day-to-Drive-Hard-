using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EEBGM : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    private static EEBGM instance = null;
    public static EEBGM Instance()
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
    void Update ()
    {
		
	}
}
