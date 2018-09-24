using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class PlayerProperties : MonoBehaviour {
    public static int currentPlayerHealth;
    public static int playerCoinBalance;
    public bool playerInvincible;
    private static int maxPlayerHealth;

    public Text playerCoinCounter;
    public Text playerHealthCounter;
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
            currentPlayerHealth--;
            SetHealthCounter();
            Debug.Log("Player health:" + currentPlayerHealth);

            playerInvincible = true;
        
            //Calling PlayerSetDamageable after a 3 second delay
            Invoke("PlayerSetDamageable", 3); 
        }
        
    }

    // What to do if the player has collided with a Pickup
    public void PickupCollision(Collider2D col)
    {
        playerCoinBalance++;
        SetCoinCounter();
        Destroy(col.gameObject);
    }

    //Read health and coin balance from text file and apply values found to the player
    public void ReadSaveState()
    {
        try
        {
            StreamReader sr = new StreamReader(Application.dataPath + @"/SaveState.txt");
            int.TryParse(sr.ReadLine(), out playerCoinBalance);
            int.TryParse(sr.ReadLine(), out maxPlayerHealth);

            sr.Dispose();

            currentPlayerHealth = maxPlayerHealth;
        }
        catch (FileNotFoundException)
        {
            Debug.Log("The file was not found!, New Save file will be created");

            //Setting all values to the base value, in case the player has no save file
            playerCoinBalance = 900; //For testing the default number of coins is 900 (Would normally be 0)
            maxPlayerHealth = 3;
            currentPlayerHealth = 3;
        }
        catch (IOException)
        {
            Debug.Log("There was an error in the file");
        }
    }
    
    //Updating the savestate with any new coins collected
    public void WriteSaveState()
    {
        try
        {
            StreamWriter sw = new StreamWriter(Application.dataPath + @"/SaveState.txt");
            sw.WriteLine(playerCoinBalance);
            sw.WriteLine(maxPlayerHealth);

            sw.Dispose();
        }
        catch (IOException)
        {
            Debug.Log("There was an error writing the save state");
        }
    }

    //Returns Player to main menu
    void ReturnToMenu()
    {
        WriteSaveState();
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
        playerHealthCounter.text = "Health: " + currentPlayerHealth;
    }

    // Coin Counter
    void SetCoinCounter()
    {
        playerCoinCounter.text = "Coins: " + playerCoinBalance;
    }
}
