using UnityEngine;
using System.Collections;
 
public class EngineSound : MonoBehaviour
{
    //audiosource reference
    private AudioSource carSound;
    private AudioSource musicSound;
    public float carVolume;
    public float musicVolume;


    //the range for audio source pitch
    private const float lowPtich = 0.5f;
    private const float highPitch = 5f;

    //change the reductionFactor to 0.1f if you are using the rigidbody velocity as parameter to determine the pitch
    private const float reductionFactor = .001f;

    //Rigidbody2D carRigidbody;
    private float userInput;

    private static bool goingRight;
    private static bool goingLeft;

    //wheeljoint2d reference
    WheelJoint2D wj;

    void Awake()
    {
        //get the Audio Source component attached to the car
        carSound = GetComponent<AudioSource>();
        //get the wheelJoint2D component attached to the car
        wj = GetComponent<WheelJoint2D>();
        //carRigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        //get the userInput
        if (goingRight)
        {
            userInput = getRightValue();
            notGoingLeft();
        }
        else if (goingLeft)
        {
            userInput = getLeftValue();
            notGoingRight();
        }
        else
        {
            userInput = carIsIdle();
        }

        //userInput = Input.GetAxis("Horizontal");
        //get the absolute value of jointSpeed
        float forwardSpeed = Mathf.Abs(wj.jointSpeed);
        //float forwardSpeed = transform.InverseTransformDirection(carRigidbody.velocity).x;
        //calculate the pitch factor which will be added to the audio source
        float pitchFactor = Mathf.Abs(forwardSpeed * reductionFactor * userInput);
        //clamp the calculated pitch factor between lowPitch and highPitch
        carSound.pitch = Mathf.Clamp(pitchFactor, lowPtich, highPitch);
    }

    public void isGoingRight()
    {
        goingRight = true;
    }

    public void notGoingRight()
    {
        goingRight = false;
    }

    public void isGoingLeft()
    {
        goingLeft = true;
    }

    public void notGoingLeft()
    {
        goingLeft = false;
    }

    public float getRightValue()
    {
        return 1.0f;
    }

    public float getLeftValue()
    {
        return -1.0f;
    }

    public float carIsIdle()
    {
        return 0.0f;
    }
}