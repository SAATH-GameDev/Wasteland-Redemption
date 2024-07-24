using UnityEngine;

public class Breakable : MonoBehaviour, IDamageable
{
    public GameObject destroyedVersion;

    public void TakeDamage (int damge)
    {
        Instantiate(destroyedVersion, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    //private void OnMouseDown()
    //{
    //    Instantiate(destroyedVersion, transform.position, transform.rotation);
    //    Destroy(gameObject);
    //}

}
