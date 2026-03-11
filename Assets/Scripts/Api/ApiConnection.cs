using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class ApiConnection : MonoBehaviour
{
    private const string rootUrl = "http://localhost:8080/players";
    public string sessionId;

    public IEnumerator CreatePlayer(string firstName, string lastName, string userName,
        System.Action<PlayerResponseDTO> onSuccess, System.Action<string> onError)
    {
        PlayerCreationDTO dto = new PlayerCreationDTO
        {
            firstName = firstName,
            lastName = lastName,
            userName = userName
        };

        string json = JsonUtility.ToJson(dto);

        UnityWebRequest request = new UnityWebRequest(rootUrl, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        HandleResponse(request, onSuccess, onError);

    }

    public IEnumerator SessionLogin(string firstName, string lastName, string userName,
        System.Action<LoginResponseDTO> onSuccess, System.Action<string> onError)
    {
        string url = rootUrl + $"/{firstName}/{lastName}/{userName}";
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
        string url = rootUrl + "/" + sessionId;
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
        string url = rootUrl + "/" + sessionId;
        string gameStateJson = JsonUtility.ToJson(gamestate);
        byte[] body = Encoding .UTF8.GetBytes(gameStateJson);

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
