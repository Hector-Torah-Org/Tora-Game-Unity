using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

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

    private Action currentOnWin;
    private Action currentOnLose;

    private readonly List<Classification> localClassifications = new List<Classification>();
    private List<ImageResponseListDTO.ImageResponseDTO> currentImages = new List<ImageResponseListDTO.ImageResponseDTO>();
    private int currentImageIndex = 0;
    private bool roundRunning = false;

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

        if (overlayPanel == null || displayedImage == null || decoratedButton == null || notDecoratedButton == null)
        {
            Debug.LogError("UI reference missing");
            onLose?.Invoke();
            return;
        }

        currentOnWin = onWin;
        currentOnLose = onLose;

        roundRunning = true;
        localClassifications.Clear();
        currentImages.Clear();
        currentImageIndex = 0;

        ShowOverlay();
        ClearImageDisplay();
        SetButtonsInteractable(false);
        SetStatus("Loading images...");
        SetCounter("");
        SetCharacter("");

        StartCoroutine(BeginRoundCoroutine());
    }

    private IEnumerator BeginRoundCoroutine()
    {
        bool callbackReceived = false;
        ImageResponseListDTO response = null;

        yield return StartCoroutine(apiConnection.GetImage(imagesPerRound, result =>
        {
            response = result;
            callbackReceived = true;
        }));

        if (!callbackReceived || response == null || response.images == null || response.images.Count == 0)
        {
            Debug.LogError("TorahManager: No images received from API.");
            FailRound();
            yield break;
        }

        currentImages = response.images;
        currentImageIndex = 0;

        yield return StartCoroutine(ShowCurrentImageCoroutine());
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
        SetStatus("Loading image...");
        SetCounter($"Image {currentImageIndex + 1} / {currentImages.Count}");
        SetCharacter(currentImage.character);

        yield return StartCoroutine(LoadImageFromUrl(currentImage.link));

        if (!roundRunning)
            yield break;

        SetStatus("Choose a classification");
        SetButtonsInteractable(true);
    }

    private IEnumerator LoadImageFromUrl(string imageUrl)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("TorahManager: Failed to load image: " + request.error);
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

            displayedImage.sprite = sprite;
            displayedImage.preserveAspect = true;
            displayedImage.color = Color.white;
        }
    }

    public void OnDecoratedClicked()
    {
        SaveClassification(true);
    }

    public void OnNotDecoratedClicked()
    {
        SaveClassification(false);
    }

    private void SaveClassification(bool isDecorated)
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
            isDecorated = isDecorated
        });

        currentImageIndex++;
        StartCoroutine(ShowCurrentImageCoroutine());
    }

    private IEnumerator FinishRoundCoroutine()
    {
        SetStatus("Sending classifications...");
        SetButtonsInteractable(false);

        yield return StartCoroutine(apiConnection.SendClassifications(localClassifications));

        Debug.Log("TorahManager: Classifications sent.");

        localClassifications.Clear();
        currentImages.Clear();
        currentImageIndex = 0;
        roundRunning = false;

        HideOverlay();
        currentOnWin?.Invoke();
    }

    private void FailRound()
    {
        localClassifications.Clear();
        currentImages.Clear();
        currentImageIndex = 0;
        roundRunning = false;

        HideOverlay();
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
    }

    private void SetStatus(string message)
    {
        if (statusText != null)
            statusText.text = message;
    }

    private void SetCounter(string message)
    {
        if (counterText != null)
            counterText.text = message;
    }

    private void SetCharacter(string message)
    {
        if (characterText != null)
            characterText.text = string.IsNullOrEmpty(message) ? "" : message;
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