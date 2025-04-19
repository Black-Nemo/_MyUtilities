using System;
using System.Collections.Generic;
using System.Linq;

namespace NemoUtility
{
    [System.Serializable]
    public class LocalizeString : LocalizedData
    {
        public List<LocalizedStringTableSlot> LocalizedStringTableSlots = new List<LocalizedStringTableSlot>();

        public string GetLocalizedString()
        {
            var l = LocalizedStringTableSlots.FirstOrDefault(a => a.Localize.Code == LocalizationManager.Instance.GetLocalization().CurrentLocalize.Code);
            return (l == null) ? "!Not Found" : l.LocalizeString.Replace("\r", "").Replace("\u200b", "").Replace("\uFEFF", "");;
        }
    }
}