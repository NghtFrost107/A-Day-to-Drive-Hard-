using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MilestoneMenu : MonoBehaviour {

    [SerializeField]
    private Image completedTick;

    [SerializeField]
    private Button milestone;

    //used to check against the distance the user has travelled
    [SerializeField]
    private int TotalCondition;
    [SerializeField]
    private int SingleRunCondition;

    private int distanceTravelled;
    private int distanceSingleRun;

    private int[] distanceConditions = new int[3];
    private Database database;
    void Awake()
    {
        database = GameObject.FindGameObjectWithTag("Database").GetComponent<Database>();
    }

    public void setArray()
    {
        distanceTravelled = database.player.LifetimeDistance;
        distanceSingleRun = database.player.score;

        
        distanceConditions[0] = TotalCondition;
        distanceConditions[1] = SingleRunCondition;
        distanceConditions[2] = distanceTravelled;
        distanceConditions[3] = distanceSingleRun;

    }
    /*
     * if condition is met display the tick and disable button colour
     * 
     */

    public void milestoneMet(bool completed)
    {
        if (completed == true)
        {
            completedTick.enabled = true;
            milestone.GetComponent<Button>().interactable = false;
        }
    }

    public void checkDistanceTravelledTotal()
    {
        distanceTravelled = database.player.LifetimeDistance;

        if (distanceConditions[2] >= distanceConditions[0])
        {
            milestoneMet(true);
        }

    }

    public void checkDistanceSingleRun()
    {
        distanceSingleRun = database.player.score;

        if (distanceConditions[3] >= distanceConditions[1])
        {
            milestoneMet(true);
        }

    }

}
