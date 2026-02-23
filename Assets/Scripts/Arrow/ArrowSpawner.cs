using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    [SerializeField] private ArrowScript arrowPrefab;

    public ArrowScript createArrow(float x, float y, float d, int s)
    {
        ArrowScript arrow = Instantiate(arrowPrefab);

        arrow.Init(new Vector2(x, y), d, s);                                            // Das wird gecalled, um einen Arrow zu spawnen

        return arrow;
    }
}
