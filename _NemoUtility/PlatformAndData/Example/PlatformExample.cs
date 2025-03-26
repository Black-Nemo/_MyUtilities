using NemoUtility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlatformExample : MonoBehaviour
{
    //String
    //Get
    [SerializeField] private TMP_InputField GetStringIdInputField;
    [SerializeField] private Button GetStringButton;
    [SerializeField] private TextMeshProUGUI GetStringText;

    //Set
    [SerializeField] private Button SetStringButton;
    [SerializeField] private TMP_InputField SetStringIdInputField;
    [SerializeField] private TMP_InputField SetStringValueInputField;


    private void Awake()
    {
        GetStringButton.onClick.AddListener(() =>
        {
            GetStringText.text = DataManager.Instance.GetString(GetStringIdInputField.text);
        });
        SetStringButton.onClick.AddListener(() =>
        {
            DataManager.Instance.SetString(SetStringIdInputField.text, SetStringValueInputField.text);
        });
    }
}
