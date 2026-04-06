using System.Collections.Generic;

public class InventoryModel
{
    private readonly Dictionary<ItemType, ItemModel> itemMap;
    public IReadOnlyDictionary<ItemType, ItemModel> items => itemMap;

    public InventoryModel()
    {
        itemMap = new Dictionary<ItemType, ItemModel>
        {
            { ItemType.Apple,      new ItemModel(ItemType.Apple,      0) },
            { ItemType.Pear,       new ItemModel(ItemType.Pear,       0) },
            { ItemType.Strawberry, new ItemModel(ItemType.Strawberry, 0) }
        };
    }

    public void AddItem(ItemType itemType, int amount)
    {
        if (itemMap.TryGetValue(itemType, out var item))
            item.Add(amount);
    }

    public int GetAmount(ItemType itemType)
    {
        return itemMap.TryGetValue(itemType, out var item) ? item.amount : 0;
    }

    public void SetAmount(ItemType itemType, int amount)
    {
        if (itemMap.TryGetValue(itemType, out var item))
            item.SetAmount(amount);
    }
}

