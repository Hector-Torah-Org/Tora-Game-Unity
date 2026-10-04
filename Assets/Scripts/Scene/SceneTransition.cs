using System.Collections;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance { get; private set; }

    [Header("References")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private CanvasGroup fadeCanvasGroup;

    [Header("Zoom")]
    [SerializeField] private float zoomDuration = 0.65f;
    [SerializeField] private float zoomAmount = 1.8f;
    [SerializeField] private float moveTowardTargetAmount = 0.35f;
    [Range(0f, 1f)]
    [SerializeField] private float fadeStartPoint = 0.55f;

    [Header("Fade")]
    [SerializeField] private float fadeToBlackDuration = 0.25f;
    [SerializeField] private float fadeFromBlackDuration = 0.18f;

    private bool isTransitioning;

    private Vector3 originalCameraPosition;
    private float originalOrthographicSize;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (mainCamera == null)
            mainCamera = Camera.main;

        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0f;
            fadeCanvasGroup.blocksRaycasts = false;
        }
    }

    public void FadeReloadCurrentScene(int backgroundIndex, string usedEventId)
    {
        if (isTransitioning)
            return;

        StartCoroutine(FadeReloadRoutine(backgroundIndex, usedEventId));
    }

    private IEnumerator FadeReloadRoutine(int backgroundIndex, string usedEventId)
    {
        isTransitioning = true;

        if (SceneManager.Instance != null)
            SceneManager.Instance.AddUIBlockingWorldInput(SceneManager.UIBlockLevel.SceneTransition);
  
        yield return Fade(0f, 1f, fadeToBlackDuration);

        if (SceneManager.Instance != null)
        {
            SceneManager.Instance.SetCurrentSceneBackgroundIndex(backgroundIndex);
            SceneManager.Instance.SetSceneEventState(usedEventId, true);

            SceneManager.Instance.ReloadCurrentScene();
        }

        yield return null;

        yield return new WaitForSeconds(0.2f);

        yield return Fade(1f, 0f, fadeFromBlackDuration);

        if (SceneManager.Instance != null)
            SceneManager.Instance.RemoveUIBlockingWorldInput(SceneManager.UIBlockLevel.SceneTransition);

        isTransitioning = false;
    }

    public void TransitionToScene(int sceneId, Vector3 clickedPosition)
    {
        if (isTransitioning)
            return;

        StartCoroutine(TransitionRoutine(sceneId, clickedPosition));
    }

    private IEnumerator TransitionRoutine(int sceneId, Vector3 clickedPosition)
    {
        isTransitioning = true;

        if (SceneManager.Instance != null)
            SceneManager.Instance.AddUIBlockingWorldInput(SceneManager.UIBlockLevel.SceneTransition);

   
        originalCameraPosition = mainCamera.transform.position;
        originalOrthographicSize = mainCamera.orthographicSize;





        Vector3 zoomTargetPosition = Vector3.Lerp(
    originalCameraPosition,
    new Vector3(
        clickedPosition.x,
        clickedPosition.y,
        originalCameraPosition.z
    ),
    moveTowardTargetAmount
);

        float targetOrthographicSize =
            originalOrthographicSize / zoomAmount;

        float elapsed = 0f;

        fadeCanvasGroup.blocksRaycasts = true;

        while (elapsed < zoomDuration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / zoomDuration);

            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            mainCamera.transform.position =
                Vector3.Lerp(
                    originalCameraPosition,
                    zoomTargetPosition,
                    smoothT
                );

            mainCamera.orthographicSize =
                Mathf.Lerp(
                    originalOrthographicSize,
                    targetOrthographicSize,
                    smoothT
                );

            float fadeT = Mathf.InverseLerp(
                fadeStartPoint,
                1f,
                t
            );

            fadeCanvasGroup.alpha = fadeT;

            yield return null;
        }

        fadeCanvasGroup.alpha = 1f;  // kleiner failsafe falls es nicht 100% schwarz ist




        if (SceneManager.Instance != null)
            SceneManager.Instance.ChangeScene(sceneId);


        mainCamera.transform.position = originalCameraPosition;
        mainCamera.orthographicSize = originalOrthographicSize;

        
        yield return null;



        yield return new WaitForSeconds(0.2f);     // das ist essentially der delay zwischen fade to black und fade back.



        yield return Fade(1f, 0f, fadeFromBlackDuration);

        if (SceneManager.Instance != null)
            SceneManager.Instance.RemoveUIBlockingWorldInput(SceneManager.UIBlockLevel.SceneTransition);

        isTransitioning = false;
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        if (fadeCanvasGroup == null)
            yield break;

        fadeCanvasGroup.blocksRaycasts = true;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / duration);

            fadeCanvasGroup.alpha = Mathf.Lerp(from, to, t);

            yield return null;
        }

        fadeCanvasGroup.alpha = to;

        if (to <= 0f)
            fadeCanvasGroup.blocksRaycasts = false;
    }
}
