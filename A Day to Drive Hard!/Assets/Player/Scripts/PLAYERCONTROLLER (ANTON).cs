using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLAYERCONTROLLER : MonoBehaviour
{
    WheelJoint2D[] wheelJoints;
    public Transform centerOfMass;
    JointMotor2D motorBack;
    public float dir = 0f;
    float torqueDir = 0f;
    float maxFwdSpeed = -8000;
    float maxBwdSpeed = 1000f;
    float accelerationRate = 1000;
    float decelerationRate = -100;
    float brakeSpeed = 2500f;
    float gravity = 1;// 9.81f;
    float slope = 0;
    public Transform rearWheel;
    public Transform frontWheel;
    public Rigidbody2D rb2d;
    public bool accelerateIsPressed;
    public bool decelerateIsPressed;
    public bool brakeIsPressed;

    // Use this for initialization
    void Start()
    {
		
	}
	
	// Update is called once per frame
	void Update()
    {
		
	}
}
