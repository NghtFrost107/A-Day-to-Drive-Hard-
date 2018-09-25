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

    // Use this for initialization
    void Start()
    {
        PlayerData.ReadSaveState();
        UpdateValuesHealth();
        UpdateValuesSpeed();
    }
   
    //Update the values on the HUD
    public void UpdateValuesHealth()
    {
        healthText.text = "Max Health: " + PlayerData.MAX_PLAYER_HEALTH;
        playerCoinsText.text = "Coins: " + PlayerData.playerCoins;
    }

    public void UpdateValuesSpeed()
    {
        speedText.text = "Speed: " + PlayerData.playerSpeed;
        playerCoinsText.text = "Coins: " + PlayerData.playerCoins;
    }


    // When pressed will check coins, deduct coins and apply upgrade, write out to save
    public void HealthUpgrade()
    {
  
        if (PlayerData.playerCoins >= 100)
        {
            PlayerData.playerCoins -= 100;
            PlayerData.MAX_PLAYER_HEALTH++;
            PlayerData.playerHealth = PlayerData.MAX_PLAYER_HEALTH;
            UpdateValuesHealth();
            PlayerData.WriteSaveState();
        }
        else
        {
            showNotEnoughCoinsPanel();
        }

    }

    // When pressed will check coins, deduct coins and apply upgrade, write out to save
    public void SpeedUpgrade()
    {
       
        if (PlayerData.playerCoins >= 75)
        {
            PlayerData.playerCoins -= 75;
            PlayerData.playerSpeed += speedUpgradeAmount;
            UpdateValuesSpeed();
            PlayerData.WriteSaveState();
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