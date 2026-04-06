using System.Linq;
using UnityEngine;

public class DiceController : MonoBehaviour, IInitializable
{
    [SerializeField] private GameConfigSO gameConfig;

    private DiceModel diceModel;

    public void Initialize()
    {
        diceModel = new DiceModel(gameConfig.defaultDiceCount);
    }

    public void SetDiceCount(int count)
    {
        int clamped = Mathf.Clamp(count, gameConfig.minDiceCount, gameConfig.maxDiceCount);
        diceModel.SetDiceCount(clamped);
        EventBus.Raise(new OnDiceCountChanged { count = clamped });
    }

    public void SetDiceValue(int diceIndex, int value)
    {
        diceModel.SetValue(diceIndex, value);
    }

    public void RollDice()
    {
        if (!diceModel.Validate()) return;

        EventBus.Raise(new OnDiceRolled
        {
            totalValue = diceModel.total,
            diceValues = diceModel.diceValues.ToArray()
        });
    }
}