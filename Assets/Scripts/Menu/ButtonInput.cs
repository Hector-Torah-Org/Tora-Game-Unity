using JetBrains.Annotations;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonInput : MonoBehaviour
{
    public ApiConnection apiConnection;
    public TMP_InputField firstNameInput;
    public TMP_InputField lastNameInput;
    public TMP_InputField userNameInput;

    public GameObject panel;

    public TMP_Text errorDisplay;

    public TutorialRound tutorialRound;

    private void Start()
    {
        SceneManager.Instance.AddUIBlockingWorldInput(SceneManager.UIBlockLevel.LoginInterface);
    }

    public void LoginClicked()
    {
        string firstName = firstNameInput.text;
        string lastName = lastNameInput.text;
        string userName = userNameInput.text;
        

        StartCoroutine(apiConnection.SessionLogin(firstName, lastName, userName, success => 
                                    { 
                                        panel.SetActive(false);
                                        SceneManager.Instance.RemoveUIBlockingWorldInput(SceneManager.UIBlockLevel.LoginInterface);

                                        if (!success.hasPassedTutorial)
                                        {
                                            Debug.Log("Start Tutorial");
                                            tutorialRound.StartTutorialRound();
                                        }
                                        
                                        if (GameStateManager.Instance != null)
                                        {
                                            GameStateManager.Instance.LoadGameStateString(success.gameState);
                                        }
                                    }, 
                                    error => { Debug.LogError(error); I18nText.SetText(errorDisplay, "login.failed"); }));
    }

    public void SignUpLoginClicked()
    {
        string firstName = firstNameInput.text;
        string lastName = lastNameInput.text;
        string userName = userNameInput.text;

        StartCoroutine(apiConnection.CreatePlayer(firstName, lastName, userName, success => {
                        StartCoroutine(apiConnection.SessionLogin(firstName, lastName, userName, success => 
                        {

                            panel.SetActive(false); Debug.Log("Login successful");
                            SceneManager.Instance.RemoveUIBlockingWorldInput(SceneManager.UIBlockLevel.LoginInterface);

                            if (!success.hasPassedTutorial)
                            {
                                tutorialRound.StartTutorialRound();
                            }

                            if (GameStateManager.Instance != null)
                            {
                                GameStateManager.Instance.LoadGameStateString(success.gameState);
                            }
                        }, error => { Debug.LogError(error); I18nText.SetText(errorDisplay, "login.failed"); })); },
                        error => { Debug.LogError(error); I18nText.SetText(errorDisplay, "signup.names_taken"); }));

    }










}
