using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    public float health = 100.0f;

    [HideInInspector]
    public float score = 0f;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        onDeath();
        Debug.Log("Score: " + score);
	}

    void onDeath()
    {
        if(health <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
