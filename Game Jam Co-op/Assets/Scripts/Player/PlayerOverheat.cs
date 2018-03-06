using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOverheat : MonoBehaviour {

    public PlayerController player1;
    public PlayerController player2;

    [HideInInspector]
    public float overheatValue = 0f;
    private float overheatTimer = 1.5f;
    public float overheatIncreaseRate = 10.0f;
    public float overheatDropRate = 2.0f;
    public float anchoredCooldownRate = 3.0f;
    [HideInInspector]
    public bool overheated = false;

    private float jumpBuffer = 0.1f;
    private float overheatcooloff = 0;

    AudioSource mechOverheated;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if ((player1.isRunning || player2.isShooting) && overheatValue <= 100)
        {
            if(player1.isRunning)
            {

            }
            overheatValue += Time.deltaTime * overheatIncreaseRate;
        }

        if ((player1.gamepad.GetTriggerDown_L()) && !player1.gamepad.GetTriggerDown_R() && overheatValue <= 100)
        {
            if (player1.jumpCoolDown < 0)
            {
                jumpBuffer -= Time.deltaTime;
                if (jumpBuffer < 0)
                {
                    jumpBuffer = 0.1f;
                    overheatValue += 25;
                }
            }
        }

        if (overheatValue >= 100)
        {
            overheated = true;
        }

        if(overheated)
        {
            if (overheatValue > 0)
            {
                overheatcooloff += Time.deltaTime;
                overheatValue -= overheatDropRate * overheatcooloff;
            }
            else if(overheatValue <= 0)
            {
                overheated = false;
                overheatcooloff = 0;

            }
        }

        if (overheatValue >= 0 && !player1.isRunning && !player2.isShooting && !player1.isJumping)
        {
            overheatTimer--;


            if (overheatTimer < 0)
            {
                if (overheatValue > 0)
                {
                    overheatTimer = 1.5f;
                    if (!player1.anchored)
                    {
                        overheatValue -= Time.deltaTime * overheatDropRate;
                    }
                    else if (player1.anchored)
                    {
                        overheatValue -= Time.deltaTime * (overheatDropRate * anchoredCooldownRate);
                    }
                }
            }
        }

    
      //  Debug.Log("Player one : " + player1.jumpCoolDown);
     //   Debug.Log("Overheat: " + overheatValue);
    }


}
