using System;
using UnityEngine;

public class TutorialRound : MonoBehaviour
{
    public Annotationsguideline annotationsguideline;

    public void StartTutorialRound(string message = "")
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
                    StartTutorialRound($"Da du nur eine Genauigkeit von {Math.Round(result.correctAnswerRate * 100)}% erreicht hast, kannst du noch nicht mit dem Spiel beginnen. Bitte lies dir den Annotationsleitfaden erneut genau durch, um falsche Annotationen zu vermeiden. Viel Erfolg beim nächsten Versuch.");
                }
                else
                {
                    this.gameObject.SetActive(false);
                    SceneManager.Instance.RemoveUIBlockingWorldInput(SceneManager.UIBlockLevel.ForcedTutorial);
                }
            }));
        }, message);
    }
}
