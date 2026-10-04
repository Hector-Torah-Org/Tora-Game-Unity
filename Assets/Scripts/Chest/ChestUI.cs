using System.Collections.Generic;
using UnityEngine;

public class ChestUI : MonoBehaviour
{
    public static ChestUI Instance { get; private set; }

    [SerializeField] private Transform chestPanel;
    [SerializeField] private Transform gridParent;
    [SerializeField] private ItemView itemViewPrefab;
    [SerializeField] private Inventory playerInventory;

    private ChestScript currentChest;


    public bool IsOpen
    {
        get
        {
            return chestPanel != null && chestPanel.gameObject.activeSelf;
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }


    public void Open(ChestScript chest, List<ItemStack> contents)
    {
        currentChest = chest;

        if (chestPanel != null) { chestPanel.gameObject.SetActive(true); }

        SceneManager.Instance?.AddUIBlockingWorldInput(SceneManager.UIBlockLevel.ChestInventory);

        Render(contents);
    }

    public void Close()
    {
        currentChest = null;
        ClearUI();
        if (chestPanel != null) { chestPanel.gameObject.SetActive(false); }

        SceneManager.Instance?.RemoveUIBlockingWorldInput(SceneManager.UIBlockLevel.ChestInventory);
    }

    private void Render(List<ItemStack> contents)
    {
        ClearUI();

        if (gridParent == null || itemViewPrefab == null || playerInventory == null || currentChest == null)
        {
            Debug.LogError("ChestUI not configured");
            return;
        }

        foreach (var stack in contents)
        {
            ItemView view = Instantiate(itemViewPrefab, gridParent);
            view.Init(stack, ItemLocation.Chest, playerInventory, currentChest);
        }
    }

    private void ClearUI()
    {
        if (gridParent == null) return;
        for (int i = gridParent.childCount - 1; i >= 0; i--)
            Destroy(gridParent.GetChild(i).gameObject);
    }
}