using System.Collections;
using UnityEngine;

public class ApiTester : MonoBehaviour 
{
    [SerializeField] private ApiConnection playerApiConnection;
    private void Start()
    {
        Debug.Log("Started Api Tester");
        StartCoroutine(TesterRoutine());
    }


    IEnumerator TesterRoutine() { 
        
        //Player Creation Testen
        yield return StartCoroutine(playerApiConnection.CreatePlayer("Frederik", "Haase", "haafr", response =>
        {
            Debug.Log("Created Player with first name: " + response.firstName + ", last name: " + response.lastName + ", user name: " + response.userName);
        }, error => Debug.LogError(error)));


        //In erstellten Player einloggen 
        yield return StartCoroutine(playerApiConnection.SessionLogin("Frederik", "Haase", "haafr", response =>
        {
            Debug.Log("Logged into session" + playerApiConnection.sessionId);
        }, error => Debug.LogError(error)));

        yield return StartCoroutine(playerApiConnection.UpdatePlayer("Frederiks", "Haases", "haasefrsNewName", response =>
        {
            Debug.Log("Updated player to first name: " + response.firstName + ", last name: " + response.lastName + ", user name: " + response.userName);
        }, error => Debug.LogError(error)));
    
    }
}
