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

    [SerializeField] private ItemData triggerItem5;
    [SerializeField] private int targetSceneId5 = 22;
    [SerializeField] private int newBackgroundIndex5 = 1;
    [SerializeField] private string eventId5 = "scene22_special_used";

    [SerializeField] private ItemData triggerItem6;
    [SerializeField] private int targetSceneId6 = 21;
    [SerializeField] private int newBackgroundIndex6 = 1;
    [SerializeField] private string eventId6 = "scene21_special_used";

    [SerializeField] private ItemData triggerItem7;
    [SerializeField] private int targetSceneId7 = 19;
    [SerializeField] private int newBackgroundIndex7 = 1;
    [SerializeField] private string eventId7 = "scene19_special_used";

    [SerializeField] private ItemData triggerItem8;
    [SerializeField] private int targetSceneId8 = 26;
    [SerializeField] private int newBackgroundIndex8 = 1;
    [SerializeField] private string eventId8 = "scene26_special_used";

    [SerializeField] private ItemData triggerItem9;
    [SerializeField] private int targetSceneId9 = 36;
    [SerializeField] private int newBackgroundIndex9 = 1;
    [SerializeField] private string eventId9 = "scene36_special_used";

    [SerializeField] private ItemData triggerItem10;
    [SerializeField] private int targetSceneId10 = 18;
    [SerializeField] private int newBackgroundIndex10 = 1;
    [SerializeField] private string eventId10 = "scene18_special_used";

    [SerializeField] private ItemData triggerItem11;
    [SerializeField] private int targetSceneId11 = 37;
    [SerializeField] private int newBackgroundIndex11 = 1;
    [SerializeField] private string eventId11 = "scene37_special_used";

    [SerializeField] private ItemData triggerItem12;
    [SerializeField] private int targetSceneId12 = 8;
    [SerializeField] private int newBackgroundIndex12 = 1;
    [SerializeField] private string eventId12 = "scene8_special_used";

    [SerializeField] private ItemData triggerItem13;
    [SerializeField] private int targetSceneId13 = 38;
    [SerializeField] private int newBackgroundIndex13 = 1;
    [SerializeField] private string eventId13 = "scene38_special_used";

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

        if (stack.item == triggerItem5 && SceneManager.Instance.CurrentSceneId == targetSceneId5)
        {
            UseSpecialItem(stack, newBackgroundIndex5, eventId5);
            return true;
        }

        if (stack.item == triggerItem6 && SceneManager.Instance.CurrentSceneId == targetSceneId6)
        {
            UseSpecialItem(stack, newBackgroundIndex6, eventId6);
            return true;
        }

        if (stack.item == triggerItem7 && SceneManager.Instance.CurrentSceneId == targetSceneId7)
        {
            UseSpecialItem(stack, newBackgroundIndex7, eventId7);
            return true;
        }

        if (stack.item == triggerItem8 && SceneManager.Instance.CurrentSceneId == targetSceneId8)
        {
            UseSpecialItem(stack, newBackgroundIndex8, eventId8);
            return true;
        }

        if (stack.item == triggerItem9 && SceneManager.Instance.CurrentSceneId == targetSceneId9)
        {
            UseSpecialItem(stack, newBackgroundIndex9, eventId9);
            return true;
        }

        if (stack.item == triggerItem10 && SceneManager.Instance.CurrentSceneId == targetSceneId10)
        {
            UseSpecialItem(stack, newBackgroundIndex10, eventId10);
            return true;
        }

        if (stack.item == triggerItem11 && SceneManager.Instance.CurrentSceneId == targetSceneId11)
        {
            UseSpecialItem(stack, newBackgroundIndex11, eventId11);
            return true;
        }

        if (stack.item == triggerItem12 && SceneManager.Instance.CurrentSceneId == targetSceneId12)
        {
            UseSpecialItem(stack, newBackgroundIndex12, eventId12);
            return true;
        }

        if (stack.item == triggerItem13 && SceneManager.Instance.CurrentSceneId == targetSceneId13)
        {
            UseSpecialItem(stack, newBackgroundIndex13, eventId13);
            return true;
        }
        // ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        Debug.Log("Item doesnt work here.");
        return false;
    }

    private void UseSpecialItem(ItemStack stack, int backgroundIndex, string usedEventId)
    {

        if (stack.amount > 1)
        {
            stack.amount--;
        }
        else
        {
            inventory.RemoveItem(stack);
        }

        inventory.RefreshUI();
        SceneTransition.Instance.FadeReloadCurrentScene(
            backgroundIndex,
            usedEventId
        );  // <------ wie du siehst nutze ich jetzt das neue mit fade, statt das normale neuladen

        Debug.Log("Special item used successfully.");
    }
}