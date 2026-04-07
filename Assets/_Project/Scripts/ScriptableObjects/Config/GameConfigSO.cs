using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Config/Game Config")]
public class GameConfigSO : ScriptableObject
{
    [Header("Dice Settings")]
    public int defaultDiceCount = 2;
    public int minDiceCount = 1;
    public int maxDiceCount = 20;

    [Header("Animation Settings")]
    public float playerMoveDuration = 0.3f;
    public float playerWaitTime = 0.1f;

    [Header("Item Data")]
    public ItemDataSO[] items;
}
