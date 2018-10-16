using UnityEngine;
using UnityEngine.UI;

public enum Upgrades
{
    HEALTH = 5,
    SPEED = 10,
    GRIP = 3
    //Future upgrades here
}
public class UpgradeMenu : MonoBehaviour
{
    public GameObject notEnoughCoinsPanel;

    public Upgrades thisUpgrade;

    public Text upgradeText;
    public Text costText;
    public Text playerCoinsText;

    private int upgradeCost;
    private Database database;
    void Awake()
    {
        database = GameObject.FindGameObjectWithTag("Database").GetComponent<Database>();
    }
    // Use this for initialization
    void Start()
    {
        setHUD();
    }

    public void setHUD()
    {
        switch(thisUpgrade)
        {
            case Upgrades.HEALTH:
                {
                    upgradeCost = database.player.MAX_PLAYER_HEALTH * (int)Upgrades.HEALTH;
                    upgradeText.text = "Max Health: " + database.player.MAX_PLAYER_HEALTH;
                    
                } break;
            case Upgrades.SPEED:
                {
                    upgradeCost = (database.player.PlayerSpeed/1000) * (int)Upgrades.SPEED;
                    upgradeText.text = "Speed: " + database.player.PlayerSpeed;
                } break;
        }
        playerCoinsText.text = "Coins: " + database.player.PlayerCoins;
        costText.text = "Cost: " + upgradeCost;
    }
    public void ApplyUpgrade()
    {
        if (database.player.PlayerCoins >= upgradeCost)
        {
            database.player.PlayerCoins -= upgradeCost;
            switch (thisUpgrade)
            {
                case Upgrades.HEALTH:
                    {
                        database.player.MAX_PLAYER_HEALTH++;
                        database.player.playerHealth = database.player.MAX_PLAYER_HEALTH;
                    }
                    break;
                case Upgrades.SPEED:
                    {
                        database.player.PlayerSpeed += 1000;
                    }
                    break;
            }
            setHUD();
            database.SetPlayerData();
        }
        else
        {
            notEnoughCoinsPanel.SetActive(true);
        }

    } 
}