using System;
using System.Collections.Generic;
using UnityEngine;

public static class BoardJsonParser
{
    [Serializable]
    private class TileJsonData
    {
        public string itemType;
        public int amount;
    }

    [Serializable]
    private class BoardJsonData
    {
        public TileJsonData[] tiles;
    }

    public static List<TileModel> ParseFromJson(string json)
    {
        var result = new List<TileModel>();

        if (string.IsNullOrWhiteSpace(json))
        {
            Debug.LogError("[BoardJsonParser] JSON string is empty or null.");
            return result;
        }

        var boardData = JsonUtility.FromJson<BoardJsonData>(json);

        if (boardData?.tiles == null)
        {
            Debug.LogError("[BoardJsonParser] Failed to parse JSON or 'tiles' array is missing.");
            return result;
        }

        for (int i = 0; i < boardData.tiles.Length; i++)
        {
            var tileData = boardData.tiles[i];

            if (!Enum.TryParse<ItemType>(tileData.itemType, ignoreCase: true, out var itemType))
                itemType = ItemType.None;

            var reward = new TileRewardModel(itemType, tileData.amount);
            result.Add(new TileModel(i, reward));
        }

        return result;
    }
}
