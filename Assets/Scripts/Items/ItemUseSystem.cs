using UnityEngine;

public class ItemUseSystem : MonoBehaviour
{
    public static ItemUseSystem Instance { get; private set; }

    [SerializeField] private Inventory inventory;

    [SerializeField] private ItemData triggerItem;
    [SerializeField] private int targetSceneId = 0;
    [SerializeField] private int newBackgroundIndex = 1;
    [SerializeField] private string eventId = "scene3_special_used";
    
    [SerializeField] private ItemData triggerItem2;
    [SerializeField] private int targetSceneId2 = 0;
    [SerializeField] private int newBackgroundIndex2 = 1;
    [SerializeField] private string eventId2 = "scene9_special_used";

    [SerializeField] private ItemData triggerItem3;
    [SerializeField] private int targetSceneId3 = 0;
    [SerializeField] private int newBackgroundIndex3 = 1;
    [SerializeField] private string eventId3 = "scene11_special_used";

    [SerializeField] private ItemData triggerItem4;
    [SerializeField] private int targetSceneId4 = 0;
    [SerializeField] private int newBackgroundIndex4 = 1;
    [SerializeField] private string eventId4 = "scene17_special_used";

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public bool TryUse(ItemStack stack)
    {
        if (stack == null || stack.item == null)
            return false;

        Debug.Log($"Trying to use item: {stack.item.displayName}");

        if (inventory == null)
        {
            Debug.LogError("ItemUseSystem: Inventory not assigned.");
            return false;
        }

        if (SceneManager.Instance == null)
        {
            Debug.LogError("ItemUseSystem: SceneManager-Instance is null.");
            return false;
        }

        // -------------------------------------------------------------------------- DONT FORGET TO ADD THIS ASWELL --------------------------------------------------------------------

        Debug.Log("Stack.item: " + stack.item);
        Debug.Log("trigger item: " + triggerItem4);
        Debug.Log("CurrentSceneId: " + SceneManager.Instance.CurrentSceneId);
        Debug.Log("targetSceneId: " + targetSceneId4);

        if (stack.item == triggerItem && SceneManager.Instance.CurrentSceneId == targetSceneId)
        {
            UseSpecialItem(stack, newBackgroundIndex, eventId);
            return true;
        }

        if (stack.item == triggerItem2 && SceneManager.Instance.CurrentSceneId == targetSceneId2)
        {
            UseSpecialItem(stack, newBackgroundIndex2, eventId2);
            return true;
        }

        if (stack.item == triggerItem3 && SceneManager.Instance.CurrentSceneId == targetSceneId3)
        {
            UseSpecialItem(stack, newBackgroundIndex3, eventId3);
            return true;
        }

        if (stack.item == triggerItem4 && SceneManager.Instance.CurrentSceneId == targetSceneId4)
        {
            UseSpecialItem(stack, newBackgroundIndex4, eventId4);
            return true;
        }
        // ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        Debug.Log("Item doesnt work here.");
        return false;
    }

    private void UseSpecialItem(ItemStack stack, int backgroundIndex, string usedEventId)
    {
        SceneManager.Instance.SetCurrentSceneBackgroundIndex(backgroundIndex);
        SceneManager.Instance.SetSceneEventState(usedEventId, true);

        if (stack.amount > 1)
        {
            stack.amount--;
        }
        else
        {
            inventory.RemoveItem(stack);
        }

        inventory.RefreshUI();
        SceneManager.Instance.ReloadCurrentScene();

        Debug.Log("Special item used successfully.");
    }
}