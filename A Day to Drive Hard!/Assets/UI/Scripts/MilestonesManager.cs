using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MilestonesManager : MonoBehaviour {

    public GameObject milestonePrefabLocked;
    public GameObject milestonePrefabUnlocked;
    public Sprite unlockedSprite;

    private static MilestonesManager instance;

    public static MilestonesManager Instance
    {
        get {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<MilestonesManager>();
            }
            return MilestonesManager.instance;
        }
    }

    

    //stores all the milestones for easy acces as well
    public Dictionary<string, Milestone> milestones = new Dictionary<string, Milestone>();

    private Database database;

    // Use this for initialization
    void Start () {

        setMiletones();
        database = GameObject.FindGameObjectWithTag("Database").GetComponent<Database>();
        Console.WriteLine(database.player.score);



}
    
    public void setMiletones()
    {
        CreateMilestone("Milestones", "Just the Start:", "Score a total of 250 points in a single run", 250);
        CreateMilestone("Milestones", "Becoming a pro:", "Score a total of 500 points in a single run", 500, new string[] { "Just the Start" });
        CreateMilestone("Milestones", "Champion:", "Score a total of 1000 points in a single run", 1000, new string[] { "Just the Start", "Becoming a Pro" });
        CreateMilestone("Milestones", "Road Trip:", "Travel a total of 2000m", 2000);
        CreateMilestone("Milestones", "Around the World:", "Travel a total of 7000m", 7000, new string[] { "Road Trip" });
    }

    public void CreateMilestone(string category, string title, string description, int condition, string[] dependencies =null)
    {

        GameObject milestone = (GameObject)Instantiate(milestonePrefabLocked);
  
        Milestone newMilestone = new Milestone(title, description, condition, milestone);
        //add to dictionary
        milestones.Add(title, newMilestone);
        SetMilestoneInfo(category, milestone, title, description);

/*
        if(dependencies != null)
        {
            foreach(string milestoneTitle in dependencies)
            {
                Milestone dependency = milestones[milestoneTitle];
                dependency.Child = title;
                newMilestone.addDependency(dependency);
            }
        }
*/

    }

    public void SetMilestoneInfo(string category, GameObject milestone,string title, string description)
    {
        milestone.transform.SetParent(GameObject.Find(category).transform);
        milestone.transform.localScale = new Vector3(1, 1, 1);
        milestone.transform.GetChild(0).GetComponent<Text>().text = title;
        milestone.transform.GetChild(1).GetComponent<Text>().text = description;

    }

    public void EarnMilestone(string title)
    {
        milestones[title].EarnMilestone();
    }

    public void checkMilestones()
    {
        foreach (KeyValuePair<string, Milestone> pair in milestones)
        {
            if (database.player.score >= 250)
            {
                EarnMilestone("Just the Start");
            }
            if (database.player.score >= 500)
            {
                EarnMilestone("Becoming a pro");
            }
            if (database.player.score >= 1000)
            {
                EarnMilestone("Champion");
            }
            if (database.player.LifetimeDistance >= 2000)
            {
                EarnMilestone("Road Trip");
            }
            if (database.player.LifetimeDistance >= 2000)
            {
                EarnMilestone("Road Trip");
            }
            if (database.player.LifetimeDistance >= 7000)
            {
                EarnMilestone("Around the World");
            }
        }
    }

}
