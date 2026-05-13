using UnityEngine;

public class InterScript : MonoBehaviour
{
    private InteractableSpawnData data;
    private bool isUsed;

    [SerializeField] private GameObject unusedVisual;
    [SerializeField] private GameObject usedVisual;

    private Inventory playerInventory;

    public void Init(InteractableSpawnData interactableData, bool used)
    {
        data = interactableData;

        transform.position = new Vector3(data.position.x, data.position.y, transform.position.z);
        transform.localScale = new Vector3(data.scale.x, data.scale.y, 1f);

        playerInventory = FindFirstObjectByType<Inventory>();

        SetUsed(used);
    }

    private void SetUsed(bool used)
    {
        isUsed = used;

        if (unusedVisual != null) { unusedVisual.SetActive(!isUsed); }
        if (usedVisual != null) { usedVisual.SetActive(isUsed); }
    }

    private void GiveReward()
    {
        if (playerInventory == null)
        {
            Debug.LogError("Inventory not found in scene.");
            return;
        }

        if (data.rewardItem == null)
        {
            Debug.LogWarning("Interactable has no reward item assigned.");
            return;
        }

        playerInventory.AddItem(new ItemStack
        {
            item = data.rewardItem,
            amount = data.rewardAmount

        });
        if (data.id == "well_trigger1")                     // bin noch am überlegen ob ich das hier lasse tbh
        {
            QuestManager.Instance?.AddQuest("Finde einen Ort für das Wasser");
        }
    }

    private void OnMouseDown()
    {
        if (SceneManager.Instance != null && SceneManager.Instance.IsUIBlockingWorldInput)
            return;

        if (isUsed)
            return;

        if (TorahManager.Instance == null)
        {
            Debug.LogWarning("TorahManager.Instance is null - no minigame started.");
            return;
        }

        TorahManager.Instance.StartRound(
            onWin: () =>
            {
                GiveReward();

                SceneManager.Instance?.SetInteractableUsed(data.id, true);
                SetUsed(true);
            },
            onLose: () => { }
        );
    }
}