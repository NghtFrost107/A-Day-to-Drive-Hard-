using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Flip {
    FRONTFLIP,
    BACKFLIP,
    NOFLIP        
}

public class CombinedMovementScript : MonoBehaviour
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

    public Text stuntText;
    public Transform canvasObject;

    public bool accelerateIsPressed;
    public bool decelerateIsPressed;
    public bool brakeIsPressed;

    private Flip flipStatus = Flip.NOFLIP;
    private bool halfway;
    // Use this for initialization
    void Start ()
    {
        rb2d = GetComponent<Rigidbody2D>();

        //set the center of mass of the car
        rb2d.centerOfMass = centerOfMass.transform.localPosition;
        //get the wheeljoint components
        wheelJoints = gameObject.GetComponents<WheelJoint2D>();
        //get the reference to the motor of rear wheels joint
        motorBack = wheelJoints[0].motor;
    }
	
	// Update is called once per frame
	void Update ()
    {
        float currentAngle = gameObject.transform.eulerAngles.z;
        if (currentAngle <= 270 && currentAngle > 180)
        {
            if(flipStatus == Flip.NOFLIP)
            {
                flipStatus = Flip.FRONTFLIP;
            }
            
            if(flipStatus == Flip.BACKFLIP)
            {
                halfway = true;
            }

            
        }
        else if (currentAngle >= 90 && currentAngle <= 180)
        {
            if (flipStatus == Flip.NOFLIP)
            {
                flipStatus = Flip.BACKFLIP;
            }
            if (flipStatus == Flip.FRONTFLIP)
            {
                halfway = true;
            }
        }
        else if(flipStatus != Flip.NOFLIP)
        {
            switch (flipStatus)
            {
                case Flip.FRONTFLIP:
                    {
                        if(currentAngle <= 90 && halfway)
                        {
                            stuntText.text = "Full Front Flip!";
                            GameObject.FindGameObjectWithTag("Database").GetComponent<Database>().player.fullFlips++;
                        } else if (halfway)
                        {
                            stuntText.text = "Half Front Flip!";
                            GameObject.FindGameObjectWithTag("Database").GetComponent<Database>().player.halfFlips++;
                        } else
                        {
                            stuntText.text = "Quarter Front Flip!";
                            GameObject.FindGameObjectWithTag("Database").GetComponent<Database>().player.quarterFlips++;
                        } break;
                    }
                case Flip.BACKFLIP:
                    {
                        if (currentAngle >= 270 && halfway)
                        {
                            stuntText.text = "Full Back Flip!";
                            GameObject.FindGameObjectWithTag("Database").GetComponent<Database>().player.fullFlips++;
                        }
                        else if (halfway)
                        {
                            stuntText.text = "Half Back Flip!";
                            GameObject.FindGameObjectWithTag("Database").GetComponent<Database>().player.halfFlips++;
                        }
                        else
                        {
                            stuntText.text = "Quarter Back Flip!";
                            GameObject.FindGameObjectWithTag("Database").GetComponent<Database>().player.quarterFlips++;
                        }
                        break;
                    }
            }
            
            StartCoroutine(FadeInOutText(Instantiate(stuntText, canvasObject)));
            flipStatus = Flip.NOFLIP;
            halfway = false;
            

        }
    }

    public static IEnumerator FadeInOutText(Text textToFade)
    {
        for (float f = 0f; f < 1f; f += 0.1f) {
            textToFade.transform.position += new Vector3(0, 1, 0);
            Color textColour = textToFade.color;
            textColour.a = f;
            textToFade.color = textColour;
            yield return new WaitForSeconds(.02f);
        }

        for(int i = 0; i < 10; i++)
        {
            textToFade.transform.position += new Vector3(0, 1, 0);
            yield return new WaitForSeconds(.04f);
        }
        
        for (float f = 1f; f > 0f; f -= 0.1f)
        {
            textToFade.transform.position += new Vector3(0, 1, 0);
            Color textColour = textToFade.color;
            textColour.a = f;
            textToFade.color = textColour;
            yield return new WaitForSeconds(.02f);
        }
        Destroy(textToFade.gameObject);
    }
    //all physics based assignment done here
    void FixedUpdate()
    {
        //add ability to rotate the car around its axis

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
            dir = 0;
            torqueDir = 0;
        }
        else if (brakeIsPressed && motorBack.motorSpeed < 0)
        {
            motorBack.motorSpeed = Mathf.Clamp(motorBack.motorSpeed + brakeSpeed * Time.deltaTime, maxFwdSpeed, 0);
            dir = 0;
            torqueDir = 0;
        }
        else
        {
            torqueDir = Input.GetAxis("Horizontal");
            dir = Input.GetAxis("Horizontal");

            if (Input.GetKey(KeyCode.Space) && motorBack.motorSpeed > 0)
            {
                motorBack.motorSpeed = Mathf.Clamp(motorBack.motorSpeed - brakeSpeed * Time.deltaTime, 0, maxBwdSpeed);
                dir = 0;
                torqueDir = 0;
            }
            else if (Input.GetKey(KeyCode.Space) && motorBack.motorSpeed < 0)
            {
                motorBack.motorSpeed = Mathf.Clamp(motorBack.motorSpeed + brakeSpeed * Time.deltaTime, maxFwdSpeed, 0);
                dir = 0;
                torqueDir = 0;
            }
        }



        if (torqueDir != 0)
        {
            rb2d.AddTorque(-8 * Mathf.PI * torqueDir, ForceMode2D.Force);
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
