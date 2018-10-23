using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Milestone {

    public Milestones milestone;
    public string title;
    public string description;
    public bool Unlocked { get; private set; }
    private int condition;
    private List<Milestone> dependencies = new List<Milestone>();
    private string child;

    public string Child
    {
        get { return child; }
        set { child = value; }
    }

    public Milestone(Milestones milestone, string title, string description, int condition, bool unlocked)
    {
        this.title = title;
        this.description = description;
        this.condition = condition;
        Unlocked = unlocked;

        LoadMilestone();
    }

    public void addDependency(Milestone dependency)
    {
        dependencies.Add(dependency);
    }

    public bool EarnMilestone()
    {
        //makes sure that all previous achievements are unlocked
        if (!Unlocked && !dependencies.Exists(x => x.Unlocked == false))
        {
            Unlocked = true;

            if (child != null)
            {
                MilestonesManager.Instance.EarnMilestone(child);
            }
            return true;
        }
        return false;
    }

    public void LoadMilestone()
    {

        
    }
}
