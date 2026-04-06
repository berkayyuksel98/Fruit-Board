using UnityEngine;

[CreateAssetMenu(fileName = "BoardConfig", menuName = "Config/Board Config")]
public class BoardConfigSO : ScriptableObject
{
    [Header("Board Settings")]
    public float tileSpacing = 2f;
    public int defaultTileCount = 20;

    [Header("References")]
    public TileDataSO tileData;
    public GameObject tilePrefab;
}
