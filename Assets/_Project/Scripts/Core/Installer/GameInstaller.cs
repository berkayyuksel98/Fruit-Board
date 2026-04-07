using UnityEngine;

public class GameInstaller : MonoBehaviour
{
    [Header("Board")]
    [SerializeField] private BoardController boardController;
    [SerializeField] private BoardView boardView;

    [Header("Dice")]
    [SerializeField] private DiceController diceController;
    [SerializeField] private DiceView diceView;

    [Header("Inventory")]
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private InventoryView inventoryView;

    [Header("Player")]
    [SerializeField] private PlayerController playerController;

    [Header("UI")]
    [SerializeField] private DiceInputUI diceInputUI;

    [Header("Effects")]
    [SerializeField] private AudioController audioController;

    [Header("Config")]
    [SerializeField] private GameConfigSO gameConfig;

    private void Awake()
    {
        var inventoryModel = new InventoryModel();
        var persistence = new PlayerPrefsPersistence(inventoryModel);

        diceController.Initialize();
        diceView.Initialize();

        boardView.Initialize();

        inventoryController.Initialize(inventoryModel, persistence);
        inventoryView.Initialize(inventoryController);

        playerController.Initialize();
        diceInputUI.Initialize(diceController);
        
        audioController.Initialize();
        boardController.Initialize();
    }

    private void OnDestroy()
    {
        EventBus.Clear();
    }
}

