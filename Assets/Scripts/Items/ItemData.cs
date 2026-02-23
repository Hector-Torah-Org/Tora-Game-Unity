using UnityEngine;

[CreateAssetMenu(menuName = "Game/Item Data")]
public class ItemData : ScriptableObject
{
    public int id;
    public string displayName;
    public Sprite icon;
}
