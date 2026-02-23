using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance { get; private set; }

    [Header("Spawners")]
    [SerializeField] private ArrowSpawner arrowSpawner;
    [SerializeField] private DoorSpawner doorSpawner;
    [SerializeField] private ChestSpawner chestSpawner;

    [Header("Scene Data Library")]
    [SerializeField] private List<SceneData> scenes = new List<SceneData>();

    private readonly List<ArrowScript> arrows = new List<ArrowScript>();

    private readonly Dictionary<string, bool> doorOpenState = new Dictionary<string, bool>();
    private readonly Dictionary<string, bool> chestOpenState = new Dictionary<string, bool>();
    private readonly Dictionary<string, List<ItemStack>> chestContentsState = new Dictionary<string, List<ItemStack>>();

    private int currentSceneId = -1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        ChangeScene(0);                                                                          //Ich starte einfach mal in scene 0
    }

    public void RegisterArrow(ArrowScript arrow)
    {
        if (arrow != null && !arrows.Contains(arrow))
            arrows.Add(arrow);
    }

    public void UnregisterArrow(ArrowScript arrow)
    {
        if (arrow != null)
            arrows.Remove(arrow);
    }

    public void ChangeScene(int sc)
    {
        doorSpawner.DestroyAllSpawned();
        chestSpawner.DestroyAllSpawned();

        for (int i = 0; i < arrows.Count; i++)
        {
            if (arrows[i] != null)
                Destroy(arrows[i].gameObject);
        }
        arrows.Clear();

        SceneData data = GetSceneData(sc);                                                             //Daten werden abgerufen
        if (data == null)                                                                              
        {
            Debug.LogError($"No SceneData found for sceneId={sc}.");
            return;
        }

        currentSceneId = sc;

        if (chestSpawner == null)
        {
            Debug.LogError("ChestSpawner not assigned in SceneManager.");
            return;
        }

        foreach (var c in data.chests)
        {
            bool isOpen = GetChestIsOpen(c.id, c.startsOpen);
            List<ItemStack> contents = GetChestContents(c.id, c.contents);
            chestSpawner.CreateChest(c, isOpen, contents);
        }


        if (doorSpawner != null)
            doorSpawner.DestroyAllSpawned();                                                          //alte doors weg

        
        for (int i = 0; i < arrows.Count; i++)
        {                                                                                            //alte arrows weg
            if (arrows[i] != null)
                Destroy(arrows[i].gameObject);
        }
        arrows.Clear();


        
        if (doorSpawner == null)
        {
            Debug.LogError("DoorSpawner not assigned in SceneManager.");                                
            return;
        }
                                                
        foreach (var d in data.doors)
        {                                                                                                //Spawnt doors
            bool isOpen = GetDoorIsOpen(d.id, d.startsOpen);
            doorSpawner.CreateDoor(d, isOpen);
        }

    
        if (arrowSpawner == null)
        {                                                                                                 
            Debug.LogError("ArrowSpawner not assigned in SceneManager.");
            return;
        }

        foreach (var a in data.arrows)                                                                      //Spawnt arrows
        {
            arrowSpawner.createArrow(a.position.x, a.position.y, a.rotationDegrees, a.sceneToCall);
        }

        Debug.Log($"Loaded scene data: {data.displayName} (id={data.sceneId})");
    }


    private SceneData GetSceneData(int id)
    {
        for (int i = 0; i < scenes.Count; i++)
        {
            if (scenes[i] != null && scenes[i].sceneId == id)
                return scenes[i];
        }
        return null;
    }

    public bool GetChestIsOpen(string chestId, bool fallback)
    {
        if (chestOpenState.TryGetValue(chestId, out bool open))
            return open;
        return fallback;
    }

    public void SetChestOpen(string chestId, bool open)
    {
        chestOpenState[chestId] = open;
    }

    public List<ItemStack> GetChestContents(string chestId, List<ItemStack> fallbackTemplate)
    {
        if (chestContentsState.TryGetValue(chestId, out var list) && list != null)
            return list;

        var copy = new List<ItemStack>();
        if (fallbackTemplate != null)
        {
            foreach (var s in fallbackTemplate)
            {
                if (s != null && s.item != null)
                    copy.Add(new ItemStack { item = s.item, amount = s.amount });
            }
        }

        chestContentsState[chestId] = copy;
        return copy;
    }

    public bool GetDoorIsOpen(string doorId, bool fallback)
    {
        if (doorOpenState.TryGetValue(doorId, out bool open))
            return open;
        return fallback;
    }
                   
    public void SetDoorOpen(string doorId, bool open)
    {
        doorOpenState[doorId] = open;                                                                    //irgendwann muss das gespeichert werden
        
    }

    public ArrowScript SpawnArrowOnDoor(Transform doorTransform, int nextSceneId, float rotationDeg)
    {
        if (arrowSpawner == null)
        {
            Debug.LogError("SpawnArrowOnDoor failed: ArrowSpawner not assigned in SceneManager.");
            return null;
        }

        Vector3 p = doorTransform.position + Vector3.up;                   
        return arrowSpawner.createArrow(p.x, p.y, rotationDeg, nextSceneId);
    }
    public bool IsUIBlockingWorldInput { get; private set; }

    public void SetUIBlockingWorldInput(bool block)
    {
        IsUIBlockingWorldInput = block;
    }
}
