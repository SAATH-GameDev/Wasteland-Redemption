using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody rb;

    public void SetVelocity(float speed)
    {
        rb.linearVelocity = transform.forward * speed;
    }
}
