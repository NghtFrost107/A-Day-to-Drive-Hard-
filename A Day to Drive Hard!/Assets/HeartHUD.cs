using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartHUD : MonoBehaviour {
    public Sprite[] HeartSprites;
    public Image HeartUI;
    public Player player;
    private Database database;

    private void Awake()
    {
        database = GameObject.FindGameObjectWithTag("Database").GetComponent<Database>();
    }

    private void Update()
    {
        try
        {
            HeartUI.sprite = HeartSprites[database.player.playerHealth];
        }
        catch (System.IndexOutOfRangeException e)
        {
            Debug.Log(e, player);
        }
        
    }
}
