using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour, IInitializable
{
    [SerializeField] private GameConfigSO gameConfig;
    [SerializeField] private PlayerView playerView;

    private PlayerModel playerModel;

    public void Initialize()
    {
        playerModel = new PlayerModel(0);
        EventBus.Subscribe<OnPlayerMoveRequested>(HandleMoveRequested);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<OnPlayerMoveRequested>(HandleMoveRequested);
    }

    private void HandleMoveRequested(OnPlayerMoveRequested e)
    {
        StartCoroutine(MoveStepByStep(e));
    }

    private IEnumerator MoveStepByStep(OnPlayerMoveRequested e)
    {
        int current = e.fromIndex;

        for (int step = 0; step < e.steps; step++)
        {
            current = (current + 1) % e.totalTileCount;
            playerModel.MoveTo(current);

            playerView.MoveToTile(current);

            EventBus.Raise(new OnPlayerMoved { tileIndex = current });

            yield return new WaitForSeconds(gameConfig.playerMoveDuration);
        }

        EventBus.Raise(new OnPlayerLanded { tileIndex = playerModel.currentTileIndex });
        EventBus.Raise(new OnGameInputLocked { isLocked = false });
    }
}
