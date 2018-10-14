using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;
using System.IO;

public class Database : MonoBehaviour {

    public static Database databaseObject;

    private SQLiteConnection connection;
    public PlayerData player;

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
}

