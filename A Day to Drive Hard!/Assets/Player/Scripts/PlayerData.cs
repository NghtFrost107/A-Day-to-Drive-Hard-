using SQLite4Unity3d;
using System.Collections.Generic;

public class PlayerData
{
    [PrimaryKey]
    public int ID { get; set; }
    public int PlayerCoins { get; set; }
    public int MAX_PLAYER_HEALTH { get; set; }
    public int PlayerSpeed { get; set; }
    public int LifetimeDistance { get; set; }

    public int score;
    public float currentPosition;
    public int fullFlips;
    public int halfFlips;
    public int quarterFlips;
    public int playerHealth;

    //stores all the milestones for easy acces as well
    public Dictionary<string, Milestone> milestonesDictionary = new Dictionary<string, Milestone>();

    public void resetCurrentGameStatistics()
    {
        score = 0;
        currentPosition = 0;
        fullFlips = 0;
        halfFlips = 0;
        quarterFlips = 0;
        playerHealth = MAX_PLAYER_HEALTH;
    }

    //Add more variables for more properties added to the car
}

