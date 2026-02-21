using System.Collections.Generic;
using UnityEngine;

public class DoorSpawner : MonoBehaviour
{
    [SerializeField] private DoorScript doorPrefab;

    private readonly List<DoorScript> spawned = new List<DoorScript>();

    public DoorScript CreateDoor(DoorSpawnData data, bool isOpen)
    {
        DoorScript door = Instantiate(doorPrefab);
        door.Init(data, isOpen);

        spawned.Add(door);
        return door;
    }                                                             

    public void DestroyAllSpawned()
    {
        for (int i = 0; i < spawned.Count; i++)
            if (spawned[i] != null) Destroy(spawned[i].gameObject);
        spawned.Clear();
    }
}
