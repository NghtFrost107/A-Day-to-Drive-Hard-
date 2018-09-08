using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneonClick : MonoBehaviour {

    public void LoadSceneOnCick(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
