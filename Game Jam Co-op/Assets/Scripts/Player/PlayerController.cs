using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


    public int playerNumber = 1;

    public float movementSpeed = 10.0f;
    public float rotationSpeed = 5.0f;
    
    private float deadZone = 0.1f;
    private float forwardAcceleration;
    private float turnValue;
    private float moveValue;
    private float stickInputTurn;

    private xbox_gamepad gamepad;
    private Rigidbody mechBody;

    public GameObject lowerBody;
    public GameObject upperBody;

    public GameObject playerMech;
    private Animator animator;



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

        upperBody.transform.position = new Vector3(lowerBody.transform.position.x, lowerBody.transform.position.y + 0.8f, lowerBody.transform.position.z);



        stickInputTurn = gamepad.GetStick_R().X;
        forwardAcceleration = gamepad.GetStick_L().Y;

        turnValue = 0;

        if(Mathf.Abs(stickInputTurn) > deadZone)
        {
            turnValue = stickInputTurn;
        }

        if(Mathf.Abs(forwardAcceleration) > deadZone)
        {
            moveValue = forwardAcceleration;
        }

        Debug.Log(turnValue);

	}

    private void FixedUpdate()
    {

        moveCharacter(playerNumber);
    }

    void moveCharacter(int a_playerNumber)
    {
        switch(playerNumber)
        {
            case 1:

                if(forwardAcceleration > 0)
                {
                    lowerBody.transform.Translate(-playerMech.transform.forward * forwardAcceleration * movementSpeed);
                   
                }
                else if(forwardAcceleration < 0)
                {
                    lowerBody.transform.Translate(-playerMech.transform.forward * forwardAcceleration * movementSpeed);
                }

                if(turnValue > 0)
                {
                    lowerBody.transform.Rotate(playerMech.transform.up * turnValue * rotationSpeed);
                }
                else if(turnValue < 0)
                {
                    lowerBody.transform.Rotate(playerMech.transform.up * turnValue * rotationSpeed);
                }

                break;
            case 2:

                if(turnValue > 0)
                {
                    upperBody.transform.Rotate(playerMech.transform.up * turnValue * rotationSpeed);
                }
                else if(turnValue < 0)
                {
                    upperBody.transform.Rotate(playerMech.transform.up * turnValue * rotationSpeed);
                }
                break;
            default:
                break;


        }
    }
}
