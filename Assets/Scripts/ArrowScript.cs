using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public int SceneToCall { get; private set; }

    public void Init(Vector2 position, float rotationDegrees, int sceneToCall)
    {
        Debug.Log($"Arrow spawned at {transform.position} rot={rotationDegrees} sceneToCall={sceneToCall}");

        SceneToCall = sceneToCall;

        transform.position = new Vector3(position.x, position.y, transform.position.z);        // Hier wende ich die Parameter an (hier die pos)
        transform.rotation = Quaternion.Euler(0f, 0f, rotationDegrees);                        //                                 (hier die rotation)


        if (SceneManager.Instance != null)
        {
            SceneManager.Instance.RegisterArrow(this);
        }
    }

    private void OnDestroy()
    {
        if (SceneManager.Instance != null)
        {
            SceneManager.Instance.UnregisterArrow(this);                                       //Wenn der Arrow despawned wird
        }
    }

    private void OnMouseDown()
    {
        if (SceneManager.Instance != null)
        {
            SceneManager.Instance.ChangeScene(SceneToCall);                                    // Wenn der Arrow geklickt wird, ruft es im SceneManager die Funktion auf und wechselt die Scene
        }
    }
}

