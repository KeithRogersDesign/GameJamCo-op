//--------------------------------------------------------------------------------------
// Purpose: Spawn Enemies.
//
// Description: This class is used for spawning and holding the enemy agents pool.
// Also used for any settings or changes that need to be made to Enemies in the scene.
//
// Author: Thomas Wiltshire.
//--------------------------------------------------------------------------------------

// Using, etc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------------------------------------------------------
// SpawnPoint object. Inheriting from MonoBehaviour.Used for spawning the enemies.
//--------------------------------------------------------------------------------------
public class SpawnController : MonoBehaviour
{
    // PUBLIC VALUES //
    //--------------------------------------------------------------------------------------
    // public gameobject for the enemy spawn blueprint
    public GameObject m_gEnemyBlueprint;

    // public int for array size
    public int m_nPoolSize;

    // public float for spawn rate
    public float m_fSpawnRate;

    // public float for the spawn circle radius
    public float m_fSpawnRadius;

    // public float for the amount of time between difficulty increases.
    public float m_fDifficultyTime;

    // public int for the ammount to increase the bool by each difficulty tick
    public int m_nPoolIncreaseAmmount;
    //--------------------------------------------------------------------------------------

    // PRVIATE VALUES //
    //--------------------------------------------------------------------------------------
    // prviate dynamic array for enemy list
    private List<GameObject> m_agEnemyList;

    // prviate float for the spawn timer
    private float m_fSpawnTimer;

    // private float value for the timer of the difficulty increase.
    private float m_fDifficultyTimer;
    //--------------------------------------------------------------------------------------

    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    void Awake()
    {
        // initialize enemylist with size
        m_agEnemyList = new List<GameObject>();

        // loop through each enemy
        for (int i = 0; i < m_nPoolSize; ++i)
        {
            // Instantiate and set active state.
            GameObject tmp = Instantiate(m_gEnemyBlueprint);
            tmp.SetActive(false);
            m_agEnemyList.Add(tmp);
        }

        // Set default value for the timers
        m_fSpawnTimer = 0.0f;
        m_fDifficultyTimer = m_fDifficultyTime; ;
    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    void Update()
    {
        // Start the timer // Update by deltatime
        m_fSpawnTimer += Time.deltaTime;

        // if spawn timer is greater than the spawn rate
        if (m_fSpawnTimer > m_fSpawnRate)
        {
            // reset timer
            m_fSpawnTimer = 0.0f;

            // Allocate an enemy to the pool
            GameObject gEnemy = AllocateEnemy();

            // If a valid enemy
            if (gEnemy)
            {
                // put enemy at a random position in the map
                Vector2 randPos = Random.insideUnitCircle * m_fSpawnRadius;
                Debug.Log(randPos.ToString());
                gEnemy.transform.position = new Vector3(randPos.x, 0, randPos.y);
            }
        }

        // Increase the difficulty.
        IncreaseDifficulty();
    }

    //--------------------------------------------------------------------------------------
    // AllocateEnemy: Allocate enemy to the object pool.
    //
    // Return:
    //      GameObject: Return the allocated gameobject.
    //--------------------------------------------------------------------------------------
    GameObject AllocateEnemy()
    {
        // for each in the pool
        for (int i = 0; i < m_nPoolSize; ++i)
        {
            // Check if active
            if (!m_agEnemyList[i].activeInHierarchy)
            {
                // Set active state
                m_agEnemyList[i].SetActive(true);

                // Return the enemy
                return m_agEnemyList[i];
            }
        }

        // If all fail return null;
        return null;
    }

    //--------------------------------------------------------------------------------------
    // AddNewEnemy: Add a new enemy to the object pool.
    //--------------------------------------------------------------------------------------
    void AddNewEnemy()
    {
        GameObject tmp = Instantiate(m_gEnemyBlueprint);
        tmp.SetActive(false);
        m_agEnemyList.Add(tmp);
    }

    //--------------------------------------------------------------------------------------
    // SetPoolSize: Set the pool size of the SpawnPoint.
    //
    // Param:
    //      nSize: int for setting pool size
    //--------------------------------------------------------------------------------------
    void SetPoolSize(int nSize)
    {
        // Set pool size
        m_nPoolSize = nSize;

        // Add new enemy to the list
        while (m_nPoolSize > m_agEnemyList.Count)
            AddNewEnemy();
    }

    //--------------------------------------------------------------------------------------
    // SetHealth: Set the health of the enemies in the object pool.
    //
    // Param:
    //      nHealth: int for setting enemy health
    //--------------------------------------------------------------------------------------
    void SetHealth(int nHealth)
    {
        //; Loop through each enemy
        for (int i = 0; i < m_nPoolSize; ++i)
        {
            // set health
            m_agEnemyList[i].GetComponent<Enemy>().SetHealth(nHealth);
        }
    }

    //--------------------------------------------------------------------------------------
    // SetDamage: Set the damage of the enemies in the object pool.
    //
    // Param:
    //      nDamage: int for setting enemy damage
    //--------------------------------------------------------------------------------------
    void SetDamage(int nDamage)
    {
        // Loop through each enemy
        for (int i = 0; i < m_nPoolSize; ++i)
        {
            // set damage
            m_agEnemyList[i].GetComponent<Enemy>().SetHealth(nDamage);
        }
    }

    //--------------------------------------------------------------------------------------
    // IncreaseDifficulty: Increase the difficulty over time by increasing spawn values.
    //--------------------------------------------------------------------------------------
    void IncreaseDifficulty()
    {
        // start the timer for the difficulty increase
        m_fDifficultyTimer -= Time.deltaTime;

        // if the timer is less than 0
        if (m_fDifficultyTimer < 0)
        {
            // new int value to hold what the newly increased pool size will be
            int nSetPool = m_nPoolSize += m_nPoolIncreaseAmmount;

            // Set the new pool size
            SetPoolSize(nSetPool);

            // reset the timer
            m_fDifficultyTimer = m_fDifficultyTime;
        }
    }
}