using UnityEngine;

public class Collectible : MonoBehaviour, ICollectible
{
    public ItemProfile itemProfile;

    [Space]
    public float floatHeight = 0.1f;
    public float floatRate = 0.5f;
    public float rotateRate = 20.0f;

    private Transform display;
    private Vector3 basePosition;

    public virtual ItemProfile Collect()
    {
        Destroy(gameObject);
        return itemProfile;
    }

    void Start()
    {
        display = transform.GetChild(0);
        basePosition = display.position;
    }

    void Update()
    {
        display.position = basePosition + ((Vector3.up * floatHeight) * Mathf.Sin(Time.time * floatRate));
        display.Rotate(0.0f, rotateRate * Time.deltaTime, 0.0f);
    }
}
