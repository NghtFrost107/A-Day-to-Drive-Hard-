using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Player Collided with an obstacle");
        }

        if (other.gameObject.CompareTag("PickUp"))
        {
            Debug.Log("Player Collided with a pickup");
        }
    }
}
