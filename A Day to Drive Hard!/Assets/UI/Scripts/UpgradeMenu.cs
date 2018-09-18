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

    void UpdateValues()
    {
        healthText.text = "Health: " + stats.playerHealth.ToString();
    }

    public void UpgradeHealth()
    {
        //get the player stats for health and add 1
        stats.playerHealth = stats.playerHealth + 1;
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
