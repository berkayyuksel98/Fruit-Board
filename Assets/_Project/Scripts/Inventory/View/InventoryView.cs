using System.Collections.Generic;
using UnityEngine;

public class InventoryView : MonoBehaviour
{
    [SerializeField] private GameConfigSO gameConfig;
    [SerializeField] private ItemView appleView;
    [SerializeField] private ItemView pearView;
    [SerializeField] private ItemView strawberryView;

    private InventoryController inventoryController;
    private readonly Dictionary<ItemType, ItemView> itemViews = new();

    public void Initialize(InventoryController controller)
    {
        inventoryController = controller;

        SetupItemViews();

        EventBus.Subscribe<OnInventoryChanged>(HandleInventoryChanged);

        RefreshAll();
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<OnInventoryChanged>(HandleInventoryChanged);
    }

    private void SetupItemViews()
    {
        foreach (var itemData in gameConfig.items)
        {
            ItemView view = itemData.itemType switch
            {
                ItemType.Apple      => appleView,
                ItemType.Pear       => pearView,
                ItemType.Strawberry => strawberryView,
                _                   => null
            };

            if (view == null) continue;

            view.Setup(itemData);
            itemViews[itemData.itemType] = view;
        }
    }

    private void HandleInventoryChanged(OnInventoryChanged e) => RefreshAll();

    private void RefreshAll()
    {
        var model = inventoryController.GetInventoryModel();

        foreach (var kvp in itemViews)
            kvp.Value.UpdateAmount(model.GetAmount(kvp.Key));
    }
}