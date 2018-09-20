using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    public TextAsset saveState;
    private int playerHealth;
    private int playerCoinBalance;
    private int playerSpeed = 8000;

    [SerializeField]
    private float speedMultiplier = 1.2f;


    [SerializeField]
    private Text healthText;

    [SerializeField]
    private Text costText;

    [SerializeField]
    private Text speedText;

    [SerializeField]
    private Text playerCoinsText;


    private void Start()
    {
        ReadSaveState();
        UpdateValuesHealth();
        UpdateValuesSpeed();
    }

    //Read the savestate to find out how much health and how many coins the player has
    void ReadSaveState()
    {
        string[] saveData = saveState.text.Split('\n');
        int.TryParse(saveData[0], out playerHealth);
        int.TryParse(saveData[1], out playerCoinBalance);
        int.TryParse(saveData[2], out playerSpeed);
    }

    //Update the savestate file with the new values
    public void UpdateSaveState()
    {
        string savePath = Application.dataPath + @"/SaveState.txt";
        string[] saveData = { playerHealth.ToString(), playerCoinBalance.ToString(),playerSpeed.ToString()};
        System.IO.File.WriteAllLines(savePath, saveData);
    }
    
    //Update the values on the HUD
    void UpdateValuesHealth()
    {
        healthText.text = "Health: " + playerHealth.ToString();
        playerCoinsText.text = "Coins: " + playerCoinBalance.ToString();
    }

    void UpdateValuesSpeed()
    {
        speedText.text = "Speed: " + playerSpeed.ToString();
        playerCoinsText.text = "Coins: " + playerCoinBalance.ToString();
    }

    void UpdateCoinBalance(int upgradeCost)
    {
        playerCoinBalance = playerCoinBalance - upgradeCost;
    }

    public void UpgradeHealth()
    {
        //get the player stats for health and add 1
        PlayerProperties player = gameObject.GetComponent<PlayerProperties>();
        playerHealth++;
        UpdateCoinBalance(100);
        UpdateValuesHealth();
    }

    public void UpgradeSpeed()
    {
        //get player speed and add to the speed to increase
        PlayerProperties player = gameObject.GetComponent<PlayerProperties>();
        playerSpeed = (int)(playerSpeed * speedMultiplier);
        UpdateCoinBalance(75);
        UpdateValuesSpeed();


    }

    public void UpgradeGrip()
    {
        //get player stats for the grip and increase
    }

}
