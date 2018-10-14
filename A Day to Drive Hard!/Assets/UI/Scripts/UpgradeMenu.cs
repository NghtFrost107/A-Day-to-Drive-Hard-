using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class UpgradeMenu : MonoBehaviour
{
    public int speedUpgradeAmount = 2000;

    public GameObject notEnoughCoinsPanel;
    int counter = 1;
    
    public Text healthText;
    public Text notEnoughCoins;
    public Text costText;
    public Text speedText;
    public Text playerCoinsText;

    private Database database;
    void Awake()
    {
        database = GameObject.FindGameObjectWithTag("Database").GetComponent<Database>();
    }
    // Use this for initialization
    void Start()
    {
       // PlayerData.ReadSaveState();
        UpdateValuesHealth();
        UpdateValuesSpeed();
    }
   
    //Update the values on the HUD
    public void UpdateValuesHealth()
    {
        healthText.text = "Max Health: " + database.player.MAX_PLAYER_HEALTH;
        playerCoinsText.text = "Coins: " + database.player.playerCoins;
    }

    public void UpdateValuesSpeed()
    {
        speedText.text = "Speed: " + database.player.playerSpeed;
        playerCoinsText.text = "Coins: " + database.player.playerCoins;
    }


    // When pressed will check coins, deduct coins and apply upgrade, write out to save
    public void HealthUpgrade()
    {
  
        if (database.player.playerCoins >= 100)
        {
            database.player.playerCoins -= 100;
            database.player.MAX_PLAYER_HEALTH++;
            database.player.playerHealth = database.player.MAX_PLAYER_HEALTH;
            UpdateValuesHealth();
            database.SetPlayerData();
        }
        else
        {
            showNotEnoughCoinsPanel();
        }

    }

    // When pressed will check coins, deduct coins and apply upgrade, write out to save
    public void SpeedUpgrade()
    {
       
        if (database.player.playerCoins >= 75)
        {
            database.player.playerCoins -= 75;
            database.player.playerSpeed += speedUpgradeAmount;
            UpdateValuesSpeed();
            database.SetPlayerData();
        }
        else
        {
            showNotEnoughCoinsPanel();
        }
      

    }

    // Placeholder function for Grip Upgrade
    public void GripUpgrade()
    {
        // Nothing here yet
    }

    // Show "Not Enough Coins" panel
    public void showNotEnoughCoinsPanel()
    {
        counter++;
        if (counter % 2 == 1)
        {
            notEnoughCoinsPanel.gameObject.SetActive(false);
        }
        else
        {
            notEnoughCoinsPanel.gameObject.SetActive(true);
        }
    }
}