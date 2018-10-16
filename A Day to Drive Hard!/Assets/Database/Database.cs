using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;

public class Database : MonoBehaviour {

    public static Database databaseObject;

    private SQLiteConnection connection;
    public PlayerData player;
    public Score score;

    private string databaseName = "DayToDriveHardDB.db";
	// Use this for initialization
	void Awake() {
        if (databaseObject == null)
        {
            databaseObject = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
	}

    void Start()
    {
        InitialiseDB();
    }

    void InitialiseDB()
    {
        connection = new SQLiteConnection(Application.persistentDataPath + "/" + databaseName);

        //connection.DropTable<PlayerData>(); 
        connection.CreateTable<PlayerData>();
        connection.CreateTable<Score>();

        RetrievePlayerData();
    }


    public void RetrievePlayerData()
    {
        List<PlayerData> playerTable = connection.Query<PlayerData>("SELECT * FROM PlayerData", new object[0]);

        if (playerTable.Count == 0)
        {
            player = new PlayerData()
            {
                PlayerCoins = 20,
                MAX_PLAYER_HEALTH = 3,
                playerHealth = 3,
                PlayerSpeed = 2000,
                LifetimeDistance = 0          //<-----added this 
            };
        }
        else
        {
            player = playerTable[0];
            player.playerHealth = player.MAX_PLAYER_HEALTH;
        }
    }

    public List<Score> RetrieveScores()
    {
        return connection.Query<Score>("SELECT * FROM Score ORDER BY score DESC LIMIT 10", new object[0]);
    }

    public void AddScore(Score score)
    {
        connection.Insert(score);
    }

    public void SetPlayerData()
    {
        connection.Delete<PlayerData>(0);
        connection.Insert(player);
    }

    public void EraseTable(string table)
    {
        switch(table)
        {
            case "Score":
                {
                    connection.DeleteAll<Score>();
                }break;
            case "PlayerData":
                {
                    connection.DeleteAll<PlayerData>();
                } break;
        }
    }


    //is tthis where i put it?
    public void totalDistance()
    {
        player.LifetimeDistance = player.LifetimeDistance + score.score;
    }
}

