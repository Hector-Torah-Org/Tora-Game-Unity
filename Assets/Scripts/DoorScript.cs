using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private DoorSpawnData data;
    private bool isOpen;

    [SerializeField] private GameObject closedVisual;
    [SerializeField] private GameObject openVisual;

    private ArrowScript doorArrow;
                                                                    
    public void Init(DoorSpawnData doorData, bool open)
    {
        data = doorData;

        transform.position = new Vector3(data.position.x, data.position.y, transform.position.z);
        transform.localScale = new Vector3(data.scale.x, data.scale.y, 1f);

        SetOpen(open);
    }

    private void SetOpen(bool open)
    {
        isOpen = open;

        if (closedVisual != null) closedVisual.SetActive(!isOpen);
        if (openVisual != null) openVisual.SetActive(isOpen);

        if (isOpen)
        {
            if (doorArrow == null && SceneManager.Instance != null)
            {
                doorArrow = SceneManager.Instance.SpawnArrowOnDoor(
                    transform,
                    data.nextSceneId,
                    data.arrowRotation
                );
                Debug.Log($"Door {data.id}: spawned arrow -> scene {data.nextSceneId}, rot {data.arrowRotation}");
            }
            else
            {
                Debug.Log($"Door {data.id}: arrow already exists or SceneManager missing");
            }
        }
        else
        {
            if (doorArrow != null)
            {
                Destroy(doorArrow.gameObject);
                doorArrow = null;
            }
        }
    }


    private void OnMouseDown()
    {
        if (isOpen)
        {
            return;
        }

        
        if (TorahManager.Instance != null)
        {

            TorahManager.Instance.StartRound(                                                                         //TEMPORARY, das ist das win/loss feedback
                onWin: () =>
                {                                                                                    
                    
                    if (SceneManager.Instance != null)
                        SceneManager.Instance.SetDoorOpen(data.id, true);           //Speicher door state

                    
                    SetOpen(true);                                                 //setzt door auf offen
                },
                onLose: () =>
                {
                                                                                    //Noch nicht gemacht (verlieren wird man ja wahrscheinlich sowieso nicht
                }
            );
        }
        else
        {
            Debug.LogWarning("TorahManager.Instance is null - no minigame started.");
        }
    }
}
