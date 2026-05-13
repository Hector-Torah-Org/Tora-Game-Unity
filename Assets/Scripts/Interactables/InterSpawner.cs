using System.Collections.Generic;
using UnityEngine;

public class InterSpawner : MonoBehaviour
{
    private readonly List<InterScript> spawned = new List<InterScript>();

    public InterScript CreateInteractable(InteractableSpawnData data, bool isUsed)
    {
        if (data == null)
        {
            Debug.LogError("InteractableSpawnData is null.");
            return null;
        }

        if (data.prefab == null)
        {
            Debug.LogError($"Interactable prefab missing for id={data.id}");
            return null;
        }

        InterScript interactable = Instantiate(data.prefab);
        interactable.Init(data, isUsed);

        spawned.Add(interactable);
        return interactable;
    }

    public void DestroyAllSpawned()
    {
        for (int i = 0; i < spawned.Count; i++)
            if (spawned[i] != null) { Destroy(spawned[i].gameObject); }

        spawned.Clear();
    }
}