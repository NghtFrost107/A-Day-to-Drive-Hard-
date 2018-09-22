using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    public TextAsset saveState;
    public string savePath;

    public int playerCoins;
    public int playerHealth;
    public int playerSpeed;

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
        ReadSaveState();
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateValuesHealth();
        UpdateValuesSpeed();
        WriteSaveState();
    }

    // Read in from Save State - Coins, Health, Speed
    public void ReadSaveState()
    {
        string[] saveData = saveState.text.Split('\n');
        int.TryParse(saveData[0], out playerCoins);
        int.TryParse(saveData[1], out playerHealth);
        int.TryParse(saveData[2], out playerSpeed);
    }

    // Write out to Save State - Coins, Health, Speed
    public void WriteSaveState()
    {
        savePath = Application.dataPath + @"/SaveState.txt";
        string[] saveData = { playerCoins.ToString(), playerHealth.ToString(),playerSpeed.ToString()};
        System.IO.File.WriteAllLines(savePath, saveData);
    }
   
    //Update the values on the HUD
    public void UpdateValuesHealth()
    {
        healthText.text = "Health: " + playerHealth;
        playerCoinsText.text = "Coins: " + playerCoins;
    }

    public void UpdateValuesSpeed()
    {
        speedText.text = "Speed: " + playerSpeed;
        playerCoinsText.text = "Coins: " + playerCoins;
    }


    // When pressed will check coins, deduct coins and apply upgrade, write out to save
    public void HealthUpgrade()
    {
  
        if (playerCoins >= 100)
        {
            playerCoins -= 100;
            playerHealth++;
            UpdateValuesHealth();
            WriteSaveState();
        }
        else
        {
            showNotEnoughCoinsPanel();
        }

    }

    // When pressed will check coins, deduct coins and apply upgrade, write out to save
    public void SpeedUpgrade()
    {
       
        if (playerCoins >= 75)
        {
            playerCoins -= 75;
            playerSpeed += speedUpgradeAmount;
            UpdateValuesSpeed();
            WriteSaveState();
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