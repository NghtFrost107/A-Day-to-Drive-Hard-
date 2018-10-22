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

    private Database database;

    //stores all the milestones for easy acces as well
    public Dictionary<string, Milestone> milestones = new Dictionary<string, Milestone>();

    // Use this for initialization
    void Start () {

        CreateMilestone("Milestones", "Just the Start:", "Score a total of 250 points in a single run");
        CreateMilestone("Milestones", "Becoming a pro:", "Score a total of 500 points in a single run", new string[] {"Just the Start"});
        CreateMilestone("Milestones", "Champion:", "Score a total of 1000 points in a single run", new string[] { "Just the Start" , "Becoming a Pro"});
        CreateMilestone("Milestones", "Road Trip:", "Travel a total of 2000m");
        CreateMilestone("Milestones", "Around the World:", "Travel a total of 7000m", new string[] { "Road Trip" });
        

    }
	

    public void CreateMilestone(string category, string title, string description, string[] dependencies =null)
    {

        GameObject milestone = (GameObject)Instantiate(milestonePrefabLocked);
        SetMilestoneInfo(category, milestone, title, description);


        Milestone newMilestone = new Milestone(title, description, milestone);

        //add to dictionary
        milestones.Add(title, newMilestone);

        if(dependencies != null)
        {
            foreach(string milestoneTitle in dependencies)
            {
                Milestone dependency = milestones[milestoneTitle];
                dependency.Child = title;
                newMilestone.addDependency(dependency);
            }
        }


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
        if (milestones[title].EarnMilestone())
        {
            //earn the milestone

        }
    }


}
