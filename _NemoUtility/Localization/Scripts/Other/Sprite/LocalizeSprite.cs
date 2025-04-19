using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NemoUtility
{
    [System.Serializable]
    public class LocalizeSprite : LocalizedData
    {
        public List<LocalizedSpriteTableSlot> LocalizedSpriteTableSlots = new List<LocalizedSpriteTableSlot>();


        public Sprite GetLocalizedSprite()
        {
            var l = LocalizedSpriteTableSlots.FirstOrDefault(a => a.Localize.Code == LocalizationManager.Instance.GetLocalization().CurrentLocalize.Code);
            return l.LocalizeSprite;
        }
    }
}