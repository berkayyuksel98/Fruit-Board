public class ItemModel
{
    public ItemType itemType { get; }
    public int amount { get; private set; }

    public ItemModel(ItemType itemType, int amount)
    {
        this.itemType = itemType;
        this.amount = amount;
    }

    public void Add(int value)
    {
        amount += value;
    }

    public void SetAmount(int value)
    {
        amount = value;
    }
}
