using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    // speeduup movement so it will actually be visible
    public float orbitSpeed = 1000000;
    public Transform planetTransform;
    
    private Planet planet;

    private float planetAlpha = 0.0f;
    public float scaler = 10.0f; // multiply the size of planets so they will actually be visible comparing to the sun
    public float rotateSpeed = 0f;
    


    // Start is called before the first frame update
    void Start()
    {
        float scaleCoeff = GameObject.Find("SUN").GetComponent<Transform>().localScale.x / 1391016; // sun diameter in km
        planet = planetFromName(gameObject.name);
        float x = planet.diameter * scaleCoeff * scaler - planetTransform.localScale.x;
        float y = planet.diameter * scaleCoeff * scaler - planetTransform.localScale.y;
        float z = planet.diameter * scaleCoeff * scaler - planetTransform.localScale.z;
        planetTransform.localScale += new Vector3(x, y, z);
        planetAlpha = planet.alpha;
        rotateSpeed = planet.angularRotationSpeed * orbitSpeed;


        // orbit time is scalled full circle arount the sun will take ~100s
    }


    // Update is called once per frame
    void Update()
    {
        if (planet == null)
            return;
        planetTransform.position = new Vector3(planet.centerAxis - planet.semiMajorAxis * Mathf.Cos(planetAlpha), 0f, planet.semiMinorAxis * Mathf.Sin(planetAlpha));
        planetAlpha += planet.angularSpeed * orbitSpeed * Time.deltaTime;


        // ----------------------------------------------

        transform.Rotate(new Vector3(0, -rotateSpeed*Time.deltaTime, 0));
    }


    private Planet planetFromName(string name)
    {
        switch (name)
        {
            case "Sun":
                return new Planet(0f, 0f, 0f, 0f, 0f, 1391016.0f, 587.28f);
            case "Mercury":
                return new Planet(57.91f, 87.969f, 0.205630f, 29.1241f, 0.0000101444f, 4879.0f, 1407.6f);
            case "Venus":
                return new Planet(108.21f, 224.701f, 0.006772f, 54.8910f, 0.0000138374f, 12104.0f, -5832.6f);
            case "Earth":
                return new Planet(149.60f, 365.256f, 0.0167086f, 282.9404f, 0.0000470935f, 12756.0f, 23.9345f);
            case "Mars":
                return new Planet(227.92f, 686.971f, 0.0934f, 286.5016f, 0.0000292961f, 6792.0f, 24.6229f);
            case "Jupiter":
                return new Planet(778.57f, 4332.59f, 0.048498f, 273.8777f, 0.0000164505f, 142984.0f, 9.9250f);
            case "Saturn":
                return new Planet(1433.53f, 10759.22f, 0.05555f, 339.3939f, 0.0000297661f, 120536.0f, 10.656f);
            case "Uranus":
                return new Planet(2872.46f, 30688.5f, 0.046381f, 96.6612f, 0.000030565f, 51118.0f, -17.24f);
            case "Neptune":
                return new Planet(4495.06f, 60182f, 0.009456f, 272.8461f, 0.000006027f, 49528.0f, 16.11f);
            case "Pluto":
                return new Planet(5906.38f, 90560f, 0.2488f, 272.8461f, 0.000006027f, 2377.0f, -153.2928f);
            default:
                return null;
        }
    }
    
}

// data from: https://nssdc.gsfc.nasa.gov/planetary/factsheet/



class Planet
{
    public float orbitalPeriod;
    public float orbitalSpeed;
    public float eccentricy;
    public float rotationSpeed;

    public float rotationPeriod;
    public float angularSpeed;
    public float alpha;
    public float diameter;
    public float angularRotationSpeed;
    public float semiMajorAxis;
    public float semiMinorAxis;
    public float centerAxis;

    public Planet(float semiMajorAxis, float orbitalPeriod, float eccentricy, float alpha1, float alpha2, float diameter, float rotationPeriod)
    {
        // semiminorAxis in milion km
        this.orbitalPeriod = orbitalPeriod;
        this.orbitalSpeed = orbitalPeriod * 24.0f * 60.0f * 60.0f;
        this.angularSpeed = (2.0f / orbitalSpeed);
        this.eccentricy = eccentricy;
        this.semiMajorAxis = semiMajorAxis;
        this.semiMinorAxis = semiMajorAxis * Mathf.Sqrt(1 - Mathf.Pow(eccentricy, 2));
        this.centerAxis = semiMinorAxis - semiMajorAxis;
        
        this.alpha = calcCurrentAlpha(alpha1, alpha2);
        this.diameter = diameter;
        this.rotationPeriod = rotationPeriod;
        this.rotationSpeed = rotationPeriod * 60.0f * 60.0f;
        this.angularRotationSpeed = (2.0f / rotationSpeed);
    }


    private float calcCurrentAlpha(float p1, float p2)
    {
        int Y = DateTime.Now.Year;
        int M = DateTime.Now.Month;
        int D = DateTime.Now.Day;
        float d = 367 * Y - (7 * (Y + ((M + 9) / 12))) / 4 + (275 * M) / 9 + D - 730530;
        return p1 + p2 * d;
    }
}
