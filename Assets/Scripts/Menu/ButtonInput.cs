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

    public void LoginClicked()
    {
        string firstName = firstNameInput.text;
        string lastName = lastNameInput.text;
        string userName = userNameInput.text;
        

        StartCoroutine(apiConnection.SessionLogin(firstName, lastName, userName, success => { panel.SetActive(false); }, 
                                    error => { errorDisplay.text = "User not found"; }));
    }

    public void SignUpLoginClicked()
    {
        string firstName = firstNameInput.text;
        string lastName = lastNameInput.text;
        string userName = userNameInput.text;

        StartCoroutine(apiConnection.CreatePlayer(firstName, lastName, userName, success => {
                        StartCoroutine(apiConnection.SessionLogin(firstName, lastName, userName, success => { panel.SetActive(false); }, error => { })); },
                        error => { errorDisplay.text = "Names already taken"; }));

    }










}
