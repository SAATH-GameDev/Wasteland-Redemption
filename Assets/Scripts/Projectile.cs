using UnityEngine;

public class Projectile : MonoBehaviour
{
    public ProjectileProfile profile;

    [Space]
    public GameObject preserveObject;
    public float preserveTimer = 1.0f;

    private Rigidbody rb;

    private Transform bulletShotBy;
    private Vector3 preserveObjectOffset;

    public void Set(ProjectileProfile profile)
    {
        rb = GetComponent<Rigidbody>();

        if(!profile) return;
        
        this.profile = profile;
        rb.linearVelocity = transform.forward * profile.speed;
        GetComponent<DamageOnContact>().damage = profile.damage;
        Destroy(gameObject, profile.destroyTime);
    }
    
    public void SetShotBy(Transform bulletShotBy)
    {
        this.bulletShotBy = bulletShotBy;
    }

    void Start()
    {
        Set(profile);

        if(!preserveObject)
            return;
        preserveObjectOffset = preserveObject.transform.position - transform.position;
        Destroy(preserveObject, preserveTimer);
    }

    void Update()
    {
        if(!preserveObject)
            return;
        preserveObject.transform.parent = null;
        preserveObject.transform.position = transform.position + preserveObjectOffset;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out AIController aiController))
        {
            aiController.ChaseTargetAfterShot(bulletShotBy);
        }
    }
}
