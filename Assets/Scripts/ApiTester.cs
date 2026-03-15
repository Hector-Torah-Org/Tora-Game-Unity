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
        yield return StartCoroutine(apiConnection.GetImage(4, response =>
        {
            foreach (var image in response.images)
            {
                Debug.Log("Received image with id: " + image.id + ", link: " + image.link + ", character: " + image.character);
                imageIds.Add(image.id);
            }
        }));

        

        //Klassificationen senden
        yield return StartCoroutine(apiConnection.SendClassifications(new List<Classification>
        {
            new Classification() { imageId = imageIds[0], isDecorated = true },
            new Classification() { imageId = imageIds[1], isDecorated = false },
            new Classification() { imageId = imageIds[2], isDecorated = true },
            new Classification() { imageId = imageIds[3], isDecorated = false }
        }));


    }
}
