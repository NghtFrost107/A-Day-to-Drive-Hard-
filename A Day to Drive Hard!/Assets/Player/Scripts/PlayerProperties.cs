using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class PlayerProperties : MonoBehaviour {
    public static int currentPlayerHealth;
    public Text playerHealthCounter;

    public static int playerCoinBalance;
    public Text playerCoinCounter;

    public bool playerInvincible;

    public TextAsset saveState;

    public Text gameOverMessage;

	// Use this for initialization
	void Start () {
        ReadSaveState();
        SetHealthCounter();
        SetCoinCounter();

        //Setting the gameover message to blank so it appears hidden untill the player runs out of lives
        gameOverMessage.text = "";
	}
	
	// Update is called once per frame
	void Update () {
        if (currentPlayerHealth <= 0)
        {
            Debug.Log("Player has run out of lives!");
            gameOverMessage.text = "Out of Lives!\n" + "Game Over!\n" + "Returning To Main Menu!";
            Invoke("ReturnToMenu", 5);
        }
	}

    //What to do if the player has collided with an obstacle
    public void ObstacleCollision()
    {
        currentPlayerHealth--;
        SetHealthCounter();
        Debug.Log("Player health:" + currentPlayerHealth);

        playerInvincible = true;
        
        //Calling PlayerSetDamageable after a 3 second delay
        Invoke("PlayerSetDamageable", 3); 
    }

    // What to do if the player has collided with a Pickup
    public void PickupCollision(Collider2D col)
    {
        playerCoinBalance++;
        SetCoinCounter();
        Debug.Log("Coins: " + playerCoinBalance);
       
        Destroy(col.gameObject);
    }

    //Read health and coin balance from text file and apply values found to the player
    void ReadSaveState()
    {
        string[] saveData = saveState.text.Split('\n');
        int.TryParse(saveData[0], out currentPlayerHealth);
        int.TryParse(saveData[1], out playerCoinBalance);
    }

    //Returns Player to main menu
    void ReturnToMenu()
    {
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
        playerHealthCounter.text = "Health: " + currentPlayerHealth.ToString();
    }

    // Coin Counter
    void SetCoinCounter()
    {
        playerCoinCounter.text = "Coins: " + playerCoinBalance.ToString();
    }

    // Get Health
    public int getPlayerHealth()
    {
        return currentPlayerHealth;
    }

    // Set Health
    public void setPlayerHealth(int i)
    {
        currentPlayerHealth = i;
    }

    // Get Coin Balance
    public int getPlayerCoinBalance()
    {
        return playerCoinBalance;
    }

    // Set Coin Balance
    public void setPlayerCoinBalance(int i)
    {
        playerCoinBalance = i;
    }
}
