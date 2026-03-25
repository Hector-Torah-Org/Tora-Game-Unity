using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Scene Data")]
public class SceneData : ScriptableObject
{
    public int sceneId;
    public string displayName;

    [Header("Backgrounds")]
    public List<BackgroundOption> backgrounds = new List<BackgroundOption>();
    public int defaultBackgroundIndex = 0;

    [Header("Components")]
    public List<ArrowSpawnData> arrows = new List<ArrowSpawnData>();
    public List<DoorSpawnData> doors = new List<DoorSpawnData>();
    public List<ChestSpawnData> chests = new List<ChestSpawnData>();
    public List<InteractableSpawnData> interactables = new List<InteractableSpawnData>();
}

[Serializable]
public class BackgroundOption
{
    public string id;
    public Sprite sprite;
}

[Serializable]
public class ArrowSpawnData
{
    public Vector2 position;
    public float rotationDegrees;
    public int sceneToCall;
    public float scale = 1f;
}

[Serializable]
public class ChestSpawnData
{
    public string id;
    public Vector2 position;
    public Vector2 scale;
    public bool startsOpen;

    public List<ItemStack> contents = new List<ItemStack>();
}

[Serializable]
public class DoorSpawnData
{
    public string id;
    public Vector2 position;
    public Vector2 scale;
    public bool startsOpen;
    public int nextSceneId;
    public float arrowRotation;
    public float arrowScale = 1f;
}

[Serializable]
public class InteractableSpawnData
{
    public string id;
    public InterScript prefab;

    public Vector2 position;
    public Vector2 scale = Vector2.one;

    public bool startsUsed;

    public ItemData rewardItem;
    public int rewardAmount = 1;
}