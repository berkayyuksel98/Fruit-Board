public enum ItemType
{
    None,
    Apple,
    Pear,
    Strawberry
}

public class TileRewardModel
{
    public ItemType itemType { get; }
    public int amount { get; }

    public bool isEmpty => itemType == ItemType.None || amount <= 0;

    public TileRewardModel(ItemType itemType, int amount)
    {
        this.itemType = itemType;
        this.amount = amount;
    }
}
