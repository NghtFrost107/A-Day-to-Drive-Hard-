using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScript : MonoBehaviour
{
    private bool canJump = true;
    private float jumpStart = 0f;

	// Use this for initialization
	void Start()
    {
		
	}
	
	// Update is called once per frame
	void Update()
    {
        if(canJump)
        {
            if (Input.GetKeyDown("j"))
            {
                //transform.Translate(Vector3.up * 200 * Time.deltaTime, Space.World);
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(15, 35f);
                canJump = false;

                Invoke("jump", 2);
            }
        }
	}

    public void jump()
    {
        canJump = true;
    }
}
