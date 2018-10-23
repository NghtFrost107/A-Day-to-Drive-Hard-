using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

    public bool playerInvincible;
    public Text playerCoinCounter;
    public Text playerHealthCounter;
    public Text gameOverMessage;
    public Text distanceCounter;
    public SpriteRenderer carBody;
    public SpriteRenderer backWheel;
    public SpriteRenderer frontWheel;
    public SpriteRenderer shieldOverlay;

    private bool playerDead = false;
    private MilestonesManager milestones;
    private Database database;
    void Awake()
    {
        database = GameObject.FindGameObjectWithTag("Database").GetComponent<Database>();
    }
    // Use this for initialization
    void Start () {
        SetHealthCounter();
        SetCoinCounter();
        
        //starts game with gameover message hidden
        gameOverMessage.text = "";

        //Disable the shieldOverlay on startup
        shieldOverlay.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {

        if (!playerDead)
        {
            if (database.player.playerHealth <= 0)
            {
                EndScreen("You ran out of lives!");
            }
            else
            {
                database.player.currentPosition = transform.GetChild(0).transform.position.x;
                distanceCounter.text = "Distance Travelled: " + Mathf.Round(database.player.currentPosition) + "m";
            }
        }

        if (playerInvincible == false)
        {
            carBody.color = Color.white;
            frontWheel.color = Color.white;
            backWheel.color = Color.white;
        }


        milestones.checkMilestones();

	}

    public void EndScreen(string reasonForExit)
    {
        playerDead = true;
        gameOverMessage.gameObject.SetActive(true);
        database.player.score = Mathf.RoundToInt(database.player.currentPosition) + database.player.fullFlips * 10 + database.player.halfFlips * 5 + database.player.quarterFlips * 2;
        gameOverMessage.text = reasonForExit + "\n\n" +
            "Distance Travelled: " + Mathf.RoundToInt(database.player.currentPosition) + "m\n\n" +
            "Full Flips: x" + database.player.fullFlips + " x 10 = " + database.player.fullFlips * 10 + "\n\n" +
            "Half Flips: x" + database.player.halfFlips + " x 5 = " + database.player.halfFlips * 5 + "\n\n" +
            "Quarter Flips: x" + database.player.quarterFlips + " x 2 = " + database.player.quarterFlips * 2 + "\n\n" +
            "Total Score: " + database.player.score;

        //Return to main menu after a 3 second delay
        Invoke("ReturnToMenu", 5);
    }
    //What to do if the player has collided with an obstacle
    public void ObstacleCollision(Collider2D col)
    {
        if (playerInvincible == false)
        {
            Destroy(col.gameObject);
            database.player.playerHealth--;
            SetHealthCounter();
            Debug.Log("Player health:" + database.player.playerHealth);

            PlayerSetInvincible(3);
        }
        
    }

    //Player Collides with pickup
    public void PickupCollision(Collider2D col)
    {
        database.player.PlayerCoins++;
        SetCoinCounter();
        Destroy(col.gameObject);
    }

    //Player collides with shield powerup
    public void ShieldCollision(Collider2D col)
    {
        playerInvincible = true;
        Destroy(col.gameObject);
        ShieldPowerUp(10);
    }

    //Player collides with heart powerup
    public void HeartCollision(Collider2D col)
    {
        database.player.playerHealth++;
        SetHealthCounter();
        Destroy(col.gameObject);
    }

    //Returns Player to main menu
    void ReturnToMenu()
    {
        database.player.playerHealth = database.player.MAX_PLAYER_HEALTH;
        database.player.LifetimeDistance += (int)Mathf.Round(database.player.currentPosition);
        database.AddScore(new Score()
        {
            time = System.DateTime.Now.ToShortTimeString(),
            date = System.DateTime.Now.ToShortDateString(),
            distance = Mathf.RoundToInt(database.player.currentPosition),
            score = database.player.score
        });
        database.SetPlayerData();
        database.player.resetCurrentGameStatistics();
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }

    void ShieldPowerUp(int waitTime)
    {
        shieldOverlay.enabled = true;
        playerInvincible = true;

        Invoke("PlayerSetDamageable", waitTime);
    }

    //Sets playerinvincible for a time
    void PlayerSetInvincible(int waitTime)
    {
        playerInvincible = true;

        //Calls FlashSprite every 33.3 milliseconds for duration of invincibility
        InvokeRepeating("FlashSprite", 0.0f, 0.3f); 

        //Call PlayerSetDamageable after int waitTime in seconds
        Invoke("PlayerSetDamageable", waitTime);
    }

    //Sets the player damageable
    void PlayerSetDamageable()
    {
        //Resetting the color so the player is not stuck invisible
        carBody.color = Color.white;
        frontWheel.color = Color.white;
        backWheel.color = Color.white;

        playerInvincible = false;
        shieldOverlay.enabled = false;
        CancelInvoke("FlashSprite"); //Stops sprite flashing
    }

    //Flashes Sprite
    void FlashSprite()
    {
        if (carBody.color == Color.white)
        {
            carBody.color = Color.clear;
            frontWheel.color = Color.clear;
            backWheel.color = Color.clear;
        }
        else
        {
            carBody.color = Color.white;
            frontWheel.color = Color.white;
            backWheel.color = Color.white;
        }
    }

    // Health Counter
    void SetHealthCounter()
    {
        playerHealthCounter.text = "Health: " + database.player.playerHealth;
    }

    // Coin Counter
    void SetCoinCounter()
    {
        playerCoinCounter.text = "Coins: " + database.player.PlayerCoins;
    }
}
