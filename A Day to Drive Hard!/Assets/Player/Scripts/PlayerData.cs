using SQLite4Unity3d;

public class PlayerData
{
    [PrimaryKey]
    public int ID { get; set; }
    public int PlayerCoins { get; set; }
    public int MAX_PLAYER_HEALTH { get; set; }
    public int PlayerSpeed { get; set; }
    public int LifetimeDistance { get; set; }

    public float currentPosition;
    public int playerHealth;

    //Add more variables for more properties added to the car
}

