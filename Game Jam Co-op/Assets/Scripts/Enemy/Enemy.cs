//--------------------------------------------------------------------------------------
// Purpose: Enemy class.
//
// Description: The main class for the indivual enemy units.
//
// Author: Thomas Wiltshire.
//--------------------------------------------------------------------------------------

// using, etc
using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using System.Collections.Generic;

//--------------------------------------------------------------------------------------
// Enemy object. Inheriting from MonoBehaviour. The main enemy class for enemy behaviour
//--------------------------------------------------------------------------------------
public class Enemy : MonoBehaviour
{
    // PUBLIC VALUES //
    //--------------------------------------------------------------------------------------
    // pulbic int for the max health of the enemy
    public int m_nMaxHealth;

    // public int for the damage that the enemy can do
    public int m_nDamage;

    // public foat for the speed of the enemy.
    public float m_fSpeed;

    // public hover height for enemy
    public float m_fHoverHeight;

    // public int for the score of what a enemy is worth.
    public int m_nScoreValue;
    //--------------------------------------------------------------------------------------

    // PRIVATE VALUES //
    //--------------------------------------------------------------------------------------
    // private NavMeshAgent for the navmesh agent of the enemy
    private NavMeshAgent m_nmAgent;

    // private transform for the goal postion of the enemy.
    private Transform m_tGoal;
    
    // private int for the current health of the enemy
    [HideInInspector]                                           // Edited By: Angus
    public int m_nHealth;

    // private gameobject for the player object
    private GameObject m_gPlayer;

    // private PlayerStats for the stat class of player.
    private PlayerStats m_pPlayerStats;

    // the enemy animator
    private Animator m_aAnimator;

    // boolean for an animation of the enemy death
    public bool m_bDeathAni;
    //--------------------------------------------------------------------------------------

    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    void Awake()
    {
        // Get navmesh agent commponent
        m_nmAgent = GetComponent<NavMeshAgent>();

        // set the speed of the navmesh agent
        m_nmAgent.speed = m_fSpeed;

        // set the offset of the navmesh
        m_nmAgent.baseOffset = m_fHoverHeight;

        // get player object
        m_gPlayer = GameObject.FindGameObjectWithTag("Mechboy");
        
        // get the player stats component
        m_pPlayerStats = m_gPlayer.GetComponent<PlayerStats>();
        
        // if valid player
        if (m_gPlayer)
        {
            // set goal to player transform
            m_tGoal = m_gPlayer.transform;
        }

        // Get the component of the animator
        m_aAnimator = GetComponent<Animator>();

        // set the deafult for the death animation
        m_bDeathAni = false;
    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    void Update()
    {
        // if valid goal
        if (m_tGoal)
        {
            // set navmesh agent destination to goal position.
            m_nmAgent.destination = m_tGoal.position;
        }
        
        // Debug enemy health value
        Debug.Log(m_nHealth);

        // Check if the enemy is dead
        CheckDeath();
    }

    //--------------------------------------------------------------------------------------
    // SetHealth: Set the health of the enemy
    //
    // Param:
    //      nHealth: int value for setting health
    //--------------------------------------------------------------------------------------
    public void SetHealth(int nHealth)
    {
        // set health
        m_nHealth += nHealth;
    }

    //--------------------------------------------------------------------------------------
    // SetMaxHealth: Set the max health of the enemy
    //
    // Param:
    //      nHealth: int value for setting health
    //--------------------------------------------------------------------------------------
    public void SetMaxHealth(int nHealth)
    {
        // set health
        m_nMaxHealth = nHealth;
    }

    //--------------------------------------------------------------------------------------
    // SetDamage: Set the damage of the enemy
    //
    // Param:
    //      nDamage: int value for setting damage
    //--------------------------------------------------------------------------------------
    public void SetDamage(int nDamage)
    {
        // Set damage
        m_nDamage = nDamage;
    }

    //--------------------------------------------------------------------------------------
    // SetSpeed: Set the speed of the enemy
    //
    // Param:
    //      fSpeed: int value for setting speed
    //--------------------------------------------------------------------------------------
    public void SetSpeed(int fSpeed)
    {
        // Set damage
        m_fSpeed = fSpeed;

        // set speed to the navmesh agent
        m_nmAgent.speed = m_fSpeed;
    }

    //--------------------------------------------------------------------------------------
    // CheckDeath: Check if the enemy is dead.
    //--------------------------------------------------------------------------------------
    public bool CheckDeath()
    {
        // if no health
        if (m_nHealth <= 0)
        {
            // play death animation
            m_bDeathAni = true;

            // set inactive when dead
            gameObject.SetActive(false);
            
            // set health back to full
            m_nHealth = m_nMaxHealth;

            // reset death animation for next death
            m_bDeathAni = false;

            // return true
            return true;
        }

        // else no death then return false
        return false;
    }

    //--------------------------------------------------------------------------------------
    // OnCollisionEnter: Calls when this object has hit a collision.
    //
    // Param:
    //      collision: The object that has been collided with.
    //
    // Edited By:
    //      Angus
    //--------------------------------------------------------------------------------------
    private void OnCollisionEnter(Collision collision)
    {
        // if the collison is with the player.
        if (collision.gameObject.tag == "Mechboy")
        {
            // set health of the enemy to 0
            m_nHealth = 0;

            m_pPlayerStats.health = -m_nDamage;

        }

        // if the collison is with the player.
        if (collision.gameObject.tag == "Bullet")
        {
            Debug.Log("Hit enemy!");
            // Get bullet object
            //   GameObject gBullet = GameObject.FindGameObjectWithTag("Bullet");

            //Grab bullet information from object.
            BulletScript bullet = collision.gameObject.GetComponent<BulletScript>();

            //take bullet damage away from health.
            m_nHealth =- bullet.bulletDamage;

            if(m_nHealth < 0)
            {
                //Add 10 points to score.
                m_pPlayerStats.score += 10;
            }
        }
    }
}