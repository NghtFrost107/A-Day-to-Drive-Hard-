using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthCounter : MonoBehaviour {

    private Text textHealth;
    private PlayerController PC;
    void Awake()
    {
        textHealth = GetComponent<Text>();
    }

	void Update ()
    {
        textHealth.text = "Health: " + PC.playerHealth.ToString();

	}
}
