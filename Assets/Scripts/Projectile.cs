using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public ProjectileProfile profile;

    private Rigidbody rb;

    public Transform bulletShotBy;

    public void Set(ProjectileProfile profile)
    {
        rb = GetComponent<Rigidbody>();

        if(!profile) return;
        
        this.profile = profile;
        rb.linearVelocity = transform.forward * profile.speed;
        Destroy(gameObject, profile.destroyTime);
    }
    
    public void SetProjectileShotBy(Transform bulletShotBy)
    {
        this.bulletShotBy = bulletShotBy;
    }

    void Start()
    {
        Set(profile);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out AIController aiController))
        {
            aiController.ChaseTargetAfterShot(bulletShotBy);
        }
    }
}
