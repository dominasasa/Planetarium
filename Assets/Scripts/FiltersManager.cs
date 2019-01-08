using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FiltersManager : MonoBehaviour
{
    public Toggle Major;
    public Toggle Minor;
    public Toggle Stars;
    public Toggle Moons;
    public Toggle Planets;

    // Start is called before the first frame update
    private void Awake()
    {
        Major = GameObject.Find("Major").GetComponent<Toggle>();
        Minor = GameObject.Find("Minor").GetComponent<Toggle>();
        Stars = GameObject.Find("Stars").GetComponent<Toggle>();
        Moons = GameObject.Find("Moons").GetComponent<Toggle>();
        Planets = GameObject.Find("Planets").GetComponent<Toggle>();

        Major.isOn = DataPersistorScript.Instance.renderMajor;
        Minor.isOn = DataPersistorScript.Instance.renderMinor;
        Stars.isOn = DataPersistorScript.Instance.renderStars;
        Moons.isOn = DataPersistorScript.Instance.renderMoons;
        Planets.isOn = DataPersistorScript.Instance.renderPlanets;

    }

    public void saveFilters()
    {
        DataPersistorScript.Instance.renderMajor = Major.isOn;
        DataPersistorScript.Instance.renderMinor = Minor.isOn;
        DataPersistorScript.Instance.renderMoons = Moons.isOn;
        DataPersistorScript.Instance.renderStars = Stars.isOn;
        DataPersistorScript.Instance.renderPlanets = Planets.isOn;
        Debug.Log("Filters Manager Save");
        Debug.Log("Maj: " + DataPersistorScript.Instance.renderMajor + ", Min: " + DataPersistorScript.Instance.renderMinor + ", Moons: " + DataPersistorScript.Instance.renderMoons + ", Stars: " + DataPersistorScript.Instance.renderStars + ", Planets: " + DataPersistorScript.Instance.renderPlanets);
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }
}
