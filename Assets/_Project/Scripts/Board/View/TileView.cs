using UnityEngine;
using TMPro;

public class TileView : MonoBehaviour
{
    [SerializeField] private TextMeshPro indexText;
    [SerializeField] private TextMeshPro rewardText;
    [SerializeField] private MeshRenderer tileRenderer;

    [Header("Colors")]
    [SerializeField] private Color emptyColor;
    [SerializeField] private Color rewardColor;

    private TileModel tileModel;

    public void Setup(TileModel model)
    {
        tileModel = model;

        indexText.text = model.index.ToString();

        if (model.hasReward)
        {
            rewardText.text = $"{model.reward.amount} {model.reward.itemType}";
            tileRenderer.material.color = rewardColor;
        }
        else
        {
            rewardText.text = "Empty";
            tileRenderer.material.color = emptyColor;
        }
    }

    public void Highlight(bool isActive)
    {
        transform.localScale = isActive
            ? Vector3.one * 1.2f
            : Vector3.one;
    }
}