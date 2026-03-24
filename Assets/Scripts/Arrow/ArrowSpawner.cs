using UnityEngine;

public class ArrowSpawner : MonoBehaviour
{
    [SerializeField] private ArrowScript arrowPrefab;

    public ArrowScript createArrow(float x, float y, float d, int s, float scale = 1f)
    {
        ArrowScript arrow = Instantiate(arrowPrefab);

        arrow.Init(new Vector2(x, y), d, s);                                            // Das wird gecalled, um einen Arrow zu spawnen

        arrow.GetComponent<SpriteRenderer>().sortingOrder = 1000;

        arrow.transform.localScale = new Vector3(scale, scale, 1f);

        return arrow;
    }
}
