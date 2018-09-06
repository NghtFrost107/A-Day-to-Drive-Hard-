using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeCounter : MonoBehaviour
{
    public Text timeCounter;

    public int multiplier = 10;

	void Start()
    {
        timeCounter = GetComponent<Text>();
    }
	
	void Update()
    {
        int time = Mathf.RoundToInt(Time.time * multiplier);

        timeCounter.text = "Time Played: " + time + " ms";
    }
}
