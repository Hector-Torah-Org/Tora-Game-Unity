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