using NemoUtility;
using TMPro;
using UnityEngine;

namespace NemoUtility
{
    public class LocalizeTMP : MonoBehaviour
    {
        [SerializeField] private string _key;

        private LocalizeString _localizeString;

        private TextMeshProUGUI _textMeshProUGUI;

        private Localization _localization;
        private void Awake()
        {
            _localization = LocalizationManager.Instance.GetLocalization();
            _localizeString = _localization.GetLocalizeString(_key);

            _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            _localization.LocalizeChangeEvent += ChangeLocalize;
            ChangeLocalize(_localization.CurrentLocalize);
        }
        private void OnDisable()
        {
            _localization.LocalizeChangeEvent -= ChangeLocalize;
        }

        public void ChangeLocalize(Locale localize)
        {
            _textMeshProUGUI.text = _localizeString.GetLocalizedString();
        }

    }
}