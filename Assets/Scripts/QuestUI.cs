using UnityEngine;
using TMPro;

public class QuestUI : MonoBehaviour
{
    public static QuestUI Instance {get; private set;}

    [SerializeField] private TextMeshProUGUI objectiveText;

    private void Awake()
    {
        if (Instance != null && Instance != this) 
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SetObjective(string text)
    {
        if (objectiveText == null)
        {
            Debug.LogError("QuestUI: objectiveText not assigned.");
            return;
        }

        objectiveText.text = text;
    }

    public void ClearObjective()
    {
        if (objectiveText != null)
        {
            objectiveText.text = "";
        }
    }
}