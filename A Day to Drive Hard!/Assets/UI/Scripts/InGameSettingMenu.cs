using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameSettingMenu : MonoBehaviour {

    public GameObject panel;

    //counter is set to 1 so the panel is not displayed
    int counter = 1;

    //function is used to display the pause menu when the pause button is pushed
    public void showHidePanel()
    {

        counter++;

        //if the number is odd dont display the pause menu - if even display menu
        if (counter % 2 == 1)
        {
            panel.gameObject.SetActive(false);
        }
        else
        {
            panel.gameObject.SetActive(true);
        }
    }
}
