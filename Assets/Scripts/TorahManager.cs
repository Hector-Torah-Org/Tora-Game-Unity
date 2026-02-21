using System;
using UnityEngine;

public class TorahManager : MonoBehaviour
{
    public static TorahManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartRound(Action onWin, Action onLose)
    {
        Debug.Log("Torah round started (placeholder).");

        
        onWin?.Invoke();                                                  // TEMPORARY, simuliert den Win 
    }
}
