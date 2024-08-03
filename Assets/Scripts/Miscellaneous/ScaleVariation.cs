using UnityEngine;

public class ScaleVariation : MonoBehaviour
{
    public float min;
    public float max;

    void Start()
    {
        transform.localScale = Vector3.one * Random.Range(min, max);
    }
}
