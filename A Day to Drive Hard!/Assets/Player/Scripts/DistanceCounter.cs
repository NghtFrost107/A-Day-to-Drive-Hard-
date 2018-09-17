using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceCounter : MonoBehaviour
{
    public Text distanceCounter;

    public GameObject player;
    public float beginningPos;
    public float currentPos;

    public float multiplier = 0.1f;

	void Start()
    {
        beginningPos = player.transform.position.x;
        distanceCounter = GetComponent<Text>();
	}
	
	void Update()
    {
        currentPos = player.transform.position.x - beginningPos;

        int distance = Mathf.Abs(Mathf.RoundToInt(currentPos * multiplier));

        distanceCounter.text = "Distance Traveled: " + distance + " m";
	}
}
