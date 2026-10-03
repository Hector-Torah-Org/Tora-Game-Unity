using UnityEngine;

[CreateAssetMenu(menuName = "Game/Item Data")]
public class ItemData : ScriptableObject
{
    public int id;
    public string displayName;
    public Sprite icon;

    [Header("Readable Item")]
    public bool isReadable = false;

    public string readableTitle;

    [TextArea(10, 30)]
    public string readableText;
}
