using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    private string sceneHint = "";

    private readonly List<string> activeQuests = new List<string>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        UpdateQuestText();
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

        if (!activeQuests.Contains(questText))
        {
            activeQuests.Add(questText);
        }

        UpdateQuestText();
    }

    public void CompleteQuest(string questText)
    {
        if (activeQuests.Contains(questText))
        {
            activeQuests.Remove(questText);
        }

        UpdateQuestText();
    }

    private void UpdateQuestText()
    {
        string output = "";

        if (!string.IsNullOrEmpty(sceneHint))
        {
            output += sceneHint + "\n\n";
        }

        foreach (string quest in activeQuests)
        {
            output += "- " + quest + "\n";
        }

        QuestUI.Instance?.SetObjective(output);
    }
}