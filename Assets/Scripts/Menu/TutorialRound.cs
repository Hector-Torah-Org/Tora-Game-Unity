using System;
using UnityEngine;

public class TutorialRound : MonoBehaviour
{
    public Annotationsguideline annotationsguideline;

    public void StartTutorialRound(string message = "")
    {
        annotationsguideline.StartRound(() =>
        {
            Debug.Log("StartingTutorialTest");
            this.gameObject.SetActive(true);
            StartCoroutine(TorahManager.Instance.StartTutorialRound((result) =>
            {
                if (!result.passedTutorial)
                {
                    StartTutorialRound($"You only had an accuracy of {Math.Round(result.correctAnswerRate * 100)}%, which isn't enough to move on. Please read the guideline carefully to ensure your annotations are correct. Good luck on your next try.");
                }
                else
                {
                    this.gameObject.SetActive(false);
                }
            }));
        }, message);
    }
}
