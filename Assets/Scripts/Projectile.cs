using UnityEngine;

public class Projectile : MonoBehaviour
{
    public ProjectileProfile profile;

    private Rigidbody rb;

    public void Set(ProjectileProfile profile)
    {
        if(!profile) return;

        this.profile = profile;
        rb.linearVelocity = transform.forward * profile.speed;
        Destroy(gameObject, profile.destroyTime);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Set(profile);
    }
}
