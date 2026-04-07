using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceView : MonoBehaviour, IInitializable
{
    [SerializeField] private GameConfigSO gameConfig;
    [SerializeField] private Transform diceParent;
    [SerializeField] private Dice dicePrefab;
    [SerializeField] private Transform landingTarget;

    [Header("Hide Animation")]
    [SerializeField] private AnimationCurve shrinkCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
    [SerializeField] private float shrinkDuration = 0.3f;

    private readonly List<Dice> diceObjects = new();
    private List<Vector3> lastOccupiedEndPositions = new();

    public void Initialize()
    {
        EventBus.Subscribe<OnDiceCountChanged>(HandleDiceCountChanged);
        EventBus.Subscribe<OnDiceRolled>(HandleDiceRolled);
        SpawnDice(gameConfig.defaultDiceCount);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<OnDiceCountChanged>(HandleDiceCountChanged);
        EventBus.Unsubscribe<OnDiceRolled>(HandleDiceRolled);
    }

    private void HandleDiceCountChanged(OnDiceCountChanged e) => SpawnDice(e.count);

    private void HandleDiceRolled(OnDiceRolled e)
    {
        int total = diceObjects.Count;
        var occupiedEndPositions = new List<Vector3>();

        // Dice'ları göster
        for (int i = 0; i < total; i++)
        {
            diceObjects[i].gameObject.SetActive(true);
            diceObjects[i].transform.localScale = Vector3.one;
        }

        // 1) Tüm pozisyonları senkron hesapla
        for (int i = 0; i < total; i++)
        {
            int face = (e.diceValues != null && i < e.diceValues.Length) ? e.diceValues[i] : 1;
            Vector3 endPos = diceObjects[i].PreparePosition(face, landingTarget, occupiedEndPositions);
            occupiedEndPositions.Add(endPos);
        }

        lastOccupiedEndPositions = new List<Vector3>(occupiedEndPositions);

        // 2) Hepsini aynı anda başlat; hepsi bitince shrink
        int completed = 0;
        for (int i = 0; i < total; i++)
        {
            diceObjects[i].LaunchAnimation(onComplete: () =>
            {
                completed++;
                if (completed >= total)
                    StartCoroutine(ShrinkAndHideAll(e.totalValue));
            });
        }
    }

    private IEnumerator ShrinkAndHideAll(int totalValue)
    {
        int shrinkDone = 0;
        int total      = diceObjects.Count;

        foreach (var dice in diceObjects)
            StartCoroutine(ShrinkDice(dice, () => shrinkDone++));

        yield return new WaitUntil(() => shrinkDone >= total);

        foreach (var dice in diceObjects)
            dice.gameObject.SetActive(false);

        EventBus.Raise(new OnDiceHidden { totalValue = totalValue });
    }

    private IEnumerator ShrinkDice(Dice dice, Action onDone)
    {
        float elapsed = 0f;
        while (elapsed < shrinkDuration)
        {
            elapsed += Time.deltaTime;
            float scale = shrinkCurve.Evaluate(Mathf.Clamp01(elapsed / shrinkDuration));
            dice.transform.localScale = Vector3.one * scale;
            yield return null;
        }
        dice.transform.localScale = Vector3.zero;
        onDone?.Invoke();
    }

    private void SpawnDice(int count)
    {
        foreach (var dice in diceObjects)
            Destroy(dice.gameObject);

        diceObjects.Clear();

        for (int i = 0; i < count; i++)
        {
            float offset = (i - (count - 1) * 0.5f) * 1.5f;
            var position = diceParent.position + new Vector3(offset, 0f, 0f);
            var diceInstance = Instantiate(dicePrefab, position, Quaternion.identity, diceParent);
            diceInstance.transform.localScale = Vector3.zero;
            diceInstance.gameObject.SetActive(false);
            diceObjects.Add(diceInstance);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (lastOccupiedEndPositions == null) return;
        for (int i = 0; i < lastOccupiedEndPositions.Count; i++)
        {
            Gizmos.color = i == 0 ? Color.green : Color.yellow;
            Gizmos.DrawWireSphere(lastOccupiedEndPositions[i], 0.2f);
            UnityEditor.Handles.Label(lastOccupiedEndPositions[i] + Vector3.up * 0.3f, $"Die {i}");
        }
    }
#endif
}
