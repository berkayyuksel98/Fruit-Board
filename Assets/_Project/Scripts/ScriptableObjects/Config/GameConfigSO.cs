using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Config/Game Config")]
public class GameConfigSO : ScriptableObject
{
    [Header("Dice Settings")]
    public int defaultDiceCount = 2;
    public int minDiceCount = 1;
    public int maxDiceCount = 20;

    [Header("Animation Settings")]
    public float diceAnimationDuration = 1f;
    public float playerMoveDuration = 0.3f;

    [Header("Item Data")]
    public ItemDataSO[] items;
}
