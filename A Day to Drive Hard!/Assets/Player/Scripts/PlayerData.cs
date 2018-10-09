using UnityEngine;
using System.IO;

using SQLite4Unity3d;

public class PlayerData
{

    public static int playerCoins;
    public static int playerHealth;
    public static int playerSpeed = 2000;
   
    //Add more variables for more properties added to the car

    public static int MAX_PLAYER_HEALTH;

    static PlayerData()
    {
        ReadSaveState();
    }

    public static void ReadSaveState()
    {
        try
        {
            StreamReader sr = new StreamReader(Application.persistentDataPath + "/SaveState.txt");
            int.TryParse(sr.ReadLine(), out playerCoins);
            int.TryParse(sr.ReadLine(), out MAX_PLAYER_HEALTH);
            int.TryParse(sr.ReadLine(), out playerSpeed);

            sr.Dispose();
            
            playerHealth = MAX_PLAYER_HEALTH;
        }
        catch (FileNotFoundException)
        {
            Debug.Log(Application.persistentDataPath);
            Debug.Log("The file was not found!, New Save file will be created");
            

        }
        catch (IOException)
        {
            Debug.Log("There was an error in the file");
        }
        finally
        {
            if (MAX_PLAYER_HEALTH == 0)
            {
                //Setting all values to the base value, in case the player has no save file
                playerCoins = 900; //For testing the default number of coins is 900 (Would normally be 0)
                MAX_PLAYER_HEALTH = playerHealth = 3;
                WriteSaveState();
            }
        }
    }

    //Updating the savestate with any new coins collected
    public static void WriteSaveState()
    {
        try
        {
            StreamWriter sw = new StreamWriter(Application.persistentDataPath + "/SaveState.txt");
            sw.WriteLine(playerCoins);
            sw.WriteLine(MAX_PLAYER_HEALTH);
            sw.WriteLine(playerSpeed);

            sw.Dispose();
        }
        catch (IOException)
        {
            Debug.Log("There was an error writing the save state");
        }
    }
}

