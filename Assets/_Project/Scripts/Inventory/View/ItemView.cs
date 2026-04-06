using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemView : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI amountText;

    public void Setup(ItemDataSO itemData)
    {
        icon.sprite = itemData.icon;
        icon.color = itemData.color;
        UpdateAmount(0);
    }

    public void UpdateAmount(int amount)
    {
        amountText.text = amount.ToString();
    }
}
