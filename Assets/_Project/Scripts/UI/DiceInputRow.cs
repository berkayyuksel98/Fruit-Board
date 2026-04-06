using UnityEngine;
using TMPro;

public class DiceInputRow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private TMP_InputField inputField;

    private void Awake()
    {
        if (inputField == null) return;

        inputField.contentType    = TMP_InputField.ContentType.IntegerNumber;
        inputField.characterLimit = 2;
        inputField.text           = "1";
    }

    public void Setup(string labelText)
    {
        if (label      != null) label.text      = labelText;
        if (inputField != null) inputField.text = "1";
    }

    public string GetValue() => (inputField != null && !string.IsNullOrWhiteSpace(inputField.text))
        ? inputField.text
        : "1";

    public void SetInteractable(bool interactable)
    {
        if (inputField != null) inputField.interactable = interactable;
    }
}
