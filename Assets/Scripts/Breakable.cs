using UnityEngine;

public class Breakable : MonoBehaviour
{
    public GameObject destroyedVersion;

    [Space]
    public float maxPieceForce = 10.0f;

    [Space]
    public float minPieceDestroyTime = 1.0f;
    public float maxPieceDestroyTime = 4.0f;

    void Start()
    {
        GetComponent<GameEntity>().OnDeath.AddListener(Break);
    }

    public void Break()
    {
        if (destroyedVersion != null)
        {
            GameObject destroyedObject = Instantiate(destroyedVersion, transform.position, transform.rotation);
            for(int i = 0; i < destroyedObject.transform.childCount; i++)
            {
                destroyedObject.transform.GetChild(i).GetComponent<Rigidbody>().AddRelativeForce(Random.onUnitSphere * maxPieceForce);
                Destroy(destroyedObject.transform.GetChild(i).gameObject, Random.Range(minPieceDestroyTime, maxPieceDestroyTime));
            }
            Destroy(destroyedObject, maxPieceDestroyTime);
        }
    }
}
