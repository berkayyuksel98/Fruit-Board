using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Data/Item Data")]
public class ItemDataSO : ScriptableObject
{
    public ItemType itemType;
    public string displayName;
    public Sprite icon;
    public Color color;
}
