using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScores : MonoBehaviour {

    public GameObject database;
    public GameObject scorePanel;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ListScores()
    {
        List<Score> playerScores = database.GetComponent<Database>().retrieveScores();

        foreach (Score score in playerScores)
        {
            scorePanel.GetComponent<Text>().text += score.ToString() + "\n";
        }
    }
}
