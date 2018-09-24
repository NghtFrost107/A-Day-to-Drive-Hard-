using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

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
        try
        {
            StreamReader sr = new StreamReader(Application.dataPath + @"/SaveState.txt");

            int.TryParse(sr.ReadLine(), out playerCoins);
            int.TryParse(sr.ReadLine(), out playerHealth);
            int.TryParse(sr.ReadLine(), out playerSpeed);

            //Creating a save file with default values if there is no existing save
            if(playerSpeed == 0)
            {
                playerSpeed = 8000;
            }
            

            sr.Dispose();
        }
        catch (FileNotFoundException)
        {
            Debug.Log("The file was not found!, New Save file will be created");

            //Setting all values to the base value, in case the player has no save file
            playerCoins = 900; //For testing the default number of coins is 900 (Would normally be 0)
            playerHealth = 3;
            playerSpeed = 8000;
        }
        catch (IOException)
        {
            Debug.Log("There was an error in the file");
        }
    }

    // Write out to Save State - Coins, Health, Speed
    public void WriteSaveState()
    {
        try
        {
            StreamWriter sw = new StreamWriter(Application.dataPath + "/SaveState.txt");
            sw.WriteLine(playerCoins);
            sw.WriteLine(playerHealth);
            sw.WriteLine(playerSpeed);

            sw.Dispose();
        }
        catch (IOException)
        {
            Debug.Log("There was a problem writing to the file");
        }
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