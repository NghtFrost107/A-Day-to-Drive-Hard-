using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;
using System.IO;

public class Database : MonoBehaviour {

    private SQLiteConnection connection;

    public PlayerData player;

    private string databaseName = "DayToDriveHardDB.db";
	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
        InitialiseDB();

	}
	
    void InitialiseDB()
    {
        connection = new SQLiteConnection(Application.persistentDataPath + "/" + databaseName);

        connection.CreateTable<PlayerData>();
        connection.CreateTable<Score>();

        retrievePlayerData();
    }


    public void retrievePlayerData()
    {
        List<PlayerData> playerTable = connection.Query<PlayerData>("SELECT * FROM PlayerData", new object[0]);

        if (playerTable.Count == 0)
        {
            player = new PlayerData()
            {
                playerCoins = 500,
                MAX_PLAYER_HEALTH = 5,
                playerHealth = 5,
                playerSpeed = 2000
            };
        }
        else
        {
            player = playerTable[0];
            player.playerHealth = player.MAX_PLAYER_HEALTH;
        }
    }

    public List<Score> retrieveScores()
    {
        return connection.Query<Score>("SELECT * FROM Score", new object[0]);
    }

    public void addScore(Score score)
    {
        connection.Insert(score);
    }

    public void setPlayerData()
    {
        connection.Delete<PlayerData>(0);
        connection.Insert(player);
    }
}

