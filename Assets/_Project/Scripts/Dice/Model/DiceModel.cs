using System.Collections.Generic;
using System.Linq;

public class DiceModel
{
    private List<int> diceValueList;

    public IReadOnlyList<int> diceValues => diceValueList;
    public int total => diceValueList.Sum();
    public int diceCount => diceValueList.Count;

    public DiceModel(int count = 2)
    {
        diceValueList = new List<int>(new int[count]);
    }

    public void SetDiceCount(int count)
    {
        diceValueList = new List<int>(new int[count]);
    }

    public void SetValue(int diceIndex, int value)
    {
        if (diceIndex < 0 || diceIndex >= diceValueList.Count) return;
        diceValueList[diceIndex] = value;
    }

    public bool Validate()
    {
        return diceValueList.All(v => v >= 1 && v <= 6);
    }
}