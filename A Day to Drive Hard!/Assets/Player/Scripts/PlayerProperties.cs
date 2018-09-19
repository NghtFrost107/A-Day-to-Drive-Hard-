using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PlayerProperties : MonoBehaviour {
    public static int currentPlayerHealth;
    public Text playerHealthCounter;
    public static int playerCoinBalance;
    public Text playerCoinCounter;
    public bool playerInvincible;
    public TextAsset saveState;

	// Use this for initialization
	void Start () {
        ReadSaveState();
        SetHealthCounter();
        SetCoinCounter();
	}
	
	// Update is called once per frame
	void Update () {
        if (currentPlayerHealth <= 0)
        {
            Debug.Log("Player has run out of lives!");
            //TODO: Add endgame logic whe the player runs out of lives
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

    void PlayerSetDamageable()
    {
        playerInvincible = false;
    }

    void SetHealthCounter()
    {
        playerHealthCounter.text = "Health: " + currentPlayerHealth.ToString();
    }

    void SetCoinCounter()
    {
        playerCoinCounter.text = "Coins: " + playerCoinBalance.ToString();
    }

    public int getPlayerHealth()
    {
        return currentPlayerHealth;
    }

    public void setPlayerHealth(int i)
    {
        currentPlayerHealth = i;
    }

    public int getPlayerCoinBalance()
    {
        return playerCoinBalance;
    }

    public void setPlayerCoinBalance(int i)
    {
        playerCoinBalance = i;
    }
}
