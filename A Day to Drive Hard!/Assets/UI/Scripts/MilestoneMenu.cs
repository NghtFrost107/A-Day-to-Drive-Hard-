using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MilestoneMenu : MonoBehaviour {

    [SerializeField]
    private Image completedTick;

    private Database database;
    void Awake()
    {
        database = GameObject.FindGameObjectWithTag("Database").GetComponent<Database>();
    }

    /*
     * if condition is met display the tick and disbale button colour
     * 
     */

    public void milestoneMet()
    {

    }
}
