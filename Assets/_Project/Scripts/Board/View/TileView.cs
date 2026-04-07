using System.Collections;
using UnityEngine;
using TMPro;

public class TileView : MonoBehaviour
{
    [SerializeField] private TextMeshPro indexText;
    [SerializeField] private TextMeshPro rewardText;
    [SerializeField] private MeshRenderer tileRenderer;
    [SerializeField] private SpriteRenderer rewardIconRenderer;

    [Header("Colors")]
    [SerializeField] private Color emptyColor;
    [SerializeField] private Color rewardColor;

    [Header("Highlight Animation")]
    [SerializeField] private Vector3 highlightScale = new Vector3(1.2f, 1.2f, 1.2f);
    [SerializeField] private float highlightDuration = 0.2f;
    [SerializeField] private AnimationCurve highlightCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    private TileModel tileModel;
    private Coroutine scaleCoroutine;
    private Vector3 baseScale;

    private void Awake() => baseScale = transform.localScale;

    public void Setup(TileModel model, GameConfigSO gameConfig)
    {
        tileModel = model;

        indexText.text = (model.index+1).ToString();

        if (model.hasReward)
        {
            rewardText.text = $"{model.reward.amount}";
            tileRenderer.material.color = rewardColor;

            if (rewardIconRenderer != null && gameConfig != null)
            {
                Sprite icon = null;
                foreach (var item in gameConfig.items)
                {
                    if (item != null && item.itemType == model.reward.itemType)
                    {
                        icon = item.icon;
                        break;
                    }
                }
                rewardIconRenderer.sprite = icon;
                rewardIconRenderer.enabled = icon != null;
            }
            else
            {
                Debug.LogError("RewardIconRenderer or GameConfig is null. rewardIconRenderer: " + rewardIconRenderer + " gameConfig: " + gameConfig, this);
            }
        }
        else
        {
            rewardText.gameObject.SetActive(false);
            tileRenderer.material.color = emptyColor;

            if (rewardIconRenderer != null)
                rewardIconRenderer.enabled = false;
        }
    }

    public void Highlight(bool isActive)
    {
        Vector3 target = isActive ? highlightScale : baseScale;

        if (scaleCoroutine != null) StopCoroutine(scaleCoroutine);
        scaleCoroutine = StartCoroutine(AnimateScale(target));
    }

    private IEnumerator AnimateScale(Vector3 target)
    {
        Vector3 start = transform.localScale;
        float elapsed = 0f;

        while (elapsed < highlightDuration)
        {
            float t = highlightCurve.Evaluate(Mathf.Clamp01(elapsed / highlightDuration));
            transform.localScale = Vector3.LerpUnclamped(start, target, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = target;
        scaleCoroutine = null;
    }
}