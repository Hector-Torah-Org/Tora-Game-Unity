using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class I18nRuntime : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void CreateRuntime()
    {
        I18n.Initialize();
        GameObject runtime = new GameObject("I18n");
        DontDestroyOnLoad(runtime);
        runtime.AddComponent<I18nRuntime>();
    }

    private void OnEnable()
    {
        I18n.LanguageChanged += LocalizeSceneTexts;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        I18n.LanguageChanged -= LocalizeSceneTexts;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LocalizeSceneTexts();
    }

    private static void LocalizeSceneTexts()
    {
        TMP_Text[] texts = Resources.FindObjectsOfTypeAll<TMP_Text>();
        foreach (TMP_Text text in texts)
        {
            if (text == null || !text.gameObject.scene.IsValid() || string.IsNullOrEmpty(text.text))
                continue;

            if (text.GetComponent<I18nText>() == null && I18n.HasTranslation(text.text))
                text.gameObject.AddComponent<I18nText>();
        }
    }
}
