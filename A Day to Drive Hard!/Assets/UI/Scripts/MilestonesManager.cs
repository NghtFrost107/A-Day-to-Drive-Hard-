using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Milestones
{
    JUST_THE_START = 1,
    BECOMING_A_PRO = 2,
    CHAMPION = 4,
    ROAD_TRIP = 8,
    AROUND_THE_WORLD = 16
       
}
public class MilestonesManager : MonoBehaviour {

    public GameObject milestonePrefab;
    public Sprite unlockedSprite;

    public static MilestonesManager Instance;

    public Dictionary<string, Milestone> milestonesDictionary = new Dictionary<string, Milestone>();

    public Database database;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Use this for initialization
    void Start () {
        setMiletones();
    }
    
    public void setMiletones()
    {
        CreateMilestone(Milestones.JUST_THE_START, "Milestones", "Just the Start:", "Score a total of 700 points in a single run", 700);
        CreateMilestone(Milestones.BECOMING_A_PRO, "Milestones", "Becoming a pro:", "Score a total of 1500 points in a single run", 1500, new string[] { "Just the Start" });
        CreateMilestone(Milestones.CHAMPION, "Milestones", "Champion:", "Score a total of 2000 points in a single run", 2000, new string[] { "Just the Start", "Becoming a Pro" });
        CreateMilestone(Milestones.ROAD_TRIP, "Milestones", "Road Trip:", "Travel a total of 2000m", 2000);
        CreateMilestone(Milestones.AROUND_THE_WORLD, "Milestones", "Around the World:", "Travel a total of 7000m", 7000, new string[] { "Road Trip" });
    }

    public void CreateMilestone(Milestones milestone, string category, string title, string description, int condition, string[] dependencies =null)
    {
        Milestone newMilestone;
        if ((Milestones)(database.player.Milestones & (int)milestone) == milestone)
        {
            newMilestone = new Milestone(milestone, title, description, condition, true);
        } else
        {
            newMilestone = new Milestone(milestone, title, description, condition, false);
        }
        milestonesDictionary.Add(title, newMilestone);

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

    public void DisplayMilestones(GameObject parent)
    {
        foreach (Milestone milestone in milestonesDictionary.Values) {
            GameObject milestonePanel = Instantiate(milestonePrefab);

            milestonePanel.transform.SetParent(parent.transform);
            milestonePanel.transform.localScale = new Vector3(1, 1, 1);
            milestonePanel.transform.GetChild(0).GetComponent<Text>().text = milestone.title;
            milestonePanel.transform.GetChild(1).GetComponent<Text>().text = milestone.description;
            if (milestone.Unlocked)
            {
                milestonePanel.GetComponent<Image>().sprite = MilestonesManager.Instance.unlockedSprite;
            }

        }
    }

    public void EarnMilestone(string title)
    {
        milestonesDictionary[title].EarnMilestone();
    }

    public void checkMilestones()
    {
        if (database.player.score >= 700)
        {
            EarnMilestone("Just the Start:");
            database.player.Milestones |= (int)Milestones.JUST_THE_START;
        }
        if (database.player.score >= 1500)
        {
            EarnMilestone("Becoming a pro:");
            database.player.Milestones |= (int)Milestones.BECOMING_A_PRO;
        }
        if (database.player.score >= 2000)
        {
            EarnMilestone("Champion:");
            database.player.Milestones |= (int)Milestones.CHAMPION;
        }
        if (database.player.LifetimeDistance >= 2000)
        {
            EarnMilestone("Road Trip:");
            database.player.Milestones |= (int)Milestones.ROAD_TRIP;
        }
        if (database.player.LifetimeDistance >= 7000)
        {
            EarnMilestone("Around the World:");
            database.player.Milestones |= (int)Milestones.AROUND_THE_WORLD;
        }
    }

}
