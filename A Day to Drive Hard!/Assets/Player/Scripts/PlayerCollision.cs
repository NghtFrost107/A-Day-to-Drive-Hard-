using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public Player player;

    // What initially happens when the player collides with an object
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            //Call ObstacleCollision in playerProperties script
            player.ObstacleCollision();
        }

        if (other.gameObject.CompareTag("PickUp"))
        {
            player.PickupCollision(other);
        }

        if (other.gameObject.CompareTag("Shield"))
        {
            player.ShieldCollision(other);
        }

        if (other.gameObject.CompareTag("Heart"))
        {
            player.HeartCollision(other);
        }
    }
}

