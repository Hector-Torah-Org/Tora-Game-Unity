using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    private string sceneHint = "";

    private readonly List<string> activeQuests = new List<string>();
    private readonly List<string> completedQuests = new List<string>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        I18n.LanguageChanged += UpdateQuestText;
    }

    private void Start()
    {
        UpdateQuestText();
    }

    private void OnDestroy()
    {
        I18n.LanguageChanged -= UpdateQuestText;
    }

    public void SetSceneHint(string text)
    {
        sceneHint = text;
        UpdateQuestText();
    }

    public void ClearSceneHint()
    {
        sceneHint = "";
        UpdateQuestText();
    }

    public void AddQuest(string questText)
    {
        if (string.IsNullOrEmpty(questText))
            return;

        if (completedQuests.Contains(questText))
            return;

        if (!activeQuests.Contains(questText))
            activeQuests.Add(questText);

        UpdateQuestText();
    }

    public void CompleteQuest(string questText)
    {
        if (string.IsNullOrEmpty(questText))
            return;

        if (activeQuests.Contains(questText))
            activeQuests.Remove(questText);

        if (!completedQuests.Contains(questText))
            completedQuests.Add(questText);

        UpdateQuestText();
    }

    private void UpdateQuestText()
    {
        string output = "";

        if (!string.IsNullOrEmpty(sceneHint))
        {
            output += I18n.Translate(sceneHint) + "\n\n";
        }

        foreach (string quest in activeQuests)
        {
            output += "- " + I18n.Translate(quest) + "\n";
        }

        QuestUI.Instance?.SetObjective(output);
    }
}