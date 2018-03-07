//--------------------------------------------------------------------------------------
// Purpose: Draw the UI for the player stats.
//
// Description: Grab the player stats from the player and display them in canvas text UI
// objects.
//
// Author: Thomas Wiltshire.
//--------------------------------------------------------------------------------------

// Using, etc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//--------------------------------------------------------------------------------------
// UI object. Inheriting from MonoBehaviour. Used for diaplying the main UI of the game.
//--------------------------------------------------------------------------------------
public class UI : MonoBehaviour
{
    // PUBLIC VALUES //
    //--------------------------------------------------------------------------------------
    // public text object value.
    public Text m_tTextScore;

    // public slider object value.
    public Text m_tTextHealth;

    // public slider object value.
    public Text m_tTextHeat;
    //--------------------------------------------------------------------------------------

    // PRIVATE VALUES //
    //--------------------------------------------------------------------------------------
    // private score int
    private float m_fScore;

    // private health int
    private float m_fHealth;

    // private heat int
    private float m_fHeatLevel;

    // prviate gameobject for the player object
    private GameObject m_gPlayer;

    // prviate gameobject for the player stats object
    private PlayerStats m_pPlayerStats;
    //--------------------------------------------------------------------------------------

    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    void Awake()
    {
        // Get player by tag
        m_gPlayer = GameObject.FindGameObjectWithTag("Mechboy");
        m_pPlayerStats = m_gPlayer.GetComponent<PlayerStats>();

        // Set stat values for displaying.
        m_fScore = m_pPlayerStats.score;
        m_fHealth = m_pPlayerStats.health;
        m_fHeatLevel = m_gPlayer.GetComponent<PlayerOverheat>().overheatValue;
    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    void Update()
    {
        // divide the health into proportions of a hundread
        float fHealthProportion = m_fHealth / 100;

        // Lerp the color of the text between red and green
        Color cLerpColor = Color.Lerp(Color.red, Color.green, fHealthProportion);

        // set the score text and color
        m_tTextScore.text = string.Format("{0}", m_fScore);
        m_tTextScore.color = cLerpColor;

        // set the health text and color
        m_tTextHealth.text = string.Format("{0}", m_fHealth);
        m_tTextHealth.color = cLerpColor;

        // set the heat text and color
        m_tTextHeat.text = string.Format("{0}", m_fHeatLevel);
        m_tTextHeat.color = cLerpColor;
    }
}
