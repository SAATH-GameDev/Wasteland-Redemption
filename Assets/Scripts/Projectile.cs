using UnityEngine;

public class Projectile : MonoBehaviour
{
    public ProjectileProfile profile;

    [Space]
    public GameObject preserveObject;
    public float preserveTimer = 1.0f;

    protected Rigidbody rb;

    private Transform bulletShotBy;
    private Vector3 preserveObjectOffset;

    public virtual void Set(ProjectileProfile profile)
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

    protected virtual void OnCollisionEnter(Collision other)
    {
        DetectorDamage detector = other.gameObject.GetComponentInChildren<DetectorDamage>();
        if(detector)
            detector.SetDamagedBy(bulletShotBy);
    }
}