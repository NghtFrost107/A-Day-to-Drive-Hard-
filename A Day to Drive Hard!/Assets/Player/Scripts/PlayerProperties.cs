using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProperties : MonoBehaviour {
    public int playerHealth = 3;
    public Text playerHealthCounter;
    public int playerCoinBalance = 100;
    public Text playerCoinCounter;
    public bool playerInvincible;
    

	// Use this for initialization
	void Start () {
        SetHealthCounter();
        SetCoinCounter();
	}
	
	// Update is called once per frame
	void Update () {
        if (playerHealth <= 0)
        {
            Debug.Log("Player has run out of lives!");
            //TODO: Add endgame logic whe the player runs out of lives
        }
	}

    //What to do if the player has collided with an obstacle
    public void ObstacleCollision()
    {
        playerHealth--;
        SetHealthCounter();
        Debug.Log("Player health:" + playerHealth);

        playerInvincible = true;
        
        //Calling PlayerSetDamageable after a 3 second delay
        Invoke("PlayerSetDamageable", 3); 
    }

    public void PickupCollision(Collider2D col)
    {
        playerCoinBalance++;
        SetCoinCounter();
        Debug.Log("Coins: " + playerCoinBalance);
       
        Destroy(col.gameObject);
    }

    void PlayerSetDamageable()
    {
        playerInvincible = false;
    }

    void SetHealthCounter()
    {
        playerHealthCounter.text = "Health: " + playerHealth.ToString();
    }

    void SetCoinCounter()
    {
        playerCoinCounter.text = "Coins: " + playerCoinBalance.ToString();
    }
}
