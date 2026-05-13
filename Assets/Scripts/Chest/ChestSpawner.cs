using System.Collections.Generic;
using UnityEngine;

public class ChestSpawner : MonoBehaviour
{
    [SerializeField] private ChestScript chestPrefab;

    private readonly List<ChestScript> spawned = new List<ChestScript>();

    public ChestScript CreateChest(ChestSpawnData data, bool isOpen, List<ItemStack> contents)
    {
        ChestScript chest = Instantiate(chestPrefab);
        chest.Init(data, isOpen, contents);

        spawned.Add(chest);
        return chest;
    }

    public void DestroyAllSpawned()
    {
        for (int i = 0; i < spawned.Count; i++)
        {
            if (spawned[i] != null) { Destroy(spawned[i].gameObject); }
        }
            

        spawned.Clear();
    }
}