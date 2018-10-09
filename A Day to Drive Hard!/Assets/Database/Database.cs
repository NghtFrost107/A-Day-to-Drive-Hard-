using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;
using System.IO;

public class Database : MonoBehaviour {

    private SQLiteConnection connection;

    private string databaseName = "DayToDriveHardDB";
	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
        InitialiseDB();

	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void InitialiseDB()
    {
        //var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/");
        //loadDb.
#if UNITY_ANDROID && !UNITY_EDITOR
        connection = new SQLiteConnection(Application.persistentDataPath + "/" + databaseName);
#else 
        connection = new SQLiteConnection(@"Assets/StreamingAssets/" + databaseName);
#endif

        createDB();
    }

    void createDB()
    {
        //connection.DropTable<PlayerData>();
        connection.CreateTable<PlayerData>();

       // connection.Query()


        //Class1 test = connection.Table<Class1>().Where(x => x.Name = "Test").First
        List<PlayerData> list = connection.Query<PlayerData>("SELECT * FROM Class1", new object[0]);

        foreach (PlayerData test in list)
        {
            Debug.Log(test.ToString());
        }
        
    }
}

