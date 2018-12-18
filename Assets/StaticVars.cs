using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticVars : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine("InitializeLocation");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator InitializeLocation()
    {
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("location disabled by user");
            yield break;
        }

        // enable compass
        Input.compass.enabled = true;
        // start the location service
        Debug.Log("start location service");
        Input.location.Start(10, 0.01f);
        // Wait until service initializes
        int maxSecondsToWaitForLocation = 60;
        while (Input.location.status == LocationServiceStatus.Initializing && maxSecondsToWaitForLocation > 0)
        {
            yield return new WaitForSeconds(1);
            maxSecondsToWaitForLocation--;
        }

        // Service didn't initialize in 20 seconds
        if (maxSecondsToWaitForLocation < 1)
        {
            Debug.Log("location service timeout");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("unable to determine device location");
            yield break;
        }

        Debug.Log("location service loaded");
        yield break;
    }
}
