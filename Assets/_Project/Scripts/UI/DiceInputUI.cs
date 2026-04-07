using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiceInputUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameConfigSO gameConfig;
    [SerializeField] private TMP_Dropdown diceCountDropdown;
    [SerializeField] private Transform diceInputsContainer;
    [SerializeField] private GameObject diceInputRowPrefab;
    [SerializeField] private Button rollButton;

    private DiceController diceController;
    private readonly List<DiceInputRow> inputRows = new();

    public void Initialize(DiceController controller)
    {
        diceController = controller;

        SetupDropdown();
        RebuildDiceInputs(gameConfig.defaultDiceCount);

        rollButton.onClick.AddListener(OnRollClicked);
        diceCountDropdown.onValueChanged.AddListener(OnDiceCountChanged);

        EventBus.Subscribe<OnGameInputLocked>(HandleInputLocked);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<OnGameInputLocked>(HandleInputLocked);
        rollButton.onClick.RemoveListener(OnRollClicked);
        diceCountDropdown.onValueChanged.RemoveListener(OnDiceCountChanged);
    }

    private void SetupDropdown()
    {
        diceCountDropdown.options.Clear();

        for (int i = gameConfig.minDiceCount; i <= gameConfig.maxDiceCount; i++)
            diceCountDropdown.options.Add(new TMP_Dropdown.OptionData(i.ToString()));

        diceCountDropdown.value = gameConfig.defaultDiceCount - gameConfig.minDiceCount;
        diceCountDropdown.RefreshShownValue();
    }

    private void OnDiceCountChanged(int dropdownIndex)
    {
        int count = dropdownIndex + gameConfig.minDiceCount;
        diceController.SetDiceCount(count);
        RebuildDiceInputs(count);
    }

    private void RebuildDiceInputs(int count)
    {
        foreach (Transform child in diceInputsContainer)
            Destroy(child.gameObject);

        inputRows.Clear();

        for (int i = 0; i < count; i++)
        {
            var rowGO = Instantiate(diceInputRowPrefab, diceInputsContainer);
            var row = rowGO.GetComponent<DiceInputRow>();
            row.Setup($"Dice {i + 1}:");
            inputRows.Add(row);
        }
    }

    private void OnRollClicked()
    {
        for (int i = 0; i < inputRows.Count; i++)
        {
            if (int.TryParse(inputRows[i].GetValue(), out int value)){
                value = Mathf.Clamp(value,1,6);
                diceController.SetDiceValue(i, value);
            }
        }

        SetInputLocked(true);
        diceController.RollDice();
    }

    private void HandleInputLocked(OnGameInputLocked e) => SetInputLocked(e.isLocked);

    private void SetInputLocked(bool locked)
    {
        rollButton.interactable = !locked;
        diceCountDropdown.interactable = !locked;

        foreach (var row in inputRows)
            row.SetInteractable(!locked);
    }
}
