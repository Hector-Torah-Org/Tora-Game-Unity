using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using NUnit.Framework;
using System.Collections.Generic;

public class ApiConnection : MonoBehaviour
{
    private const string rootUrl = "http://localhost:8080";
    public string sessionId;


    //Player Actions
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
        string url = rootUrl + $"/players/{firstName}/{lastName}/{userName}";
        UnityWebRequest request = UnityWebRequest.Get(url);
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

    //Image actions

    public IEnumerator GetImage(int amount, System.Action<ImageResponseListDTO> onSuccess)
    {
        string url = rootUrl + "/image/" + sessionId + "/" + amount;
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
