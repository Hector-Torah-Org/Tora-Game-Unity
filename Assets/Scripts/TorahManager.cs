using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.U2D;
using UnityEngine.UI;

public class TorahManager : MonoBehaviour
{
    public static TorahManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private ApiConnection apiConnection;

    [Header("Round Settings")]
    [SerializeField] private int imagesPerRound = 3;

    [Header("UI")]
    [SerializeField] private GameObject overlayPanel;
    [SerializeField] private Image displayedImage;
    [SerializeField] private TextMeshProUGUI counterText;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI characterText;
    [SerializeField] private Button decoratedButton;
    [SerializeField] private Button notDecoratedButton;
    [SerializeField] private Button datasetErrorButton;

    private Action currentOnWin;
    private Action currentOnLose;

    private readonly List<Classification> localClassifications = new List<Classification>();
    private List<ImageResponseListDTO.ImageResponseDTO> currentImages = new List<ImageResponseListDTO.ImageResponseDTO>();
    private int currentImageIndex = 0;
    private List<Sprite> imageSprites = new List<Sprite>();
    private bool roundRunning = false;

    private bool isTutorialRound = false;
    private tutorialRoundAnswerDTO tutorialRoundResult;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (apiConnection == null)
            apiConnection = FindFirstObjectByType<ApiConnection>();

        if (decoratedButton != null)
            decoratedButton.onClick.AddListener(OnDecoratedClicked);

        if (notDecoratedButton != null)
            notDecoratedButton.onClick.AddListener(OnNotDecoratedClicked);

        if (datasetErrorButton != null)
            datasetErrorButton.onClick.AddListener(OnDatasetErrorClicked);

        HideOverlay();
    }

    public void StartRound(Action onWin, Action onLose)
    {
        if (roundRunning)
        {
            Debug.LogWarning("Torah running already");
            return;
        }

        if (apiConnection == null)
        {
            Debug.LogError("API connection 404");
            onLose?.Invoke();
            return;
        }

        if (string.IsNullOrEmpty(apiConnection.sessionId))
        {
            Debug.LogError("sessionID expired");
            onLose?.Invoke();
            return;
        }

        if (overlayPanel == null || displayedImage == null || decoratedButton == null || notDecoratedButton == null || datasetErrorButton == null)
        {
            Debug.LogError("UI reference missing");
            onLose?.Invoke();
            return;
        }

        currentOnWin = onWin;
        currentOnLose = onLose;

        roundRunning = true;
        SceneManager.Instance?.AddUIBlockingWorldInput(SceneManager.UIBlockLevel.ToraGame);
        localClassifications.Clear();
        currentImages.Clear();
        currentImageIndex = 0;

        ShowOverlay();
        ClearImageDisplay();
        SetButtonsInteractable(false);
        SetStatus("round.loading_images");
        SetCounter("");
        SetCharacter("");

        StartCoroutine(BeginRoundCoroutine());
    }

    public IEnumerator StartTutorialRound(Action<tutorialRoundAnswerDTO> onResult)
    {
        isTutorialRound = true;
        StartRound(() => { Debug.Log($"Tutorial round result: {tutorialRoundResult.correctAnswerRate} {tutorialRoundResult.passedTutorial}"); onResult.Invoke(tutorialRoundResult); }, () => { StartCoroutine(StartTutorialRound(onResult)); });
        yield return new WaitForSeconds(0.1f);
    }

    private IEnumerator BeginRoundCoroutine()
    {
        bool callbackReceived = false;
        ImageResponseListDTO response = null;

        if (!isTutorialRound)
        {
            yield return StartCoroutine(apiConnection.GetImage(imagesPerRound, result =>
            {
                response = result;
                callbackReceived = true;
            }));
        }

        if (isTutorialRound)
        {
            yield return StartCoroutine(apiConnection.GetTestImage(imagesPerRound, result =>
            {
                response = result;
                callbackReceived = true;
            }));
        }

        if (!callbackReceived || response == null || response.images == null || response.images.Count == 0)
        {
            Debug.LogError("TorahManager: No images received from API.");
            FailRound();
            yield break;
        }

        currentImages = response.images;
        currentImageIndex = 0;
        imageSprites.Clear();
        for (int i = 0; i < currentImages.Count; i++)
        {
            imageSprites.Add(null);
        }
        GetImageSprites(currentImages);
        yield return StartCoroutine(ShowCurrentImageCoroutine());
    }

    private void GetImageSprites(List<ImageResponseListDTO.ImageResponseDTO> images)
    {
        for (int i = 0;i < images.Count;i++)
        {
            StartCoroutine(LoadImageFromUrl(images[i].link, i));
        }
    }

    private IEnumerator ShowCurrentImageCoroutine()
    {
        if (currentImageIndex >= currentImages.Count)
        {
            yield return StartCoroutine(FinishRoundCoroutine());
            yield break;
        }

        var currentImage = currentImages[currentImageIndex];

        SetButtonsInteractable(false);
        SetStatus("round.loading_image");
        I18nText.SetFormattedText(counterText, "round.image_counter", currentImageIndex + 1, currentImages.Count);
        SetCharacter(currentImage.character);

        yield return new WaitUntil(() => imageSprites[currentImageIndex] != null);
        displayedImage.sprite = imageSprites[currentImageIndex];
        displayedImage.preserveAspect = true;
        displayedImage.color = Color.white;

        if (!roundRunning)
        {
            yield break;
        }    

        SetStatus("round.choose_classification");
        SetButtonsInteractable(true);
    }

    private IEnumerator LoadImageFromUrl(string imageUrl, int index)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("TorahManager: Failed to load image " + imageUrl + ": " + request.error);
                FailRound();
                yield break;
            }

            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            if (texture == null)
            {
                Debug.LogError("TorahManager: Downloaded texture is null.");
                FailRound();
                yield break;
            }

            Sprite sprite = Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f)
            );
            imageSprites[index] = sprite;
        }
    }

    public void OnDecoratedClicked()
    {
        SaveClassification(true, false);
    }

    public void OnNotDecoratedClicked()
    {
        SaveClassification(false, false);
    }

    public void OnDatasetErrorClicked()
    {
        SaveClassification(false, true); //first parameter doesn't matter for the server when dataset error is true, but a nullable doesn't work with json utility, so we set it to false
    }

    private void SaveClassification(bool isDecorated, bool isDatasetError)
    {
        if (!roundRunning)
            return;

        if (currentImageIndex < 0 || currentImageIndex >= currentImages.Count)
            return;

        SetButtonsInteractable(false);

        var currentImage = currentImages[currentImageIndex];

        localClassifications.Add(new Classification
        {
            imageId = currentImage.id,
            isDecorated = isDecorated,
            isDatasetError = isDatasetError
        });
        Debug.Log($"Classification saved: {localClassifications[localClassifications.Count - 1]}.isDecorated={localClassifications[localClassifications.Count - 1].isDecorated}, isDatasetError={localClassifications[localClassifications.Count - 1].isDatasetError}");

        currentImageIndex++;
        StartCoroutine(ShowCurrentImageCoroutine());
    }

    private IEnumerator FinishRoundCoroutine()
    {
        SetStatus("round.sending_classifications");
        SetButtonsInteractable(false);
        foreach(var classification in localClassifications)
        {
            Debug.Log($"Classification: ImageID={classification.imageId}, IsDecorated={classification.isDecorated}, IsDatasetError={classification.isDatasetError}");
        };

        if (!isTutorialRound)
        {
            yield return StartCoroutine(apiConnection.SendClassifications(localClassifications));
        }
        else
        {
            yield return StartCoroutine(apiConnection.SendTestClassifications(localClassifications, (onSuccess) =>
            {
                tutorialRoundResult = onSuccess;
            }));
        }
        Debug.Log("TorahManager: Classifications sent.");

        localClassifications.Clear();
        currentImages.Clear();
        currentImageIndex = 0;
        isTutorialRound = false;
        roundRunning = false;

        HideOverlay();
        if (SceneManager.Instance.uiLevelsBlockingWorldInput.Contains(SceneManager.UIBlockLevel.ToraGame))
            SceneManager.Instance?.RemoveUIBlockingWorldInput(SceneManager.UIBlockLevel.ToraGame);
        currentOnWin?.Invoke();
    }

    private void FailRound()
    {
        localClassifications.Clear();
        currentImages.Clear();
        currentImageIndex = 0;
        roundRunning = false;

        HideOverlay();
        if (SceneManager.Instance.uiLevelsBlockingWorldInput.Contains(SceneManager.UIBlockLevel.ToraGame))
            SceneManager.Instance?.RemoveUIBlockingWorldInput(SceneManager.UIBlockLevel.ToraGame);
        currentOnLose?.Invoke();
    }

    private void ShowOverlay()
    {
        if (overlayPanel != null)
            overlayPanel.SetActive(true);
    }

    private void HideOverlay()
    {
        if (overlayPanel != null)
            overlayPanel.SetActive(false);
    }

    private void SetButtonsInteractable(bool interactable)
    {
        if (decoratedButton != null)
            decoratedButton.interactable = interactable;

        if (notDecoratedButton != null)
            notDecoratedButton.interactable = interactable;

        if (datasetErrorButton != null)
            datasetErrorButton.interactable = interactable;
    }

    private void SetStatus(string message)
    {
        if (statusText != null)
            I18nText.SetText(statusText, message);
    }

    private void SetCounter(string message)
    {
        if (counterText != null)
            I18nText.SetLiteralText(counterText, message);
    }

    private void SetCharacter(string message)
    {
        if (characterText != null)
            I18nText.SetLiteralText(characterText, string.IsNullOrEmpty(message) ? "" : message);
    }

    private void ClearImageDisplay()
    {
        if (displayedImage != null)
        {
            displayedImage.sprite = null;
            displayedImage.color = new Color(1f, 1f, 1f, 0f);
        }
    }
}