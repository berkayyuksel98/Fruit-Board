using UnityEngine;
using TMPro;

public class DiceInputRow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private TMP_InputField inputField;

    private void Awake()
    {
        if (inputField == null) return;

        inputField.contentType = TMP_InputField.ContentType.IntegerNumber;
        inputField.characterLimit = 1;
        inputField.text = "1";
        inputField.onValueChanged.AddListener(OnInputValueChanged);
    }

    private void OnInputValueChanged(string value)
    {
        if (int.TryParse(inputField.text, out int parsedValue))
        {
            parsedValue = Mathf.Clamp(parsedValue, 1, 6);
            inputField.text = parsedValue.ToString();
        }
    }

    public void Setup(string labelText)
    {
        if (label != null) label.text = labelText;
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
