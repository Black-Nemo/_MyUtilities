using System.Collections.Generic;
using UnityEngine;

public static class LanguageUtility
{
    // Unity'nin desteklediği temel dillerin 2 haneli (ISO 639-1) kod karşılıkları
    private static readonly Dictionary<SystemLanguage, string> LanguageToIsoMap = new Dictionary<SystemLanguage, string>()
    {
        { SystemLanguage.Afrikaans, "af" },
        { SystemLanguage.Arabic, "ar" },
        { SystemLanguage.Basque, "eu" },
        { SystemLanguage.Belarusian, "be" },
        { SystemLanguage.Bulgarian, "bg" },
        { SystemLanguage.Catalan, "ca" },
        { SystemLanguage.Chinese, "zh" },
        { SystemLanguage.ChineseSimplified, "zh" },
        { SystemLanguage.ChineseTraditional, "zh-TW" },
        { SystemLanguage.Czech, "cs" },
        { SystemLanguage.Danish, "da" },
        { SystemLanguage.Dutch, "nl" },
        { SystemLanguage.English, "en" },
        { SystemLanguage.Estonian, "et" },
        { SystemLanguage.Faroese, "fo" },
        { SystemLanguage.Finnish, "fi" },
        { SystemLanguage.French, "fr" },
        { SystemLanguage.German, "de" },
        { SystemLanguage.Greek, "el" },
        { SystemLanguage.Hebrew, "he" },
        { SystemLanguage.Hungarian, "hu" },
        { SystemLanguage.Icelandic, "is" },
        { SystemLanguage.Indonesian, "id" },
        { SystemLanguage.Italian, "it" },
        { SystemLanguage.Japanese, "ja" },
        { SystemLanguage.Korean, "ko" },
        { SystemLanguage.Latvian, "lv" },
        { SystemLanguage.Lithuanian, "lt" },
        { SystemLanguage.Norwegian, "no" },
        { SystemLanguage.Polish, "pl" },
        // Mobil oyunlarda Portekizce genellikle Brezilya (pt-BR) olarak hedeflenir
        { SystemLanguage.Portuguese, "pt-BR" }, 
        { SystemLanguage.Romanian, "ro" },
        { SystemLanguage.Russian, "ru" },
        { SystemLanguage.Slovak, "sk" },
        { SystemLanguage.Slovenian, "sl" },
        { SystemLanguage.Spanish, "es" },
        { SystemLanguage.Swedish, "sv" },
        { SystemLanguage.Thai, "th" },
        { SystemLanguage.Turkish, "tr" },
        { SystemLanguage.Ukrainian, "uk" },
        { SystemLanguage.Vietnamese, "vi" }
    };

    /// <summary>
    /// Mevcut cihazın sistem dilini 2 haneli kod (örn: "en", "tr", "ru") olarak döndürür.
    /// Eğer dil listede yoksa, belirlediğin varsayılan dili döndürür.
    /// </summary>
    /// <param name="fallbackCode">Dil bulunamazsa dönülecek varsayılan kod (Varsayılan: "en")</param>
    /// <returns>2 haneli dil kodu</returns>
    public static string GetSystemLanguageCode(string fallbackCode = "en")
    {
        SystemLanguage sysLang = Application.systemLanguage;
        
        if (LanguageToIsoMap.TryGetValue(sysLang, out string isoCode))
        {
            return isoCode;
        }

        Debug.LogWarning($"[LanguageUtility] Sistem dili ({sysLang}) haritada bulunamadı. '{fallbackCode}' varsayılanı kullanılıyor.");
        return fallbackCode;
    }
}