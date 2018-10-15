using SQLite4Unity3d;

public class PlayerData
{
    [PrimaryKey]
    public int ID { get; set; }
    public int playerCoins { get; set; }
    public int MAX_PLAYER_HEALTH { get; set; }
    public int playerSpeed { get; set; }
    public int totalDistanceTravelled { get; set; }

    public float currentPosition;
    public int playerHealth;

    //Add more variables for more properties added to the car
}

