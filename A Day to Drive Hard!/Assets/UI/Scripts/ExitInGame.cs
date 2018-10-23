using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitInGame : MonoBehaviour {

    public GameObject pausePanel;
    public GameObject player;

	public void BackToMenu()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1;
        player.GetComponent<Player>().EndScreen("You exited the game!");
    }
}
