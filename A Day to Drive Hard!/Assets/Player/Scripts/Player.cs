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
    public Text distanceCounter;

    public float currentPos;
    private bool playerDead = false;

    private Database database;
    void Awake()
    {
        database = GameObject.FindGameObjectWithTag("Database").GetComponent<Database>();
    }
    // Use this for initialization
    void Start () {
        SetHealthCounter();
        SetCoinCounter();
        
        //Setting the gameover message to blank so it appears hidden untill the player runs out of lives
        gameOverMessage.text = "";
	}
	
	// Update is called once per frame
	void Update () {

        
        if (database.player.playerHealth <= 0)
        {
            gameOverMessage.text = "Out of Lives!\n" + "Game Over!\n" + "Returning To Main Menu!";

            if (!playerDead)
            {
                //Return to main menu after a 5 second delay
                Invoke("ReturnToMenu", 5);
                playerDead = true;
            }
        } else
        {
            currentPos = transform.GetChild(0).transform.position.x;
            distanceCounter.text = "Distance Travelled: " + Mathf.Round(currentPos) + "m";
        }
	}

    //What to do if the player has collided with an obstacle
    public void ObstacleCollision()
    {
        if (playerInvincible == false)
        {
            database.player.playerHealth--;
            SetHealthCounter();
            Debug.Log("Player health:" + database.player.playerHealth);

            playerInvincible = true;
        
            //Calling PlayerSetDamageable after a 3 second delay
            Invoke("PlayerSetDamageable", 3); 
        }
        
    }

    // What to do if the player has collided with a Pickup
    public void PickupCollision(Collider2D col)
    {
        database.player.playerCoins++;
        SetCoinCounter();
        Destroy(col.gameObject);
    }

    //Returns Player to main menu
    void ReturnToMenu()
    {
        database.player.playerHealth = database.player.MAX_PLAYER_HEALTH;
       // PlayerData.WriteSaveState();
        database.addScore(new Score()
        {
            time = System.DateTime.Now.ToShortTimeString(),
            date = System.DateTime.Now.ToShortDateString(),
            score = (int)Mathf.Round(currentPos)
        });
        database.setPlayerData();
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
        playerHealthCounter.text = "Health: " + database.player.playerHealth;
    }

    // Coin Counter
    void SetCoinCounter()
    {
        playerCoinCounter.text = "Coins: " + database.player.playerCoins;
    }

}
