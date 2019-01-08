using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }
    public int menuId = 0;
    public GameObject[] menuPanels;
    public GameObject WelcomePanel;
    public GameObject PointerPanel;
    public GameObject Credits;
    public GameObject PlanetariumScene;
    public GameObject ModelsScene;
    public GameObject SkyObjectInfo;
    public GameObject FiltersMenu;
    
    private void Awake()
    {
        Destroy(Instance);
        Instance = this;

        menuPanels = GameObject.FindGameObjectsWithTag("MainMenu");

        if (WelcomePanel == null) WelcomePanel = GameObject.Find("WelcomePanel");
        if (PointerPanel == null) PointerPanel = GameObject.Find("PointerPanel");
        if (Credits == null) Credits = GameObject.Find("Credits");
        if (PlanetariumScene == null) PlanetariumScene = GameObject.Find("PlanetariumScene");
        if (ModelsScene == null) ModelsScene = GameObject.Find("ModelsScene");
        if (SkyObjectInfo == null) SkyObjectInfo = GameObject.Find("SkyObjectInfo");
        if (FiltersMenu == null) FiltersMenu = GameObject.Find("FiltersMenu");
        
        switchToMenu(menuId);
    }

    // Use this for initialization
    private void Start()
    {

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
