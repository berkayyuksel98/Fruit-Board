using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ItemView : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private int amount;
    public void Setup(ItemDataSO itemData)
    {
        icon.sprite = itemData.icon;
        icon.color = itemData.color;
        amount = 0;
        UpdateAmount(amount);
    }

    public void UpdateAmount(int amount)
    {
        if(this.amount < amount){
            StartCoroutine(AnimateItemChange());
        }
        this.amount = amount;
        amountText.text = amount.ToString();
    }

    private IEnumerator AnimateItemChange()
    {
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale   = originalScale * 1.2f;
        float   halfDuration  = 0.15f;
        float   elapsed       = 0f;

        // Büyü
        while (elapsed < halfDuration)
        {
            transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / halfDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Küçül
        elapsed = 0f;
        while (elapsed < halfDuration)
        {
            transform.localScale = Vector3.Lerp(targetScale, originalScale, elapsed / halfDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale;
    }
}
