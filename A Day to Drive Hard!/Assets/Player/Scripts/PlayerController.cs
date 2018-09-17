using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public int playerHealth;
    private bool invincible;
    

	// Use this for initialization
	void Start () {
        playerHealth = 3;
        invincible = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (playerHealth <= 0)
        {
            Debug.Log("The Player has run out of lives and the game SHOULD end");
        }
	}

    // What initially happens when the player collides with an object
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("Player Collided with an obstacle");
            playerHealth--;
            SetInvincible();
        }

        if (other.gameObject.CompareTag("PickUp"))
        {
            Debug.Log("Player Collided with a pickup");
        }
    }

    public void SetInvincible()
    {
        invincible = true;
        Debug.Log("Invis");
        CancelInvoke("SetDamageable");
        Invoke("SetDamageable", 3);
    }

    void SetDamageable()
    {
        invincible = false;
        Debug.Log("not invis");
    }

}
