using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;

public class MenuNavigation : MonoBehaviour
{
    [SerializeField] public GameObject MainMenu;
    [SerializeField] public GameObject StatisticsMenu;
    [SerializeField] public GameObject LeaderboardMenu;
    [SerializeField] public GameObject TutorialMenu;
    [SerializeField] public ApiConnection apiConnection;


    private List<GameObject> history = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != null) { 
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GoBack();
        }
    }

    public void GoBack()
    {
        if (history.Count() > 0)
        {
            history.Last().SetActive(false);
            if (history.Count() > 1) history[history.Count() - 2].SetActive(true);
            history.RemoveAt(history.Count() - 1);
        } else
        {
            MainMenu.SetActive(true);
            history.Add(MainMenu);
        }
    }

    public void ActivateSubmenu(GameObject submenu)
    {
        history.Last().SetActive(false);
        history.Add(submenu);
        history.Last().SetActive(true);
    }

    //================Individual button functions================//

    public void ButtonStatistics()
    {
        ActivateSubmenu(StatisticsMenu);
    }

    public void ButtonLeaderboard()
    {
        ActivateSubmenu(LeaderboardMenu);
    }

    public void ButtonTutorial()
    {
        ActivateSubmenu(TutorialMenu);
    }

    public void ButtonSaveGame()
    {
        
    }
}