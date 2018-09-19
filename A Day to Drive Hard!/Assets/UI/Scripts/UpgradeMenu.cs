using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    private PlayerProperties stats;

    [SerializeField]
    private Text healthText;

    [SerializeField]
    private Text costText;

    [SerializeField]
    private Text playerCoinsText;


    void grabHealth()
    {
        


    }
    void UpdateValues()
    {
        healthText.text = "Health: " + stats.playerHealth.ToString();
        playerCoinsText.text = "Coins: " + stats.playerCoinBalance.ToString();
    }

    void UpgradeCost(int upgradeCost)
    {
        stats.playerCoinBalance = stats.playerCoinBalance - upgradeCost;
    }

    public void UpgradeHealth()
    {
        //get the player stats for health and add 1
        PlayerProperties player = gameObject.GetComponent<PlayerProperties>();

        player.playerHealth = player.playerHealth + 1;
        UpgradeCost(100);
        UpdateValues();
    }

    public void UpgradeSpeed()
    {
        //get player speed and add to the speed to increase
    }

    public void UpgradeGrip()
    {
        //get player stats for the grip and increase
    }



}
