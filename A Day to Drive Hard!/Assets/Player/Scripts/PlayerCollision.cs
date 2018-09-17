using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public PlayerProperites playerProperties;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // What initially happens when the player collides with an object
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            //Call ObstacleCollision in playerProperties script
            playerProperties.ObstacleCollision();
        }

        if (other.gameObject.CompareTag("PickUp"))
        {
            //TODO: Make a player collided with pickup method in playerProperties Class
            playerProperties.PickupCollision(other);
        }
    }
}

