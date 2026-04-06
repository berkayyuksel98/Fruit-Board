using UnityEngine;

using UnityEngine;

public class PlayerPrefsPersistence : IDataPersistence
{
    private const string ApplesKey = "inventory_apples";
    private const string PearsKey = "inventory_pears";
    private const string StrawberriesKey = "inventory_strawberries";

    private readonly InventoryModel _inventoryModel;

    public PlayerPrefsPersistence(InventoryModel inventoryModel)
    {
        _inventoryModel = inventoryModel;
    }

    public void Save()
    {
        PlayerPrefs.SetInt(ApplesKey, _inventoryModel.GetAmount(ItemType.Apple));
        PlayerPrefs.SetInt(PearsKey, _inventoryModel.GetAmount(ItemType.Pear));
        PlayerPrefs.SetInt(StrawberriesKey, _inventoryModel.GetAmount(ItemType.Strawberry));
        PlayerPrefs.Save();
    }

    public void Load()
    {
        _inventoryModel.SetAmount(ItemType.Apple, PlayerPrefs.GetInt(ApplesKey, 0));
        _inventoryModel.SetAmount(ItemType.Pear, PlayerPrefs.GetInt(PearsKey, 0));
        _inventoryModel.SetAmount(ItemType.Strawberry, PlayerPrefs.GetInt(StrawberriesKey, 0));
    }
}
