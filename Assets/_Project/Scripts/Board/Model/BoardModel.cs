using System.Collections.Generic;
using UnityEngine;

public class BoardModel
{
    private readonly List<TileModel> tileList;
    public int tileCount => tileList.Count;
    public IReadOnlyList<TileModel> tiles => tileList;

    public BoardModel(List<TileModel> tiles)
    {
        tileList = tiles;
    }

    public TileModel GetTile(int index)
    {
        if (index < 0 || index >= tileList.Count)
        {
            Debug.LogError($"Tile index {index} is out of bounds!");
            return null;
        }
        return tileList[index];
    }

    public int WrapIndex(int currentIndex, int steps)
    {
        return (currentIndex + steps) % tileList.Count;
    }
}
