using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScores : MonoBehaviour {

    public Database database;
    public Text scoresID;
    public Text highscore;
    public Text timestamp;

    public void ListScores()
    {
        CheckIfDatabaseLoaded();
        List<Score> playerScores = database.GetComponent<Database>().RetrieveScores();

        foreach (Score score in playerScores)
        {
            highscore.text += score.score + "\n";
            timestamp.text += score.time + " " + score.date + "\n";
        }
    }

    public void DeleteScores()
    {
        CheckIfDatabaseLoaded();
        database.GetComponent<Database>().EraseTable("Score");
        highscore.text = "Score\n-------------------\n";
        timestamp.text = "Time\n----------------------------------------------------\n";
    }

    private void CheckIfDatabaseLoaded() 
    {
        if (database == null)
        {
            database = Database.databaseObject;
        }
    }
}
