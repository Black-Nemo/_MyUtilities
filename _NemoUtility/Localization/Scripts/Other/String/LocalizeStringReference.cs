using UnityEngine;

namespace NemoUtility
{
    [System.Serializable]
    public class LocalizeStringReference
    {
        [SerializeField] private string _key;

        private LocalizeString _localizeString;
        public LocalizeString LocalizeString
        {
            get
            {
                if (_localizeString != null && _localizeString.Key == "") { _localizeString = null; }
                if (_localizeString == null) { _localizeString = LocalizationManager.Instance.GetLocalization().GetLocalizeString(_key); }
                return _localizeString;
            }
        }

        public string GetLocalizedString()
        {
            return LocalizeString.GetLocalizedString();
        }
    }

}