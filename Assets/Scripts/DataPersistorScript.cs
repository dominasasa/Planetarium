using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPersistorScript : MonoBehaviour
{
    public static DataPersistorScript Instance { get; private set; }

    public bool renderMajor;
    public bool renderMinor;
    public bool renderMoons;
    public bool renderStars;
    public bool renderPlanets;

    private void Awake()
    {
        if (Instance == null)
        {
            Debug.Log("\n\n\n\nNEW INSTANCE OF DP");
            DontDestroyOnLoad(gameObject);
            Instance = this;

            Debug.Log("Maj: " + renderMajor + ", Min: " + renderMinor + ", Moons: " + renderMoons + ", Stars: " + renderStars + ", Planets: " + renderPlanets);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            Debug.Log("\n\n\n\n REUSEDDDD INSTANCE OF DP");
            Debug.Log("Maj: " + renderMajor + ", Min: " + renderMinor + ", Moons: " + renderMoons + ", Stars: " + renderStars + ", Planets: " + renderPlanets);
        }


    }
}
