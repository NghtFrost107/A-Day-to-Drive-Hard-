using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentHealth : MonoBehaviour
{

    private Text healthText;
    private PlayerController PC;


    private void Awake()
    {
        healthText = GetComponent<Text>();
    }

}
