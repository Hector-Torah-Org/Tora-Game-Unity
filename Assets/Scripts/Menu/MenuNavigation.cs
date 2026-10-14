using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuNavigation : MonoBehaviour
{
    [SerializeField] public GameObject MainMenu;
    [SerializeField] public GameObject StatisticsMenu;
    [SerializeField] public GameObject LeaderboardMenu;
    [SerializeField] public GameObject TutorialMenu;
    [SerializeField] public ApiConnection apiConnection;
    [SerializeField] public SceneManager sceneManager;
    [SerializeField] public GameStateManager gameStateManager;



    private List<GameObject> history = new List<GameObject>();

    private void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        CreateWebGLControls();
#endif
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
            ToggleMenu();
    }

    private void ToggleMenu()
    {
        if (sceneManager.uiLevelsBlockingWorldInput.Count == 0 || sceneManager.uiLevelsBlockingWorldInput[^1] <= SceneManager.UIBlockLevel.Menu)
        {
            if (ChestUI.Instance != null && ChestUI.Instance.IsOpen)
            {
                ChestUI.Instance.Close();
                return;
            }

            GoBack();
        }
    }

    private void ToggleInventory()
    {
        Inventory inventory = FindFirstObjectByType<Inventory>();
        if (inventory == null)
        {
            Debug.LogError("Cannot toggle inventory: no Inventory component was found.");
            return;
        }

        inventory.Toggle();
    }

    private void CreateWebGLControls()
    {
        Canvas canvas = GetComponent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Cannot create WebGL controls: MenuNavigation must be attached to a Canvas.");
            return;
        }

        CreateWebGLButton(canvas.transform, "M", new Vector2(-100f, -16f), ToggleMenu);
        CreateWebGLButton(canvas.transform, "I", new Vector2(-16f, -16f), ToggleInventory);
    }

    private void CreateWebGLButton(Transform parent, string label, Vector2 anchoredPosition, UnityEngine.Events.UnityAction onClick)
    {
        GameObject buttonObject = new GameObject($"{label} Mobile Control", typeof(RectTransform), typeof(Image), typeof(Button));
        RectTransform buttonTransform = buttonObject.GetComponent<RectTransform>();
        buttonTransform.SetParent(parent, false);
        buttonTransform.anchorMin = new Vector2(1f, 1f);
        buttonTransform.anchorMax = new Vector2(1f, 1f);
        buttonTransform.pivot = new Vector2(1f, 1f);
        buttonTransform.anchoredPosition = anchoredPosition;
        buttonTransform.sizeDelta = new Vector2(72f, 72f);

        Image background = buttonObject.GetComponent<Image>();
        background.color = new Color(0.12f, 0.12f, 0.12f, 0.78f);

        Button button = buttonObject.GetComponent<Button>();
        button.targetGraphic = background;
        button.transition = Selectable.Transition.None;
        button.navigation = new Navigation { mode = Navigation.Mode.None };
        button.onClick.AddListener(() =>
        {
            EventSystem.current?.SetSelectedGameObject(null);
            onClick();
        });

        GameObject textObject = new GameObject("Label", typeof(RectTransform), typeof(Text));
        RectTransform textTransform = textObject.GetComponent<RectTransform>();
        textTransform.SetParent(buttonTransform, false);
        textTransform.anchorMin = Vector2.zero;
        textTransform.anchorMax = Vector2.one;
        textTransform.offsetMin = Vector2.zero;
        textTransform.offsetMax = Vector2.zero;

        Text text = textObject.GetComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.text = label;
        text.fontSize = 28;
        text.fontStyle = FontStyle.Bold;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;
        text.raycastTarget = false;
    }

    public void GoBack()
    {
        if (history.Count() > 0)
        {
            history.Last().SetActive(false);
            if (history.Count() > 1) history[history.Count() - 2].SetActive(true); else sceneManager.RemoveUIBlockingWorldInput(SceneManager.UIBlockLevel.Menu);
            history.RemoveAt(history.Count() - 1);
        } else
        {
            MainMenu.SetActive(true);
            history.Add(MainMenu);
            sceneManager.AddUIBlockingWorldInput(SceneManager.UIBlockLevel.Menu);
        }
    }

    public void ActivateSubmenu(GameObject submenu)
    {
        history.Last().SetActive(false);
        history.Add(submenu);
        sceneManager.AddUIBlockingWorldInput(SceneManager.UIBlockLevel.Menu);
        history.Last().SetActive(true);
    }

    //================Individual button functions================//

    public void ButtonStatistics()
    {
        ActivateSubmenu(StatisticsMenu);
        Debug.Log("Starting to draw graph");
        StartCoroutine(StatisticsMenu.GetComponent<StatisticsPanel>().DrawGraph());
    }

    public void ButtonLeaderboard()
    {
        ActivateSubmenu(LeaderboardMenu);
        StartCoroutine(LeaderboardMenu.GetComponent<LeaderboardPanel>().ReloadLeaderboard());
    }

    public void ButtonTutorial()
    {
        ActivateSubmenu(TutorialMenu);
    }

    public void ButtonSaveGame()
    {
        gameStateManager.SaveToServer();
    }
}