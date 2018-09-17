using UnityEngine;

public class PlayerStats : MonoBehaviour {

    public static PlayerStats instance;

    private int maxHealth = 100;

    public int MaxHealth
    {
        get
        {
            return maxHealth;
        }

        set
        {
            maxHealth = value;
        }
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

}
