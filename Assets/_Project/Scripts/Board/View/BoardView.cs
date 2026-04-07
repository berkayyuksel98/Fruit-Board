using System.Collections.Generic;
using UnityEngine;

public class BoardView : MonoBehaviour, IInitializable
{
    [SerializeField] private BoardConfigSO boardConfig;
    [SerializeField] private GameConfigSO gameConfig;
    [SerializeField] private Transform boardParent;

    private readonly List<TileView> tileViews = new();
    private int currentHighlightedIndex = 0;

    public void Initialize()
    {
        EventBus.Subscribe<OnBoardBuilt>(HandleBoardBuilt);
        EventBus.Subscribe<OnPlayerMoved>(HandlePlayerMoved);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<OnBoardBuilt>(HandleBoardBuilt);
        EventBus.Unsubscribe<OnPlayerMoved>(HandlePlayerMoved);
    }

    private void HandleBoardBuilt(OnBoardBuilt e) => BuildBoard(e.tiles);

    private void HandlePlayerMoved(OnPlayerMoved e) => HighlightTile(e.tileIndex);

    private void BuildBoard(IReadOnlyList<TileModel> tiles)
    {
        foreach (var view in tileViews)
            if (view != null) Destroy(view.gameObject);

        tileViews.Clear();
        currentHighlightedIndex = 0;

        foreach (var tile in tiles)
        {
            var position = new Vector3(tile.index * boardConfig.tileSpacing, 0f, 0f);
            var go = Instantiate(boardConfig.tilePrefab, position, Quaternion.identity, boardParent);

            var tileView = go.GetComponent<TileView>();
            tileView.Setup(tile, gameConfig);
            tileViews.Add(tileView);
        }

        HighlightTile(0);
    }

    private void HighlightTile(int index)
    {
        if (currentHighlightedIndex >= 0 && currentHighlightedIndex < tileViews.Count)
            tileViews[currentHighlightedIndex].Highlight(false);

        currentHighlightedIndex = index;

        if (currentHighlightedIndex >= 0 && currentHighlightedIndex < tileViews.Count)
            tileViews[currentHighlightedIndex].Highlight(true);
    }
}
