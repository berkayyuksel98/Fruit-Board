using UnityEngine;

public class InventoryController : MonoBehaviour
{
    private InventoryModel inventoryModel;
    private IDataPersistence persistence;

    public void Initialize(InventoryModel model, IDataPersistence dataPersistence)
    {
        inventoryModel = model;
        persistence = dataPersistence;

        persistence.Load();

        EventBus.Subscribe<OnTileRewardCollected>(HandleRewardCollected);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<OnTileRewardCollected>(HandleRewardCollected);
        persistence.Save();
    }

    private void HandleRewardCollected(OnTileRewardCollected e)
    {
        if (e.reward.isEmpty) return;

        inventoryModel.AddItem(e.reward.itemType, e.reward.amount);

        EventBus.Raise(new OnInventoryChanged());
    }

    public InventoryModel GetInventoryModel() => inventoryModel;
}