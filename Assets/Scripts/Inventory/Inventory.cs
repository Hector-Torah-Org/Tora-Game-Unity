using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Transform gridParent;
    [SerializeField] private Transform inventoryPanel;
    [SerializeField] private ItemView itemViewPrefab;

    private readonly List<ItemStack> items = new List<ItemStack>();
    public IReadOnlyList<ItemStack> Items => items;
    private bool isOpen = false;
                                                            
    public void AddItem(ItemStack stack)
    {
        if (stack == null || stack.item == null) return;
        items.Add(stack);
    }                                                             

    public void RemoveItem(ItemStack stack)
    {
        if (stack == null) return;
        items.Remove(stack);
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != null) { 
            return;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (isOpen) Close();
            else Open();
        }
    }

    public void Open()
    {
        if(SceneManager.Instance != null && SceneManager.Instance.uiLevelsBlockingWorldInput.Count > 0 && SceneManager.Instance.uiLevelsBlockingWorldInput[^1] > SceneManager.UIBlockLevel.Inventory)
        {
            return;
        }

        isOpen = true;
        if (inventoryPanel != null) inventoryPanel.gameObject.SetActive(true);
        RefreshUI();

        if (SceneManager.Instance != null)
            SceneManager.Instance.AddUIBlockingWorldInput(SceneManager.UIBlockLevel.Inventory);
    }

    public void Close()
    {
        if (SceneManager.Instance != null && SceneManager.Instance.uiLevelsBlockingWorldInput.Count > 0 && SceneManager.Instance.uiLevelsBlockingWorldInput[^1] > SceneManager.UIBlockLevel.Inventory)
        {
            return;
        }

        isOpen = false;
        if (inventoryPanel != null) inventoryPanel.gameObject.SetActive(false);
        ClearUI();

        if (SceneManager.Instance != null)
            SceneManager.Instance.RemoveUIBlockingWorldInput(SceneManager.UIBlockLevel.Inventory);
    }

    public void RefreshUI()
    {
        ClearUI();

        if (gridParent == null || itemViewPrefab == null)
        {
            Debug.LogError("Inventory: gridParent or itemViewPrefab not assigned.");
            return;
        }

        foreach (var stack in items)
        {
            ItemView view = Instantiate(itemViewPrefab, gridParent);
            view.Init(stack, ItemLocation.Inventory, this, null);
        }
    }

    private void ClearUI()
    {
        if (gridParent == null) return;

        for (int i = gridParent.childCount - 1; i >= 0; i--)
            Destroy(gridParent.GetChild(i).gameObject);
    }
}