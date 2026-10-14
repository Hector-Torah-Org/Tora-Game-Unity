using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public static SceneManager Instance { get; private set; }

    [Header("Spawners")]
    [SerializeField] private ArrowSpawner arrowSpawner;
    [SerializeField] private DoorSpawner doorSpawner;
    [SerializeField] private ChestSpawner chestSpawner;
    [SerializeField] private InterSpawner interSpawner;

    [Header("Background")]
    [SerializeField] private SpriteRenderer backgroundRenderer;

    [Header("Scene Data Library")]
    [SerializeField] private List<SceneData> scenes = new List<SceneData>();

    private readonly List<ArrowScript> arrows = new List<ArrowScript>();

    private readonly Dictionary<string, bool> doorOpenState = new Dictionary<string, bool>();
    private readonly Dictionary<string, bool> chestOpenState = new Dictionary<string, bool>();
    private readonly Dictionary<string, List<ItemStack>> chestContentsState = new Dictionary<string, List<ItemStack>>();
    private readonly Dictionary<string, bool> interactableUsedState = new Dictionary<string, bool>();
    private readonly Dictionary<string, bool> sceneEventState = new Dictionary<string, bool>();

    private readonly Dictionary<int, int> sceneBackgroundState = new Dictionary<int, int>();

    private int currentSceneId = -1;
    public int CurrentSceneId => currentSceneId;

    public List<UIBlockLevel> uiLevelsBlockingWorldInput { get; set; } = new List<UIBlockLevel>();

    /// <summary>
    /// Any level stops interactions with the world, UI elements with a higher level stop all lower levels, UI elements from a lower might level still allow higher levels to be opened
    /// </summary>
    public enum UIBlockLevel
    {
        ChestInventory,
        Inventory,
        ToraGame,
        SceneTransition,
        Menu,
        ForcedTutorial,
        LoginInterface
    }

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
        QuestManager.Instance?.ClearSceneHint();

        interSpawner.DestroyAllSpawned();
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

        int currentBackgroundIndex = GetSceneBackgroundIndex(data.sceneId, data.defaultBackgroundIndex);

        //----------------------------------------------------------------- H I E R  K O M M E N  D I E  Q U E S T S  H I N -----------------------------------------------------------------
        if (sc == 3 && currentBackgroundIndex == 0)
        {
            QuestManager.Instance?.SetSceneHint("quest.hint.rubble");
            QuestManager.Instance?.AddQuest("quest.find.rubble_tool");
        }

        if (sc == 9 && currentBackgroundIndex == 0)
        {
            QuestManager.Instance?.SetSceneHint("quest.hint.mine");
            QuestManager.Instance?.AddQuest("quest.use.tool_on_planks");
        }

        if (sc == 11 && currentBackgroundIndex == 0)
        {
            QuestManager.Instance?.SetSceneHint("quest.hint.pedestal");
            QuestManager.Instance?.AddQuest("quest.find.pedestal_item");
        }

        if (sc == 17 && currentBackgroundIndex == 0)
        {
            QuestManager.Instance?.SetSceneHint("quest.hint.water");
            QuestManager.Instance?.AddQuest("quest.remove.water");
        }

        if (sc == 8 && currentBackgroundIndex == 0)
        {
            QuestManager.Instance?.SetSceneHint("quest.hint.bridge");
            QuestManager.Instance?.AddQuest("quest.restore.bridge");
        }

        if (sc == 18 && currentBackgroundIndex == 0)
        {
            QuestManager.Instance?.SetSceneHint("quest.hint.tile");
            QuestManager.Instance?.AddQuest("quest.lift.tile");
        }

        if (sc == 22 && currentBackgroundIndex == 0)
        {
            QuestManager.Instance?.SetSceneHint("quest.hint.vines");
            QuestManager.Instance?.AddQuest("quest.remove.vines");
        }

        if (sc == 19 && currentBackgroundIndex == 0)
        {
            QuestManager.Instance?.SetSceneHint("quest.hint.graveyard_gate");
            QuestManager.Instance?.AddQuest("quest.find.graveyard_item");
        }

        if (sc == 36 && currentBackgroundIndex == 0)
        {
            QuestManager.Instance?.SetSceneHint("quest.hint.church_symbol");
            QuestManager.Instance?.AddQuest("quest.use.symbol_item");
        }

        if (sc == 37 && currentBackgroundIndex == 0)
        {
            QuestManager.Instance?.SetSceneHint("quest.hint.another_symbol");
            QuestManager.Instance?.AddQuest("quest.find.skull_talisman");
        }

        if (sc == 38 && currentBackgroundIndex == 0)
        {
            QuestManager.Instance?.SetSceneHint("quest.hint.glowing_symbol");
            QuestManager.Instance?.AddQuest("quest.find.tree_talisman");
        }

        if (sc == 21 && currentBackgroundIndex == 0)
        {
            QuestManager.Instance?.SetSceneHint("quest.hint.dungeon_gate");
            QuestManager.Instance?.AddQuest("quest.get.dungeon_key");
        }

        if (sc == 26 && currentBackgroundIndex == 0)
        {
            QuestManager.Instance?.SetSceneHint("quest.hint.rope");
            QuestManager.Instance?.AddQuest("quest.use.rope");
        }

        //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        ApplySceneBackground(data);                                                                   // <<< NEU: Background setzen

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

        if (interSpawner == null)
        {
            Debug.LogError("InterSpawner not assigned in SceneManager.");
            return;
        }

        foreach (var inter in data.interactables)
        {
            

            if (data.sceneId == 18 && currentBackgroundIndex == 0 && inter.id == "key_trigger")
            {
                continue;
            }

            bool isUsed = GetInteractableIsUsed(inter.id, inter.startsUsed);
            interSpawner.CreateInteractable(inter, isUsed);
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
        {                                                                                            //Spawnt doors
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
            arrowSpawner.createArrow(a.position.x, a.position.y, a.rotationDegrees, a.sceneToCall, a.scale);
        }

        // SPECIFIC ARROW SITUATIONS, PLEASE DO NOT FORGET ----------------------------------------------------------------------------------------------------------------------------------------------------
        if (sc == 3 && GetSceneEventState("scene3_special_used"))
        {
            arrowSpawner.createArrow(1.5f, 0f, 90f, 8, 0.5f);
            QuestManager.Instance?.CompleteQuest("quest.find.rubble_tool");
        }
        if (sc == 9 && GetSceneEventState("scene9_special_used"))
        {
            arrowSpawner.createArrow(6.3f, 1.5f, -20f, 7, 0.5f);
            QuestManager.Instance?.CompleteQuest("quest.use.tool_on_planks");
        }
        if (sc == 11 && GetSceneEventState("scene11_special_used"))
        {
            arrowSpawner.createArrow(0.1f, 1f, 90f, 12, 0.6f);
            QuestManager.Instance?.CompleteQuest("quest.find.pedestal_item");
        }
        if (sc == 17 && GetSceneEventState("scene17_special_used"))
        {
            arrowSpawner.createArrow(-2.8f, -0.5f, 200f, 18, 0.8f);
            QuestManager.Instance?.CompleteQuest("quest.remove.water");
        }
        if (sc == 22 && GetSceneEventState("scene22_special_used"))
        {
            arrowSpawner.createArrow(0.8f, -0.5f, 90f, 25, 0.6f);
            QuestManager.Instance?.CompleteQuest("quest.remove.vines");
        }
        if (sc == 21 && GetSceneEventState("scene21_special_used"))
        {
            arrowSpawner.createArrow(-6.5f, -0.5f, 90f, 26, 0.6f);
            QuestManager.Instance?.CompleteQuest("quest.get.dungeon_key");
        }
        if (sc == 26 && GetSceneEventState("scene26_special_used"))
        {
            arrowSpawner.createArrow(-6f, -0.5f, 90f, 27, 0.6f);
            QuestManager.Instance?.CompleteQuest("quest.use.rope");
        }
        if (sc == 19 && GetSceneEventState("scene19_special_used"))
        {
            arrowSpawner.createArrow(4f, -0.5f, 60f, 30, 0.6f);
            QuestManager.Instance?.CompleteQuest("quest.find.graveyard_item");
        }
        if (sc == 36 && GetSceneEventState("scene36_special_used"))
        {
            arrowSpawner.createArrow(2f, 1f, 90f, 37, 0.7f);
            QuestManager.Instance?.CompleteQuest("quest.use.symbol_item");
        }
        if (sc == 37 && GetSceneEventState("scene37_special_used"))
        {
            arrowSpawner.createArrow(0f, -1f, 90f, 38, 0.7f);
            QuestManager.Instance?.CompleteQuest("quest.find.skull_talisman");
        }
        if (sc == 8 && GetSceneEventState("scene8_special_used"))
        {
            arrowSpawner.createArrow(2f, 1f, 35f, 33, 0.7f);
            QuestManager.Instance?.CompleteQuest("quest.restore.bridge");
        }
        // hmm---------------------------------------------------------------------------------------------
        if (sc == 38 && GetSceneEventState("scene38_special_used"))
        {
            QuestManager.Instance?.CompleteQuest("quest.find.tree_talisman");
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

    private void ApplySceneBackground(SceneData data)
    {
        if (backgroundRenderer == null)
        {
            Debug.LogError("BackgroundRenderer not assigned in SceneManager.");
            return;
        }

        if (data == null || data.backgrounds == null || data.backgrounds.Count == 0)
        {
            backgroundRenderer.sprite = null;
            return;
        }

        int index = GetSceneBackgroundIndex(data.sceneId, data.defaultBackgroundIndex);

        if (index < 0 || index >= data.backgrounds.Count)
        {
            index = 0;
        }

        BackgroundOption option = data.backgrounds[index];

        if (option == null || option.sprite == null)
        {
            Debug.LogWarning($"Scene {data.sceneId} background at index {index} is missing a sprite.");
            backgroundRenderer.sprite = null;
            return;
        }

        backgroundRenderer.sprite = option.sprite;
    }

    public int GetSceneBackgroundIndex(int sceneId, int fallback)
    {
        if (sceneBackgroundState.TryGetValue(sceneId, out int index))
            return index;

        return fallback;
    }

    public void SetSceneBackgroundIndex(int sceneId, int index)
    {
        SceneData data = GetSceneData(sceneId);
        if (data == null)
        {
            Debug.LogError($"SetSceneBackgroundIndex failed: no SceneData for sceneId={sceneId}");
            return;
        }

        if (data.backgrounds == null || data.backgrounds.Count == 0)
        {
            Debug.LogWarning($"Scene {sceneId} has no background options.");
            return;
        }

        if (index < 0 || index >= data.backgrounds.Count)
        {
            Debug.LogError($"SetSceneBackgroundIndex failed: index {index} is out of range for scene {sceneId}");
            return;
        }

        sceneBackgroundState[sceneId] = index;

        if (sceneId == currentSceneId)
            ApplySceneBackground(data);
    }

    public void SetCurrentSceneBackgroundIndex(int index)
    {
        if (currentSceneId < 0)
        {
            Debug.LogWarning("SetCurrentSceneBackgroundIndex failed: no current scene is active.");
            return;
        }

        SetSceneBackgroundIndex(currentSceneId, index);
    }



    public void ReloadCurrentScene()
    {
        if (currentSceneId < 0)
        {
            Debug.LogWarning("No current scene to reload.");
            return;
        }

        ChangeScene(currentSceneId);
    }

    public bool GetSceneEventState(string eventId, bool fallback = false)
    {
        if (sceneEventState.TryGetValue(eventId, out bool value))
            return value;

        return fallback;
    }

    public void SetSceneEventState(string eventId, bool value)
    {
        sceneEventState[eventId] = value;
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
        doorOpenState[doorId] = open;                                                                //irgendwann muss das gespeichert werden
    }

    public ArrowScript SpawnArrowOnDoor(Transform doorTransform, int nextSceneId, float rotationDeg, float scale = 1f)
    {
        if (arrowSpawner == null)
        {
            Debug.LogError("SpawnArrowOnDoor failed: ArrowSpawner not assigned in SceneManager.");
            return null;
        }

        Vector3 p = doorTransform.position;
        return arrowSpawner.createArrow(p.x, p.y, rotationDeg, nextSceneId, scale);
    }

    public void AddUIBlockingWorldInput(UIBlockLevel blockLevel)
    {
        uiLevelsBlockingWorldInput.Add(blockLevel);
    }

    public void RemoveUIBlockingWorldInput(UIBlockLevel blockLevel)
    {
        uiLevelsBlockingWorldInput.RemoveAll(block => block == blockLevel);
    }

    public bool GetInteractableIsUsed(string interactableId, bool fallback)
    {
        if (interactableUsedState.TryGetValue(interactableId, out bool used))
            return used;

        return fallback;
    }

    public void SetInteractableUsed(string interactableId, bool used)
    {
        interactableUsedState[interactableId] = used;
    }





    public bool IsDoorOpen(string doorId)
    {
        return GetDoorIsOpen(doorId, false);
    }

    public bool IsChestOpen(string chestId)
    {
        return GetChestIsOpen(chestId, false);
    }

    public bool IsInteractableUsed(string interactableId)
    {
        return GetInteractableIsUsed(interactableId, false);
    }

    public bool ChestContainsItem(string chestId, ItemData item)
    {
        if (item == null) return false;

        List<ItemStack> contents = GetChestContents(chestId, null);

        foreach (var stack in contents)
        {
            if (stack != null && stack.item == item && stack.amount > 0)
                return true;
        }

        return false;
    }
}