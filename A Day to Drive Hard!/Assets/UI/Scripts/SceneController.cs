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
            GameObject.Find("Database").GetComponent<Database>().addScore(new Score()
            {
                time = System.DateTime.Now.ToShortTimeString(),
                date = System.DateTime.Now.ToShortDateString(),
                score = 11
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
