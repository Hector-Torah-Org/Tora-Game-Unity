using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class I18n
{
    public enum Language
    {
        English,
        German
    }

    private const string LanguagePreferenceKey = "language";
    private static bool initialized;
    private static Language currentLanguage;
    private static string languageCode;

    public static event Action LanguageChanged;

    private static Dictionary<string, string> translations = new Dictionary<string, string>();

    public static Language CurrentLanguage
    {
        get
        {
            EnsureInitialized();
            return currentLanguage;
        }
    }

    public static string LanguageCode
    {
        get
        {
            EnsureInitialized();
            return languageCode;
        }
    }

    public static CultureInfo CurrentCulture
    {
        get { return CultureInfo.GetCultureInfo(LanguageCode); }
    }

    public static void Initialize()
    {
        if (initialized)
            return;

        string savedLanguage = PlayerPrefs.GetString(LanguagePreferenceKey, "");
        string defaultLanguage = Application.systemLanguage == SystemLanguage.German ? "de" : "en";
        string initialLanguage = string.IsNullOrEmpty(savedLanguage) ? defaultLanguage : savedLanguage;

        initialized = true;
        if (!TrySetLanguage(initialLanguage, false) &&
            !string.Equals(initialLanguage, "en", StringComparison.OrdinalIgnoreCase))
        {
            TrySetLanguage("en", false);
        }
    }

    public static void SetLanguage(Language language)
    {
        SetLanguage(language == Language.German ? "de" : "en");
    }

    public static void SetLanguage(string code)
    {
        EnsureInitialized();
        if (TrySetLanguage(code, true))
        {
            PlayerPrefs.SetString(LanguagePreferenceKey, languageCode);
            PlayerPrefs.Save();
        }
    }

    private static bool TrySetLanguage(string code, bool notify)
    {
        if (string.IsNullOrWhiteSpace(code) || code.IndexOfAny(new[] { '/', '\\', '.' }) >= 0)
        {
            Debug.LogError("Invalid I18n language code: " + code);
            return false;
        }

        string normalizedCode = code.Trim().ToLowerInvariant();
        TextAsset catalog = Resources.Load<TextAsset>("I18n/" + normalizedCode);
        if (catalog == null)
        {
            Debug.LogError("I18n catalog not found: Assets/Resources/I18n/" + normalizedCode + ".txt");
            return false;
        }

        Dictionary<string, string> loadedTranslations;
        if (!TryParseCatalog(catalog, out loadedTranslations))
            return false;

        if (languageCode == normalizedCode)
        {
            translations = loadedTranslations;
            return true;
        }

        translations = loadedTranslations;
        languageCode = normalizedCode;
        currentLanguage = normalizedCode == "de" ? Language.German : Language.English;

        if (notify)
            LanguageChanged?.Invoke();

        return true;
    }

    public static string Translate(string key)
    {
        return Translate(key, key);
    }

    public static string Translate(string key, string fallback)
    {
        EnsureInitialized();
        string translation;
        return translations.TryGetValue(key, out translation) ? translation : fallback;
    }

    public static string Format(string key, params object[] arguments)
    {
        return string.Format(CurrentCulture, Translate(key), arguments);
    }

    internal static bool HasTranslation(string key)
    {
        EnsureInitialized();
        return translations.ContainsKey(key);
    }

    private static bool TryParseCatalog(TextAsset catalog, out Dictionary<string, string> parsed)
    {
        parsed = new Dictionary<string, string>();
        string[] lines = catalog.text.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);
        bool valid = true;

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            if (string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith("#", StringComparison.Ordinal))
                continue;

            int separator = FindSeparator(line);
            string key;
            string value;
            if (separator < 0 ||
                !TryUnescape(line.Substring(0, separator), out key) ||
                !TryUnescape(line.Substring(separator + 1), out value))
            {
                Debug.LogError("Invalid I18n catalog entry at " + catalog.name + ".txt:" + (i + 1));
                valid = false;
                continue;
            }

            if (parsed.ContainsKey(key))
            {
                Debug.LogError("Duplicate I18n key '" + key + "' at " + catalog.name + ".txt:" + (i + 1));
                valid = false;
                continue;
            }

            parsed.Add(key, value);
        }

        return valid;
    }

    private static int FindSeparator(string line)
    {
        bool escaped = false;
        for (int i = 0; i < line.Length; i++)
        {
            if (line[i] == '=' && !escaped)
                return i;

            if (line[i] == '\\')
                escaped = !escaped;
            else
                escaped = false;
        }

        return -1;
    }

    private static bool TryUnescape(string value, out string unescaped)
    {
        System.Text.StringBuilder result = new System.Text.StringBuilder(value.Length);
        for (int i = 0; i < value.Length; i++)
        {
            if (value[i] != '\\')
            {
                result.Append(value[i]);
                continue;
            }

            if (++i >= value.Length)
            {
                unescaped = null;
                return false;
            }

            switch (value[i])
            {
                case '\\': result.Append('\\'); break;
                case 'n': result.Append('\n'); break;
                case 'r': result.Append('\r'); break;
                case 't': result.Append('\t'); break;
                case '=': result.Append('='); break;
                default:
                    unescaped = null;
                    return false;
            }
        }

        unescaped = result.ToString();
        return true;
    }

    private static void EnsureInitialized()
    {
        if (!initialized)
            Initialize();
    }
}
