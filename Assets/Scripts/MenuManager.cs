using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    public int menuId = 0;
    public GameObject[] menuPanels;
    public GameObject WelcomePanel;
    public GameObject PointerPanel;
    public GameObject Credits;
    public GameObject PlanetariumScene;
    public GameObject ModelsScene;
    public GameObject SkyObjectInfo;
    public GameObject FiltersMenu;

    Dictionary<string, int> menus = new Dictionary<string, int>()
    {
        { "welcome", 0 },
        { "pointer", 1 },
        { "credits", 2},
        { "planetarium", 3},
        { "models", 4},
        { "skyObj", 5},
        { "filters", 6 }
    };

    private void Awake()
    {
        menuPanels = GameObject.FindGameObjectsWithTag("MainMenu");

        WelcomePanel = GameObject.Find("WelcomePanel");
        PointerPanel = GameObject.Find("PointerPanel");
        Credits = GameObject.Find("Credits");
        PlanetariumScene = GameObject.Find("PlanetariumScene");
        ModelsScene = GameObject.Find("ModelsScene");
        SkyObjectInfo = GameObject.Find("SkyObjectInfo");
        FiltersMenu = GameObject.Find("FiltersMenu");
    }

    // Use this for initialization
    void Start()
    {
        switchToMenu(menuId);

        //StartCoroutine(waitToFinishLoad(1));
    }

    IEnumerator waitToFinishLoad(int secs)
    {
        yield return new WaitForSeconds(secs);
        switchToMenu(menuId);
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void switchToMenu(string id)
    {
        switchToMenu(menus[id]);
    }

    public void switchToMenu(int id)
    {
        foreach (var panel in menuPanels)
        {
            panel.gameObject.SetActive(false);
            Debug.Log(panel.name);
        }

        switch (id)
        {
            case 0:
                WelcomePanel.gameObject.SetActive(true);
                break;

            case 1:
                PointerPanel.gameObject.SetActive(true);
                break;
            case 2:
                Credits.gameObject.SetActive(true);
                break;
            case 3:
                PlanetariumScene.gameObject.SetActive(true);
                break;
            case 4:
                ModelsScene.gameObject.SetActive(true);
                break;
            case 5:
                SkyObjectInfo.gameObject.SetActive(true);
                break;
            case 6:
                FiltersMenu.gameObject.SetActive(true);
                break;
        }
    }
}
