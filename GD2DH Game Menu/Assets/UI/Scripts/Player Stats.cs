using UnityEngine;

public class PlayerStats : MonoBehaviour {

    public static PlayerStats instance;

    public int maxHealth = 100;

    

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

}
