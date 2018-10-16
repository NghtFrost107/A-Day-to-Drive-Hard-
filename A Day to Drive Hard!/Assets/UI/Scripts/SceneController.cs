using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

    public void LoadSceneOnClick(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);

        if (gameObject.tag == "Game")
        {
            Database database = GameObject.Find("Database").GetComponent<Database>();
            database.player.LifetimeDistance += Mathf.RoundToInt(database.player.currentPosition);
            database.SetPlayerData();
            database.AddScore(new Score()
            {
                time = System.DateTime.Now.ToShortTimeString(),
                date = System.DateTime.Now.ToShortDateString(),
                score = Mathf.RoundToInt(database.player.currentPosition)
            });
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

    }
}
