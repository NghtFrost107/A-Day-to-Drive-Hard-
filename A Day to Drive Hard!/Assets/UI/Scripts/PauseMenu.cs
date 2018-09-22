using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    public GameObject panel;

    
    int counter = 1;

    public void showHidePanel()
    {

        counter++;
        if (counter % 2 == 1)
        {
            panel.gameObject.SetActive(false);
        }
        else
        {
            panel.gameObject.SetActive(true);
        }
    }

    public void onPauseMenuToggle(bool active)
    {
        //GetComponent<>
    }

}
