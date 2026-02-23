using UnityEngine;

public class ItemUseSystem : MonoBehaviour
{
    public static ItemUseSystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public bool TryUse(ItemStack stack)
    {
        Debug.Log($"Trying to use item: {stack.item.displayName}");            //NOCH NICHT GEMACHT, gerade tuen die items nichts
        return true; 
    }
}