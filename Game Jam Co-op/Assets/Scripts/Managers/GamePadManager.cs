using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Written by Angus Secomb
//Last edited 14/11/17
public class GamePadManager : MonoBehaviour {
    
    //How many gamepads
    public int GamePadCount = 4;

    //List of gamepads
    private List<xbox_gamepad> gamepads;

    //Manager variables.
    private static GamePadManager manager;

    //Public GameObjects.
    [Header("Kart Gamepad Prefabs")]
    private GameObject player1;
    private GameObject player2;
    private GameObject player3;
    private GameObject player4;


    [HideInInspector]
    public bool isPaused = false;

    private Scene currentScene;

	// initialize.
	void Awake () {
         
        //Create manager if one doesn't exist.
        if (manager != null && manager != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            //Create new gamepad manager instance
            manager = this;
            DontDestroyOnLoad(this.gameObject);  

            //Lock gamepadcount based on range
            GamePadCount = Mathf.Clamp(GamePadCount, 1, 4);

            gamepads = new List<xbox_gamepad>();

            //Create gamepads based on gamepad count.
            for(int i =0; i < GamePadCount; ++i)
            {
                gamepads.Add(new xbox_gamepad(i + 1));
            }
           
        }

	}
	
	// Update is called once per frame
	void Update () {

        //Get current scene.
        currentScene = SceneManager.GetActiveScene();
        //If scene is the main scene.
  
   

        //Update gamepads.
		for(int i = 0; i < gamepads.Count; ++i)
        {
            gamepads[i].Update();
        }
        //Activate prefabs based on connected controllers.
      
    }

    //Refresh all gamepads.
    public void Refresh()
    {
        for(int i = 0; i < gamepads.Count; ++i)
        {
            gamepads[i].Refresh();
        }
    }

    public static GamePadManager Instance
    {
        get
        {
            if(manager == null)
            {
                Debug.LogError("[GamePadManger]: Instance does not exist.");
                return null;
            }
            //Returns the instance of the gamepad manager if one exists.
            return manager;
        }
    }

    public xbox_gamepad GetGamePad(int index)
    {
        //Search gamepads for indexed gamepad.
        for(int i = 0; i < gamepads.Count;)
        {
            //indexes match, return this gamepad
            if(gamepads[i].Index == (index - 1))
            {
                return gamepads[i];
            }
            else
            {
                ++i;
            }
        }

        Debug.LogError("[GamepadManager]: " + index + "is not a valid gamepad.");

        return null;
    }

    public int ConnectedTotal()
    {
        int total = 0;

        //Adds 1 to the total for each gamepad that is connected.
        for(int i = 0; i < gamepads.Count; ++i)
        {
            if(gamepads[i].IsConnected)
            {
                total++;
            }
        }

        return total;
    }

    public bool GetButtonAny(string button)
    {
        for(int i = 0; i < gamepads.Count; ++i)
        {
            //gamepad meets both conditions
            if(gamepads[i].IsConnected && gamepads[i].GetButton(button))
            {
                return true;
            }
        }

        return false;
    }

    public bool GetButtonDownAny(string button)
    {
        for(int i = 0; i < gamepads.Count; ++i)
        {
            //gamepad meets both conditions
            if(gamepads[i].IsConnected && gamepads[i].GetButtonDown(button))
            {
                return true;
            }
        }

        return false;

    }
}
