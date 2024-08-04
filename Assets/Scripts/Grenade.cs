using UnityEngine;

public class Grenade : Projectile
{
    [SerializeField] StatusEffectProfile statusEffectProfile;
    
    public override void Set(ProjectileProfile profile)
    {
        rb = GetComponent<Rigidbody>();

        if(!profile) return;
        
        this.profile = profile;
        
        rb.useGravity = true;
        rb.AddForce( (transform.forward + transform.up) * profile.speed, ForceMode.Impulse);
        GetComponent<DamageOnContact>().damage = profile.damage;
        Destroy(gameObject, profile.destroyTime);
    }

    protected override void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out AIController aiController))
        {
            aiController.AddStatusEffect( statusEffectProfile );
            
            rb.linearVelocity = Vector3.zero;
            rb.useGravity = false;
        }
        
        base.OnCollisionEnter(other);
        
       
    }
}