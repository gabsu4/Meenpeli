using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemName = "New Item";
    public Sprite icon;
    public ItemType itemType;
}

public enum ItemType
{
    Gun
}