using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Milestone{

    private string title;
    private string description;
    private bool unlocked;
    private GameObject milestoneRef;
    private List<Milestone> dependencies = new List<Milestone>();
    private string child;

    public string Child
    {
        get { return child; }
        set { child = value; }
    }

    public Milestone(string title, string description, GameObject milestoneRef)
    {
        this.title = title;
        this.description = description;
        this.unlocked = false;
        this.milestoneRef = milestoneRef;

        LoadMilestone();
    }

    public void addDependency(Milestone dependency)
    {
        dependencies.Add(dependency);
    }

    public bool EarnMilestone()
    {
        //makes sure that all previous achievements are unlocked
        if (!unlocked && !dependencies.Exists(x => x.unlocked == false))
        {
            milestoneRef.GetComponent<Image>().sprite = MilestonesManager.Instance.unlockedSprite;
            SaveMilestone(true);

            if (child != null)
            {
                MilestonesManager.Instance.EarnMilestone(child);
            }
            return true;
        }
        return false;
    }

    public void SaveMilestone(bool value)
    {
        unlocked = value;
        //save milestone data locked/unlocked
        PlayerPrefs.SetInt(title, value ? 1 : 0);
        PlayerPrefs.Save();

    }

    public void LoadMilestone()
    {
        unlocked = PlayerPrefs.GetInt(title) == 1 ? true:false;
        milestoneRef.GetComponent<Image>().sprite = MilestonesManager.Instance.unlockedSprite;
    }
}
