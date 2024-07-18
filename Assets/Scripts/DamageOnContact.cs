using UnityEngine;

public class DamageOnContact : MonoBehaviour
{
    public int damage = 0;
    public bool destroyOnContact = true;

    public void OnTriggerEnter(Collider collider)
    {
        IDamageable damageable = collider.GetComponent<IDamageable>();

        if(damageable == null)
            damageable = collider.GetComponentInParent<IDamageable>();

        if(damageable != null)
        {
            damageable.TakeDamage(damage);

            if(destroyOnContact)
                Destroy(gameObject);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        IDamageable damageable = collision.transform.GetComponent<IDamageable>();
        
        if(damageable == null)
            damageable = collision.transform.GetComponentInParent<IDamageable>();

        if(damageable != null)
        {
            damageable.TakeDamage(damage);

            if(destroyOnContact)
                Destroy(gameObject);
        }
    }
}