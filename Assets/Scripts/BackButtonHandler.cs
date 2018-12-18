using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButtonHandler : MonoBehaviour
{
    LoadSceneOnClick lsoc = new LoadSceneOnClick();
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            lsoc.LoadByIndex(0);
        }
    }
}
