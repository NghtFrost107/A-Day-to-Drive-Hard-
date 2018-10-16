using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScript : MonoBehaviour
{
    private bool canJump = true;
    //private Collider myCollider;
    // public float distanceToGround = 0f;
    

	// Use this for initialization
	/*void Start()
    {
        //myCollider = GetComponent<Collider>();
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        // distanceToGround = myCollider.bounds.extents.y;

        if (canJump && Input.GetKeyDown("j")) // && isGrounded())
        {
            // Vector2(Right Velocity, Up Velocity)
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(8, 10f);
            canJump = false;

            Invoke("jump", 2);
        }
    }*/

    //public bool isGrounded()
    //{
    //    return Physics.Raycast(transform.position, -Vector3.up, distanceToGround + 0.3f);
    //}

    public void jumpButton()
    {
        if(canJump) // && isGrounded())
        {
            // Vector2(Right Velocity, Up Velocity)
            GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 10f);
            canJump = false;

            Invoke("jump", 2);
        }
    }

    public void jump()
    {
        canJump = true;
    }

    
}
