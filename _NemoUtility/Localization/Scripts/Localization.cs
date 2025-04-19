using System;
using System.Collections.Generic;
using UnityEngine;

namespace NemoUtility
{
    [CreateAssetMenu(fileName = "NLocalization", menuName = "NLocalization")]
    public class Localization : ScriptableObject
    {
        public List<Locale> Localizes = new List<Locale>();
        public Locale CurrentLocalize;

        public Action<Locale> LocalizeChangeEvent;

        public List<LocalizeString> LocalizeStrings = new List<LocalizeString>();
        public List<LocalizeSprite> LocalizeSprites = new List<LocalizeSprite>();

        public void ChangeLocalize(Locale localize)
        {
            CurrentLocalize = localize;
            LocalizeChangeEvent?.Invoke(localize);
        }

        public LocalizeString GetLocalizeString(string key)
        {
            var a = LocalizeStrings.Find(a => a.Key == key);
            return a;
        }
        public LocalizeSprite GetLocalizeSprite(string key)
        {
            var a = LocalizeSprites.Find(a => a.Key == key);
            return a;
        }

        public void EditLines()
        {
            //Strings
            foreach (var item in LocalizeStrings)
            {
                while (item.LocalizedStringTableSlots.Count != Localizes.Count)
                {
                    if (item.LocalizedStringTableSlots.Count > Localizes.Count)
                    {
                        item.LocalizedStringTableSlots.RemoveAt(item.LocalizedStringTableSlots.Count - 1);
                    }
                    else
                    {
                        item.LocalizedStringTableSlots.Add(new LocalizedStringTableSlot() { });
                    }
                }

                for (int i = 0; i < item.LocalizedStringTableSlots.Count; i++)
                {
                    item.LocalizedStringTableSlots[i].Localize = Localizes[i];
                }
            }

            //Sprites
            foreach (var item in LocalizeSprites)
            {
                while (item.LocalizedSpriteTableSlots.Count != Localizes.Count)
                {
                    if (item.LocalizedSpriteTableSlots.Count > Localizes.Count)
                    {
                        item.LocalizedSpriteTableSlots.RemoveAt(item.LocalizedSpriteTableSlots.Count - 1);
                    }
                    else
                    {
                        item.LocalizedSpriteTableSlots.Add(new LocalizedSpriteTableSlot() { });
                    }
                }

                for (int i = 0; i < item.LocalizedSpriteTableSlots.Count; i++)
                {
                    item.LocalizedSpriteTableSlots[i].Localize = Localizes[i];
                }
            }
        }
    }
}