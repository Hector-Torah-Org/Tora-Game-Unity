using UnityEngine;

[CreateAssetMenu(menuName = "Game/Item Data")]
public class ItemData : ScriptableObject
{
    public int id;
    public string displayName;
    public Sprite icon;

    public string LocalizedDisplayName
    {
        get { return I18n.Translate("item." + id + ".name", displayName); }
    }

    [Header("Readable Item")]
    public bool isReadable = false;

    public string readableTitle;

    [TextArea(10, 30)]
    public string readableText;

    public string LocalizedReadableTitle
    {
        get { return I18n.Translate("item." + id + ".readable.title", readableTitle); }
    }

    public string LocalizedReadableText
    {
        get { return I18n.Translate("readable.placeholder", readableText); }
    }
}
