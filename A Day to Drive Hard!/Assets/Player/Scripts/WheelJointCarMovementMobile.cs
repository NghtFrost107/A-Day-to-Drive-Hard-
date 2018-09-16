using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelJointCarMovementMobile : MonoBehaviour
{

    //reference to the wheel joints
    WheelJoint2D[] wheelJoints;
    //center of mass of the car
    public Transform centerOfMass;
    //reference tot he motor joint
    JointMotor2D motorBack;
    //horizontal movement keyboard input
    public float dir = 0f;
    //input for rotation of the car
    float torqueDir = 0f;
    //max fwd speed which the car can move at
    float maxFwdSpeed = -8000;
    //max bwd speed
    float maxBwdSpeed = 1000f;
    //the rate at which the car accelerates
    float accelerationRate = 1000;
    //the rate at which car decelerates
    float decelerationRate = -100;
    //how soon the car stops on braking
    float brakeSpeed = 2500f;
    //acceleration due to gravity
    float gravity = 1;// 9.81f;
    //angle in which the car is at wrt the ground
    float slope = 0;
    //reference to the wheels
    public Transform rearWheel;
    public Transform frontWheel;
    public Rigidbody2D rb2d;

    private bool accelerateIsPressed;
    public bool decelerateIsPressed;
    public bool brakeIsPressed;

    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();

        //set the center of mass of the car
        rb2d.centerOfMass = centerOfMass.transform.localPosition;
        //get the wheeljoint components
        wheelJoints = gameObject.GetComponents<WheelJoint2D>();
        //get the reference to the motor of rear wheels joint
        motorBack = wheelJoints[0].motor;
    }

    void Update()
    {
        if (accelerateIsPressed)
        {
            dir = 1;
            torqueDir = 1;
        }
        else if (decelerateIsPressed)
        {
            dir = -1;
            torqueDir = -1;
        }
        else if (brakeIsPressed && motorBack.motorSpeed > 0)
        {
            motorBack.motorSpeed = Mathf.Clamp(motorBack.motorSpeed - brakeSpeed * Time.deltaTime, 0, maxBwdSpeed);
        }
        else if (brakeIsPressed && motorBack.motorSpeed < 0)
        {
            motorBack.motorSpeed = Mathf.Clamp(motorBack.motorSpeed + brakeSpeed * Time.deltaTime, maxFwdSpeed, 0);
        }
        else
        {
            dir = 0;
            torqueDir = 0;
        }
    }

    //all physics based assignment done here
    void FixedUpdate()
    {
        //add ability to rotate the car around its axis
        
        //torqueDir = Input.GetAxis("Horizontal");
        if (torqueDir != 0)
        {
            rb2d.AddTorque(-15 * Mathf.PI * torqueDir, ForceMode2D.Force);
        }
        else
        {
            rb2d.AddTorque(0);
        }

        //determine the cars angle wrt the horizontal ground
        slope = transform.localEulerAngles.z;

        //convert the slope values greater than 180 to a negative value so as to add motor speed 
        //based on the slope angle
        if (slope >= 180)
            slope = slope - 360;
        
        //horizontal movement input. same as torqueDir. Could have avoided it, but decided to 
        //use it since some of you might want to use the Vertical axis for the torqueDir

        //dir = Input.GetAxis("Horizontal");
        if (dir != 0)
            //add speed accordingly
            motorBack.motorSpeed = Mathf.Clamp(motorBack.motorSpeed - (dir * accelerationRate - gravity * Mathf.Sin((slope * Mathf.PI) / 180) * 80) * Time.deltaTime, maxFwdSpeed, maxBwdSpeed);

        //if no input and car is moving forward or no input and car is stagnant and is on an inclined plane with negative slope
        if ((dir == 0 && motorBack.motorSpeed < 0) || (dir == 0 && motorBack.motorSpeed == 0 && slope < 0))
        {
            //decelerate the car while adding the speed if the car is on an inclined plane
            motorBack.motorSpeed = Mathf.Clamp(motorBack.motorSpeed - (decelerationRate - gravity * Mathf.Sin((slope * Mathf.PI) / 180) * 80) * Time.deltaTime, maxFwdSpeed, 0);
        }
        //if no input and car is moving backward or no input and car is stagnant and is on an inclined plane with positive slope
        else if ((dir == 0 && motorBack.motorSpeed > 0) || (dir == 0 && motorBack.motorSpeed == 0 && slope > 0))
        {
            //decelerate the car while adding the speed if the car is on an inclined plane
            motorBack.motorSpeed = Mathf.Clamp(motorBack.motorSpeed - (-decelerationRate - gravity * Mathf.Sin((slope * Mathf.PI) / 180) * 80) * Time.deltaTime, 0, maxBwdSpeed);
        }

        //apply brakes to the car
        //if (brakeIsPressed && motorBack.motorSpeed > 0)
        //{
        //    motorBack.motorSpeed = Mathf.Clamp(motorBack.motorSpeed - brakeSpeed * Time.deltaTime, 0, maxBwdSpeed);
        //}
        //else if (brakeIsPressed && motorBack.motorSpeed < 0)
        //{
        //    motorBack.motorSpeed = Mathf.Clamp(motorBack.motorSpeed + brakeSpeed * Time.deltaTime, maxFwdSpeed, 0);
        //}
        //connect the motor to the joint
        wheelJoints[0].motor = motorBack;
        wheelJoints[1].motor = motorBack;
    }

    public void acceleratePressed()
    {
        accelerateIsPressed = true; ;
    }

    public void accelerateNotPressed()
    {
        accelerateIsPressed = false;
    }

    public void deceleratePressed()
    {
        decelerateIsPressed = true; ;
    }

    public void decelerateNotPressed()
    {
        decelerateIsPressed = false;
    }

    public void brakePressed()
    {
        brakeIsPressed = true;
    }

    public void brakeNotPressed()
    {
        brakeIsPressed = false;
    }
}