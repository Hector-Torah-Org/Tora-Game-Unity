using UnityEngine;

public class InventoryTester : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private ItemData coinItem;

    private void Start()
    {
        Debug.Log("InventoryTester Start()");

        if (inventory == null) Debug.LogError("Inventory reference missing");
        if (coinItem == null) Debug.LogError("coinItem reference missing");

        inventory.AddItem(new ItemStack { item = coinItem, amount = 1 });
        inventory.AddItem(new ItemStack { item = coinItem, amount = 5 });
    }
}