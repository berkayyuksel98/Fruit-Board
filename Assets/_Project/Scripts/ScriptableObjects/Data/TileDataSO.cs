using UnityEngine;

[System.Serializable]
public class TileData
{
    public ItemType itemType;
    public int amount;
}

[CreateAssetMenu(fileName = "TileData", menuName = "Data/Tile Data")]
public class TileDataSO : ScriptableObject
{
    public TileData[] tiles;
}