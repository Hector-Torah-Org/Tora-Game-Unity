using System;
using UnityEngine;

public class TutorialRound : MonoBehaviour
{
    public Annotationsguideline annotationsguideline;

    public void StartTutorialRound(double retryAccuracy = -1)
    {
        SceneManager.Instance.AddUIBlockingWorldInput(SceneManager.UIBlockLevel.ForcedTutorial);
        annotationsguideline.StartRound(() =>
        {
            Debug.Log("StartingTutorialTest");
            this.gameObject.SetActive(true);
            StartCoroutine(TorahManager.Instance.StartTutorialRound((result) =>
            {
                Debug.Log("Received tutorialAnswerDTO");
                if (!result.passedTutorial)
                {
                    Debug.Log("Restarting Tutorial");
                    StartTutorialRound(Math.Round(result.correctAnswerRate * 100));
                }
                else
                {
                    this.gameObject.SetActive(false);
                    SceneManager.Instance.RemoveUIBlockingWorldInput(SceneManager.UIBlockLevel.ForcedTutorial);
                }
            }));
        }, retryAccuracy >= 0 ? "tutorial.retry_message" : "", retryAccuracy);
    }
}
