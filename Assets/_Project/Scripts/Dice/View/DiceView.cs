using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DiceView : MonoBehaviour, IInitializable
{
    [SerializeField] private GameConfigSO gameConfig;
    [SerializeField] private Transform diceParent;
    [SerializeField] private GameObject dicePrefab;

    private readonly List<GameObject> diceObjects = new();
    private readonly List<TextMeshPro> diceTexts = new();

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

    private void HandleDiceRolled(OnDiceRolled e) => StartCoroutine(PlayRollAnimation(e.diceValues));

    private void SpawnDice(int count)
    {
        foreach (var dice in diceObjects)
            Destroy(dice);

        diceObjects.Clear();
        diceTexts.Clear();

        for (int i = 0; i < count; i++)
        {
            float offset = (i - (count - 1) * 0.5f) * 1.5f;
            var position = diceParent.position + new Vector3(offset, 0f, 0f);
            var go = Instantiate(dicePrefab, position, Quaternion.identity, diceParent);

            var text = go.GetComponentInChildren<TextMeshPro>();
            diceTexts.Add(text);
            diceObjects.Add(go);
        }
    }

    private IEnumerator PlayRollAnimation(int[] finalValues)
    {
        float elapsed = 0f;
        float duration = gameConfig.diceAnimationDuration;

        while (elapsed < duration)
        {
            for (int i = 0; i < diceObjects.Count; i++)
            {
                diceObjects[i].transform.Rotate(Random.insideUnitSphere * 720f * Time.deltaTime);

                if (i < diceTexts.Count && diceTexts[i] != null)
                    diceTexts[i].text = Random.Range(1, 7).ToString();
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i < diceObjects.Count; i++)
        {
            diceObjects[i].transform.rotation = Quaternion.identity;

            if (i < finalValues.Length && i < diceTexts.Count && diceTexts[i] != null)
                diceTexts[i].text = finalValues[i].ToString();
        }
    }
}