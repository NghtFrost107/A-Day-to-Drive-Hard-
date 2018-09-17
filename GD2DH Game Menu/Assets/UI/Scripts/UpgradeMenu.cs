using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    private PlayerStats stats;
    
        
    [SerializeField]
    private Text healthText;

    [SerializeField]
    private Text costText;

    public int healthcount = 3;

    void OnEnable()
    {
        stats = PlayerStats.instance;
        UpdateValues();
    }
    void UpdateValues()
    {
        healthText.text = "Health: " + healthcount.ToString(); //stats.MaxHealth.ToString();
    }

    public void UpgradeHealth()
    {
        //get the player stats for health and add 1
        healthcount = healthcount + 1;
        
        //stats.MaxHealth = stats.MaxHealth  + 1;
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
