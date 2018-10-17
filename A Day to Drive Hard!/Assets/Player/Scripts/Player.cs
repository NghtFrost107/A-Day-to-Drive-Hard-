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

    private bool playerDead = false;

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
	}
	
	// Update is called once per frame
	void Update () {

        
        if (database.player.playerHealth <= 0)
        {
            gameOverMessage.text = "Out of Lives!\n" + "Game Over!\n" + "Returning To Main Menu!";

            if (!playerDead)
            {
                //Return to main menu after a 3 second delay
                Invoke("ReturnToMenu", 3);
                playerDead = true;
            }
        } else
        {
            database.player.currentPosition = transform.GetChild(0).transform.position.x;
            distanceCounter.text = "Distance Travelled: " + Mathf.Round(database.player.currentPosition) + "m";
        }
	}

    //What to do if the player has collided with an obstacle
    public void ObstacleCollision()
    {
        if (playerInvincible == false)
        {
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
        CancelInvoke(); //Gets rid of any leftover invokes from previous collisions

        //Set Player Invincible for 10 secs
        PlayerSetInvincible(10);

        Destroy(col.gameObject);
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
            score = (int)Mathf.Round(database.player.currentPosition)
        });
        database.SetPlayerData();
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
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
        playerInvincible = false;
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
