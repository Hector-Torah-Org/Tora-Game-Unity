using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApiTester : MonoBehaviour 
{
    [SerializeField] private ApiConnection apiConnection;
    private void Start()
    {
        Debug.Log("Started Api Tester");
        StartCoroutine(TesterRoutine());
    }


    IEnumerator TesterRoutine() { 
        
        //Player Creation Testen
        Debug.Log("Testing player creation");
        yield return StartCoroutine(apiConnection.CreatePlayer("Frederik", "Haase", "haafr", response =>
        {
            Debug.Log("Created Player with first name: " + response.firstName + ", last name: " + response.lastName + ", user name: " + response.userName);
        }, error => Debug.LogError(error)));


        //In erstellten Player einloggen 
        yield return StartCoroutine(apiConnection.SessionLogin("Frederik", "Haase", "haafr", response =>
        {
            Debug.Log("Logged into session" + apiConnection.sessionId);
        }, error => Debug.LogError(error)));

        //Playernamen ändern
        yield return StartCoroutine(apiConnection.UpdatePlayer("Frederiks", "Haases", "haasefrsNewName", response =>
        {
            Debug.Log("Updated player to first name: " + response.firstName + ", last name: " + response.lastName + ", user name: " + response.userName);
        }, error => Debug.LogError(error)));

        //Image erhalten
        List<string> imageIds = new List<string>();
        yield return StartCoroutine(apiConnection.GetImage(response =>
        {
            Debug.Log("Received image with id: " + response.id + ", link: " + response.link + ", character: " + response.character);
            imageIds.Add(response.id);
        }));

        yield return StartCoroutine(apiConnection.GetImage(response =>
        {
            Debug.Log("Received image with id: " + response.id + ", link: " + response.link + ", character: " + response.character);
            imageIds.Add(response.id);
        }));

        //Klassificationen senden
        yield return StartCoroutine(apiConnection.SendClassifications(new List<Classification>
        {
            new Classification() { imageId = imageIds[0], isDecorated = true },
            new Classification() { imageId = imageIds[1], isDecorated = false }
        }));


    }
}
