using UnityEngine;
using UnityEngine.UI;

public class ItemView : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Button button;

    private ItemStack stack;
    private ItemLocation location;

    private Inventory inventoryOwner;
    private ChestScript chestOwner;

    public void Init(ItemStack stack, ItemLocation location, Inventory inventoryOwner, ChestScript chestOwner)
    {
        this.stack = stack;
        this.location = location;
        this.inventoryOwner = inventoryOwner;
        this.chestOwner = chestOwner;

        if (iconImage != null && stack != null && stack.item != null)
            iconImage.sprite = stack.item.icon;

        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClicked);
        }
    }

    private void OnClicked()
    {
        if (stack == null || stack.item == null) return;

        if (location == ItemLocation.Chest)
        {
            if (chestOwner != null && inventoryOwner != null)
            {
                chestOwner.RemoveItem(stack);
                inventoryOwner.AddItem(stack);                                              //Chest zu inventar
                inventoryOwner.RefreshUI();
                
            }

            Destroy(gameObject);
            return;
        }

        if (location == ItemLocation.Inventory)
        {
            bool used = ItemUseSystem.Instance != null && ItemUseSystem.Instance.TryUse(stack);

            if (used)                                                                                        //probiert item zu nutzen
            {
                inventoryOwner?.RemoveItem(stack);
                
            }
        }
    }
}