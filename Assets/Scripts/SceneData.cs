using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Scene Data")]
public class SceneData : ScriptableObject
{
    public int sceneId;
    public string displayName;

    [Header("Arrows")]
    public List<ArrowSpawnData> arrows = new List<ArrowSpawnData>();
    public List<DoorSpawnData> doors = new List<DoorSpawnData>();
    [Header("Placeholders (later)")]
    public List<ChestSpawnData> chests = new List<ChestSpawnData>();
    
}

[Serializable]
public class ArrowSpawnData
{
    public Vector2 position;
    public float rotationDegrees;
    public int sceneToCall;
}


[Serializable]
public class ChestSpawnData
{                                                                          //Placeholder, also ignorieren für den moment
    public Vector2 position;
    public int chestTypeId;
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
}