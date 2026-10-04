using UnityEngine;
using TMPro;

public class ReadableItemUI : MonoBehaviour
{
    public static ReadableItemUI Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private GameObject readingPanel;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text bodyText;


    [SerializeField] private GameObject inventoryPanel;

    private void Awake()
    {
        Instance = this;

        if (readingPanel != null)
            readingPanel.SetActive(false);
    }

    public void Open(ItemData item)
    {
        if (item == null || !item.isReadable)
            return;

        titleText.text = item.readableTitle;
        bodyText.text = item.readableText;

        inventoryPanel.SetActive(false);
        readingPanel.SetActive(true);
    }

    public void Close()
    {
        readingPanel.SetActive(false);  
        inventoryPanel.SetActive(true);
    }
}