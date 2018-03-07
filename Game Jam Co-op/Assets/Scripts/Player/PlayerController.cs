using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    //Players numbers
    public int playerNumber = 1;

    //Player mech stats and movement variables.
    public float movementSpeed = 10.0f;
    public float runMultiplier = 4.0f;
    public float rotationSpeed = 5.0f;
    public float gunRotationSpeed = 4.0f;
    public float jumpHeight = 400.0f;

    //Deadzone (how far the controller must be moved for movement to work.)
    private float deadZone = 0.1f;

    //Private floats to protect the public variables and copy input values
    //from controller.
    private float forwardAcceleration;
    private float turnValueX;
    private float turnValueY;
    private float moveValue;
    private float stickInputTurnX;
    private float stickInputTurnY;

    private float moveValueCopy;

    //Copy of the gamepad.
    [HideInInspector]
    public xbox_gamepad gamepad;



    private Rigidbody mechBody;

    //Instances of the mechs body for manipulation.
    public GameObject lowerBody;
    public GameObject upperBody;
    public GameObject railgun;
    public GameObject machinegun;
    public PlayerOverheat playerOverheat;

    public GameObject playerMech;
    private Animator animator;

    AudioSource mechWalking;
    AudioSource mechJumping;
    AudioSource mechRunning;

    //Bools to check if the player i
    [HideInInspector]
    public bool isShooting = false, isRunning = false, isJumping = false, overheated = false, anchored = false;

    //checks if the mech is grounded.
    [HideInInspector]
    public bool grounded = false;

    public float jumpCoolDown = 2f;
    private float accelerationOnExit;

    private float jumpBuffer = 0.1f;

	// Use this for initialization
	void Start () {
     
        mechBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        //Assigns controllers based on player number.
        switch(playerNumber)
        {
            case 1:
               gamepad = GamePadManager.Instance.GetGamePad(1);
                break;
            case 2:
               gamepad = GamePadManager.Instance.GetGamePad(2);
                break;
            default:
                break;
        }

	}
	
	// Update is called once per frame
	void Update () {

        //Since upperbody is broken into two parts upperbody's position = lower bodys position
        // + y value
        upperBody.transform.position = new Vector3(lowerBody.transform.position.x, lowerBody.transform.position.y + 1.5f, lowerBody.transform.position.z);


        //If both triggers are down the mech is anchored.
        if(playerNumber == 1)
        {
            //Anchoring is deprecated
            //if(gamepad.GetTriggerDown_L() && gamepad.GetTriggerDown_R())
            //{
            //    anchored = true;
            //}
            //else if(!gamepad.GetTriggerDown_R() && !gamepad.GetTriggerDown_L())
            //{
            //    anchored = false;
            //}        

            jumpCoolDown -= Time.deltaTime;

            if (grounded && !playerOverheat.overheated)
            {
                moveValue = 0;
                float translation;
                translation = jumpHeight * Time.deltaTime;
                if (gamepad.GetTriggerDown_L() && !gamepad.GetTriggerDown_R() && jumpCoolDown < 0)
                {
                    jumpBuffer -= Time.deltaTime;

                    if (jumpBuffer < 0)
                    {
                        jumpBuffer = 0.1f;
                        isJumping = true;
                        jumpCoolDown = 2f;
                        mechBody.velocity = new Vector3(0, translation, 0);
                        mechBody.AddForce(new Vector3(0, translation * 60, 0), ForceMode.Impulse);
                    }
                }
            }

            if (!grounded)
            {
                isJumping = false;
                jumpCoolDown = 2f;
            }


        //    Debug.Log("jump cd" + jumpCoolDown);

        }



        if (playerNumber == 2)
        {
           
        }
   

    

        //Resets the values so the player stops if controls are let go.
        turnValueX = 0;
        turnValueY = 0;
        forwardAcceleration = 0;
       
        if (!playerOverheat.overheated)
        {
            //Gets stick input values and sets them to a variable.
            stickInputTurnX = gamepad.GetStick_R().X;
            stickInputTurnY = gamepad.GetStick_R().Y;
            forwardAcceleration = gamepad.GetStick_L().Y;


            //Sets the forward acceleration all the way to 1 if the stick
            //is moved even a little bit diagonal to stop inconsistent movement.
            if(forwardAcceleration > 0.1f)
            {
                forwardAcceleration = 1;
            }
            else if(forwardAcceleration < 0.1f && forwardAcceleration > 0)
            {
                forwardAcceleration = 0;
            }

            //sets the input to -1 if the stick is moved.
            if (forwardAcceleration < -0.1f)
            {
                forwardAcceleration = -1;
            }
            else if (forwardAcceleration > -0.1f && forwardAcceleration < 0)
            {
                forwardAcceleration = 0;
            }
            //---------------------------------------------------------------------------

            //if the stick input is greater then the deadzone apply movement.
            if (Mathf.Abs(stickInputTurnX) > deadZone)
            {
                turnValueX = stickInputTurnX;
            }

            if (Mathf.Abs(stickInputTurnY) > deadZone)
            {
                turnValueY = stickInputTurnY;
            }
            if (grounded)
            {
                if (Mathf.Abs(forwardAcceleration) > deadZone)
                {
                    if (gamepad.GetTriggerDown_R() && !playerOverheat.overheated)
                    {
                        moveValue = forwardAcceleration * runMultiplier;
                        isRunning = true;
                    }
                    else if(!gamepad.GetTriggerDown_R())
                    {
                        moveValue = forwardAcceleration;
                        isRunning = false;
                    }

                }
            }
            //----------------------------------------------------------------
        }

        Debug.Log("Overheat: " + playerOverheat.overheatValue);
    }

    private void FixedUpdate()
    {

        moveCharacter(playerNumber);
    }

    static float ClampAngle(float angle, float min, float max)
    {
        if (min < 0 && max > 0 && (angle > max || angle < min))
        {
            angle -= 360;
            if (angle > max || angle < min)
            {
                if (Mathf.Abs(Mathf.DeltaAngle(angle, min)) < Mathf.Abs(Mathf.DeltaAngle(angle, max))) return min;
                else return max;
            }
        }
        else if (min > 0 && (angle > max || angle < min))
        {
            angle += 360;
            if (angle > max || angle < min)
            {
                if (Mathf.Abs(Mathf.DeltaAngle(angle, min)) < Mathf.Abs(Mathf.DeltaAngle(angle, max))) return min;
                else return max;
            }
        }

        if (angle < min) return min;
        else if (angle > max) return max;
        else return angle;
    }

    void moveCharacter(int a_playerNumber)
    {
        switch(playerNumber)
        {
            case 1:
                //if (grounded)
                //{
                    if (forwardAcceleration > 0 && mechBody.velocity.magnitude < 30)
                    {
                    //  lowerBody.transform.Translate(-playerMech.transform.right * moveValue * movementSpeed);
                    mechBody.AddForce(-lowerBody.transform.right * moveValue * (movementSpeed * 0.3f));

                    }
                    else if (forwardAcceleration < 0 && mechBody.velocity.magnitude < 30)
                    {
                    mechBody.AddForce(-lowerBody.transform.right * moveValue * (movementSpeed * 0.3f));
                    //    lowerBody.transform.Translate(-playerMech.transform.right * moveValue * movementSpeed);
                    }

                    if(mechBody.velocity.magnitude > 30)
                {
                    mechBody.velocity = mechBody.velocity.normalized * 30;
                }    



                    if (turnValueX > 0)
                    {
                        lowerBody.transform.Rotate(playerMech.transform.up * turnValueX * rotationSpeed);
                    }
                    else if (turnValueX < 0)
                    {
                        lowerBody.transform.Rotate(playerMech.transform.up * turnValueX * rotationSpeed);
                    }
            //    }
                //else if(!grounded)
                //{
                //    if (moveValue > 0.1f || moveValue < -0.1f)
                //    {
                //        lowerBody.transform.Translate(-playerMech.transform.forward * accelerationOnExit * movementSpeed);
                //    }
              
            
                //       // lowerBody.transform.Translate(-playerMech.transform.forward  * movementSpeed);
                 

                //    if (turnValueX > 0)
                //    {
                //        lowerBody.transform.Rotate(playerMech.transform.up);
                //    }
                //    else if (turnValueX < 0)
                //    {
                //        lowerBody.transform.Rotate(playerMech.transform.up);
                //    }
             //   }
                break;
            case 2:

                float rotationX = upperBody.transform.rotation.x;
                ClampAngle(upperBody.transform.eulerAngles.x, -15f, 30f);

                if (turnValueX > 0)
                {
                    upperBody.transform.Rotate(playerMech.transform.up * turnValueX * rotationSpeed);
                }
                else if(turnValueX < 0)
                {
                    upperBody.transform.Rotate(playerMech.transform.up * turnValueX * rotationSpeed);
                }
                if(turnValueY > 0)
                {
                    railgun.transform.Rotate(0, 0, turnValueY * rotationSpeed);
                    machinegun.transform.Rotate(0, 0, turnValueY * rotationSpeed);
                }
                else if(turnValueY < 0)
                {
                    railgun.transform.Rotate(0,0, turnValueY * rotationSpeed);
                    machinegun.transform.Rotate(0, 0, turnValueY * rotationSpeed);
                }
  


                break;
            default:
                break;


        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Terrain")
        {
            grounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Terrain")
        {
            grounded = false;
            accelerationOnExit = moveValue;
        }
    }
}

