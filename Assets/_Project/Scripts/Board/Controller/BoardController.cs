using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BoardController : MonoBehaviour, IInitializable
{
    [SerializeField] private BoardConfigSO boardConfig;

    private BoardModel boardModel;
    private int currentTileIndex;

    public void Initialize()
    {
        boardModel = BuildBoard();
        currentTileIndex = 0;

        EventBus.Subscribe<OnDiceRolled>(HandleDiceRolled);
        EventBus.Subscribe<OnPlayerLanded>(HandlePlayerLanded);

        EventBus.Raise(new OnBoardBuilt { tiles = boardModel.tiles });
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<OnDiceRolled>(HandleDiceRolled);
        EventBus.Unsubscribe<OnPlayerLanded>(HandlePlayerLanded);
    }

    private void HandleDiceRolled(OnDiceRolled e)
    {
        int targetIndex = boardModel.WrapIndex(currentTileIndex, e.totalValue);

        EventBus.Raise(new OnPlayerMoveRequested
        {
            fromIndex      = currentTileIndex,
            targetIndex    = targetIndex,
            steps          = e.totalValue,
            totalTileCount = boardModel.tileCount
        });
    }

    private void HandlePlayerLanded(OnPlayerLanded e)
    {
        currentTileIndex = e.tileIndex;

        var tile = boardModel.GetTile(currentTileIndex);

        if (tile != null && tile.hasReward)
            EventBus.Raise(new OnTileRewardCollected { reward = tile.reward });
    }

    private BoardModel BuildBoard()
    {
        string jsonPath = Path.Combine(Application.streamingAssetsPath, "board_data.json");

        if (File.Exists(jsonPath))
        {
            string json = File.ReadAllText(jsonPath);
            var jsonTiles = BoardJsonParser.ParseFromJson(json);

            if (jsonTiles != null && jsonTiles.Count > 0)
                return new BoardModel(jsonTiles);
        }

        return BuildBoardFromSO();
    }

    private BoardModel BuildBoardFromSO()
    {
        var tileList = new List<TileModel>();
        var tileDataArray = boardConfig.tileData.tiles;

        for (int i = 0; i < tileDataArray.Length; i++)
        {
            var data = tileDataArray[i];
            var reward = new TileRewardModel(data.itemType, data.amount);
            tileList.Add(new TileModel(i, reward));
        }

        return new BoardModel(tileList);
    }
}
