using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class Player : MonoBehaviour {

    public bool playerInvincible;

    public Text playerCoinCounter;
    public Text playerHealthCounter;
    public Text gameOverMessage;
    
	// Use this for initialization
	void Start () {
        SetHealthCounter();
        SetCoinCounter();
        
        //Setting the gameover message to blank so it appears hidden untill the player runs out of lives
        gameOverMessage.text = "";
	}
	
	// Update is called once per frame
	void Update () {
        if (PlayerData.playerHealth <= 0)
        {
            gameOverMessage.text = "Out of Lives!\n" + "Game Over!\n" + "Returning To Main Menu!";
            
            //Return to main menu after a 5 second delay
            Invoke("ReturnToMenu", 5);
        }
	}

    //What to do if the player has collided with an obstacle
    public void ObstacleCollision()
    {
        if (playerInvincible == false)
        {
            PlayerData.playerHealth--;
            SetHealthCounter();
            Debug.Log("Player health:" + PlayerData.playerHealth);

            playerInvincible = true;
        
            //Calling PlayerSetDamageable after a 3 second delay
            Invoke("PlayerSetDamageable", 3); 
        }
        
    }

    // What to do if the player has collided with a Pickup
    public void PickupCollision(Collider2D col)
    {
        PlayerData.playerCoins++;
        SetCoinCounter();
        Destroy(col.gameObject);
    }

    //Returns Player to main menu
    void ReturnToMenu()
    {
        PlayerData.playerHealth = PlayerData.MAX_PLAYER_HEALTH;
        PlayerData.WriteSaveState();
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }

    // Invincibility Bool
    void PlayerSetDamageable()
    {
        playerInvincible = false;
    }

    // Health Counter
    void SetHealthCounter()
    {
        playerHealthCounter.text = "Health: " + PlayerData.playerHealth;
    }

    // Coin Counter
    void SetCoinCounter()
    {
        playerCoinCounter.text = "Coins: " + PlayerData.playerCoins;
    }

}
