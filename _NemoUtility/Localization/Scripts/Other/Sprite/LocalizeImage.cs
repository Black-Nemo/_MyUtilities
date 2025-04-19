using NemoUtility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LocalizeImage : MonoBehaviour
{
    [SerializeField] private string _key;

    private LocalizeSprite _localizeSprite;

    private Image _image;

    private Localization _localization;
    private void Awake()
    {
        _localization = LocalizationManager.Instance.GetLocalization();
        _localizeSprite = _localization.GetLocalizeSprite(_key);

        _image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        _localization.LocalizeChangeEvent += ChangeLocalize;
        ChangeLocalize(_localization.CurrentLocalize);
    }
    private void OnDisable()
    {
        LocalizationManager.Instance.GetLocalization().LocalizeChangeEvent -= ChangeLocalize;
    }

    public void ChangeLocalize(Locale localize)
    {
        _image.sprite = _localizeSprite.GetLocalizedSprite();
    }

}