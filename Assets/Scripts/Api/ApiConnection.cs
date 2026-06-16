using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using System;

public class ApiConnection : MonoBehaviour
{
    private const string rootUrl = "http://localhost:8080";
    public string sessionId;
    public string userName;


    //=========================Player Actions=============================//

    public IEnumerator CreatePlayer(string firstName, string lastName, string userName,
        System.Action<PlayerResponseDTO> onSuccess, System.Action<string> onError)
    {
        Debug.Log($"Creating player with firstName: {firstName}, lastName: {lastName}, userName: {userName}");
        PlayerCreationDTO dto = new PlayerCreationDTO
        {
            firstName = firstName,
            lastName = lastName,
            userName = userName
        };

        string json = JsonUtility.ToJson(dto);
        string url = rootUrl + "/players";

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        Debug.Log("Sending player creation request with body: " + json);
        yield return request.SendWebRequest();
        Debug.Log("Received response: " + request.downloadHandler.text);

        HandleResponse(request, onSuccess, onError);

    }

    public IEnumerator SessionLogin(string firstName, string lastName, string userName,
        System.Action<LoginResponseDTO> onSuccess, System.Action<string> onError)
    {
        this.userName = userName;
        string url = rootUrl + "/players/login";
        PlayerCreationDTO dto = new PlayerCreationDTO
        {
            firstName = firstName,
            lastName = lastName,
            userName = userName
        };
        string json = JsonUtility.ToJson(dto);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        HandleResponse<LoginResponseDTO>(request, result =>
        {
            sessionId = result.sessionUUID;

            onSuccess?.Invoke(result);
        }, onError);
    }

    public IEnumerator UpdatePlayer(string newFirstName, string newLastName, string newUserName, System.Action<PlayerResponseDTO> onSuccess, System.Action<string> onError)
    {
        string url = rootUrl + "/players/" + sessionId;
        PlayerCreationDTO updatedPlayerDto = new PlayerCreationDTO() { 
            firstName = newFirstName,
            lastName = newLastName,
            userName = newUserName};

        string updatedPlayerJson = JsonUtility.ToJson(updatedPlayerDto);
        byte[] body = Encoding.UTF8.GetBytes(updatedPlayerJson);

        UnityWebRequest request = UnityWebRequest.Put(url, body);
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        HandleResponse(request, onSuccess, onError);
    }

    public IEnumerator UpdateGameState(string gamestate)
    {
        string url = rootUrl + "/players/" + sessionId;
        string gameStateJson = JsonUtility.ToJson(gamestate);
        byte[] body = Encoding .UTF8.GetBytes(gameStateJson);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        HandleResponse<string>(request, result => Debug.Log(result), error => Debug.LogError(error));
    }

    //==================================Image actions=============================//

    public IEnumerator GetImage(int amount, System.Action<ImageResponseListDTO> onSuccess)
    {
        string url = rootUrl + "/image/" + sessionId + "/" + amount;
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        HandleResponse(request, onSuccess, error => Debug.LogError(error));
    }

    public IEnumerator GetTestImage(int amount, System.Action<ImageResponseListDTO> onSuccess)
    {
        string url = rootUrl + "/image/" + sessionId + "/" + amount + "?forTutorial=true";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        HandleResponse(request, onSuccess, error => Debug.LogError(error));
    }

    public IEnumerator SendClassifications(List<Classification> classifications)
    {
        string url = rootUrl + "/image/" + sessionId;
        Debug.Log("Classifications: " + classifications[1]);
        ClassificationSendDTO classificationsDto = new ClassificationSendDTO() { classifications = classifications };
        string classificationsJson = JsonUtility.ToJson(classificationsDto);
        classificationsJson = classificationsJson.Replace("{\"classifications\":", "").TrimEnd('}');
        Debug.Log("Sending classifications: " + classificationsJson);
        byte[] body = Encoding.UTF8.GetBytes(classificationsJson);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        HandleResponse<string>(request, result => Debug.Log(result), error => Debug.LogError(error));
    }

    
    
    public IEnumerator SendTestClassifications(List<Classification> classifications, System.Action<double> onSuccess)
    {
        string url = rootUrl + "/image/" + sessionId + "?giveFeedback=true";
        Debug.Log("Classifications: " + classifications[1]);
        ClassificationSendDTO classificationsDto = new ClassificationSendDTO() { classifications = classifications };
        string classificationsJson = JsonUtility.ToJson(classificationsDto);
        classificationsJson = classificationsJson.Replace("{\"classifications\":", "").TrimEnd('}');
        Debug.Log("Sending classifications: " + classificationsJson);
        byte[] body = Encoding.UTF8.GetBytes(classificationsJson);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(body);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        HandleResponse<double>(request, result => { Debug.Log(result); onSuccess?.Invoke(result); }, error => Debug.LogError(error));
    }

    //=================================Statistics actions=============================//

    public IEnumerator GetAmountStatistic(int year, System.Action<StatisticsDTO> onSuccess)
    {
        string url = rootUrl + $"/Statistics/playerAmount/{sessionId}/{year}";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        Debug.Log("Received response: " + request.downloadHandler.text);
        HandleResponse(request, onSuccess, error => Debug.LogError(error));
    }

    public IEnumerator GetConfidenceStatistic(int year, System.Action<StatisticsDTO> onSuccess)
    {
        string url = rootUrl + $"/Statistics/playerConfidence/{sessionId}/{year}";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        Debug.Log("Received response: " + request.downloadHandler.text);
        HandleResponse(request, onSuccess, error => Debug.LogError(error));
    }

    //=================================Leaderboard actions=============================//

    /**
     * <summary>Retrieves the specified leaderboard on a given page and pagesize</summary>
     * <param name="page"> The page number to retrieve, starting from 0</param>
     * <param name="pagesize"> The number of items to be on each page</param>
     * <param name="type"> The type of leaderboard to retrieve. 
     * <list type="bullet">
     *      
     *      <item>
     *          <term>amount</term>
     *          <description>Gets a Leaderboard counting total classifications</description>
     *      </item>
     *      <item>
     *          <term>amountPlayer</term>
     *          <description>Gets a Leaderboard counting total classifications, centered on the player. Page is ignored</description>
     *      </item>
     *      <item>
     *          <term>confidence</term>
     *          <description>Gets a Leaderboard averaging total confidence</description>
     *      </item>
     *      <item>
     *          <term>confidencePlayer</term>
     *          <description>Gets a Leaderboard averaging total confidence, centered on the player. Page is ignored</description>
     *      </item>
     * </list>
     * </param>
     * 
    */
    public IEnumerator GetLeaderboard(int page, int pagesize, string type, System.Action<LeaderboardDTO> onSuccess)
    {
        string url = rootUrl + "/Statistics";
        switch (type) { 
            
            case "amount":
                url += $"/totalClassifications/{page}/{pagesize}";
                break;
            case "confidence":
                url += $"/totalConfidence/{page}/{pagesize}";
                break;
            case "amountPlayer":
                url += $"/totalClassifications/fromPlayer/{sessionId}/{pagesize}";
                break;
            case "confidencePlayer":
                url += $"/totalConfidence/fromPlayer/{sessionId}/{pagesize}";
                break;
            default:
                Debug.LogError($"Unknown leaderboard type: {type}");
                yield break;
        }

        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();
        HandleResponse(request, onSuccess, error => Debug.LogError(error));


    }


    //=========================Helper Methods=============================//


    //Response Handler
    private void HandleResponse<T>(UnityWebRequest request, System.Action<T> onSuccess, System.Action<string> onError)
    {
        if (request.result == UnityWebRequest.Result.Success)
        {
            T obj = JsonUtility.FromJson<T>(request.downloadHandler.text);
            onSuccess?.Invoke(obj);
        }
        else
        {
            onError?.Invoke(request.error + " | " + request.downloadHandler.text);
        }
    }
}
