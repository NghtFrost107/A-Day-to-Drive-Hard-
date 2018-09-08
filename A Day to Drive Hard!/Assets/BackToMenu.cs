using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToMenu : MonoBehaviour {

    public void BackButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
    }
}
