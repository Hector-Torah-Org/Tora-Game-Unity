using System;
using TMPro;
using UnityEngine;

public sealed class I18nText : MonoBehaviour
{
    private TMP_Text target;
    private string sourceKey;
    private string fallback;
    private object[] formatArguments;

    private void Awake()
    {
        target = GetComponent<TMP_Text>();
        if (target != null && string.IsNullOrEmpty(sourceKey))
            sourceKey = target.text;
    }

    private void OnEnable()
    {
        I18n.LanguageChanged += Refresh;
        Refresh();
    }

    private void OnDisable()
    {
        I18n.LanguageChanged -= Refresh;
    }

    public static void SetText(TMP_Text target, string key)
    {
        SetText(target, key, key);
    }

    public static void SetText(TMP_Text target, string key, string fallback)
    {
        if (target == null)
        {
            Debug.LogError("Cannot localize a missing text component.");
            return;
        }

        I18nText localizedText = target.GetComponent<I18nText>();
        if (localizedText == null)
            localizedText = target.gameObject.AddComponent<I18nText>();

        localizedText.target = target;
        localizedText.sourceKey = key;
        localizedText.fallback = fallback;
        localizedText.formatArguments = null;
        localizedText.Refresh();
    }

    public static void SetFormattedText(TMP_Text target, string key, params object[] arguments)
    {
        if (target == null)
        {
            Debug.LogError("Cannot localize a missing text component.");
            return;
        }

        I18nText localizedText = target.GetComponent<I18nText>();
        if (localizedText == null)
            localizedText = target.gameObject.AddComponent<I18nText>();

        localizedText.target = target;
        localizedText.sourceKey = key;
        localizedText.fallback = key;
        localizedText.formatArguments = arguments;
        localizedText.Refresh();
    }

    public static void SetLiteralText(TMP_Text target, string value)
    {
        if (target == null)
        {
            Debug.LogError("Cannot update a missing text component.");
            return;
        }

        I18nText localizedText = target.GetComponent<I18nText>();
        if (localizedText != null)
        {
            localizedText.sourceKey = null;
            localizedText.fallback = null;
            localizedText.formatArguments = null;
        }

        target.text = value;
    }

    private void Refresh()
    {
        if (target != null && sourceKey != null)
        {
            string translated = I18n.Translate(sourceKey, fallback ?? sourceKey);
            target.text = formatArguments == null
                ? translated
                : string.Format(I18n.CurrentCulture, translated, formatArguments);
        }
    }
}
