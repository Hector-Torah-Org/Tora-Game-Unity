using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    private ChestSpawnData data;
    private bool isOpen;
    private List<ItemStack> contents; 

    [SerializeField] private GameObject closedVisual;
    [SerializeField] private GameObject openVisual;

    public void Init(ChestSpawnData chestData, bool open, List<ItemStack> runtimeContents)
    {
        data = chestData;
        contents = runtimeContents;

        transform.position = new Vector3(data.position.x, data.position.y, transform.position.z);
        transform.localScale = new Vector3(data.scale.x, data.scale.y, 1f);

        SetOpen(open);
    }

    private void SetOpen(bool open)
    {
        isOpen = open;

        if (closedVisual != null) closedVisual.SetActive(!isOpen);
        if (openVisual != null) openVisual.SetActive(isOpen);
    }

    public void RemoveItem(ItemStack stack)
    {
        if (stack == null) return;
        contents.Remove(stack);
    }

    private void OnMouseDown()
    {
        if (SceneManager.Instance != null && SceneManager.Instance.IsUIBlockingWorldInput)
            return;

        if (!isOpen)
        {
            if (TorahManager.Instance == null)
            {
                Debug.LogWarning("TorahManager.Instance is null - no minigame started.");
                return;
            }

            TorahManager.Instance.StartRound(
                onWin: () =>
                {
                    SceneManager.Instance?.SetChestOpen(data.id, true);
                    SetOpen(true);
                },
                onLose: () => { }
            );

            return;
        }

        
        if (ChestUI.Instance != null)
        {
            ChestUI.Instance.Open(this, contents);
        }
        else
        {
            Debug.LogError("ChestUI.Instance is null. Add ChestUI to the scene and assign references.");
        }
    }
}