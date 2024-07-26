using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public WeaponProfile profile;

    public Transform muzzle;

    protected bool isAttacking = false;

    private int currentMagazine = 0;
    private float reloadTimer = 0.0f;

    private int consecutiveCount = 0;
    private float consecutiveTimer = 0.0f;

    protected float timer = 0.0f;

    public void Set(WeaponProfile profile = null)
    {
        if(profile)
            this.profile = profile;

        if(!this.profile)
            return;

        currentMagazine = this.profile.magazine;
    }

    void Start()
    {
        Set();
    }

    public virtual void HandleShooting()
    {
        if(profile.isContinous && isAttacking)
            Attack();
        ReloadingMagazine();
        AttackingConsecutive();
        timer -= Time.deltaTime;
    }

    protected void AttackingConsecutive()
    {
        if(consecutiveCount > 0)
        {
            if(consecutiveTimer <= 0.0f)
            {
                Burst();

                DecrementMagazine();

                consecutiveCount--;
                consecutiveTimer = consecutiveCount > 0 ? profile.consecutiveDelay : 0.0f;

                timer = consecutiveCount > 0 ? 0.0f : profile.delay;
            }
            else
            {
                consecutiveTimer -= Time.deltaTime;
            }
        }
    }

    void EmitProjectile(float yAngleOffset)
    {
        Quaternion projectileRotation = muzzle.rotation;
        projectileRotation = Quaternion.Euler(
            projectileRotation.eulerAngles.x + (Random.Range(-1.0f, 1.0f) * profile.maxDispersionAngle),
            projectileRotation.eulerAngles.y + (Random.Range(-1.0f, 1.0f) * profile.maxDispersionAngle) + yAngleOffset,
            projectileRotation.eulerAngles.z + (Random.Range(-1.0f, 1.0f) * profile.maxDispersionAngle)
        );

        GameObject newProjectile = Instantiate(profile.projectile.prefab, muzzle.position, projectileRotation);
        Projectile projectile = newProjectile.GetComponent<Projectile>();
        projectile.Set(profile.projectile);
        projectile.SetProjectileShotBy(transform.parent.parent);
    }

    void Burst()
    {
        int burstCount = profile.burst + Random.Range(-profile.burstRandomRange, profile.burstRandomRange);
        do
        {
            if(profile.projectile != null)
                EmitProjectile( (burstCount * profile.angleBurst) - ((profile.burst * profile.angleBurst) / 2.0f) );

            burstCount--;
        }
        while(burstCount > 0);
    }

    protected void ReloadingMagazine()
    {
        if(profile.magazine > 0 && currentMagazine <= 0)
        {
            reloadTimer -= Time.deltaTime;
            if(reloadTimer <= 0.0f)
            {
                currentMagazine = profile.magazine;
                reloadTimer = 0.0f;
            }
        }
    }

    void DecrementMagazine()
    {
        if(profile.magazine > 0)
        {
            currentMagazine--;

            if(currentMagazine <= 0)
            {
                reloadTimer = profile.reloadDelay;
            }
        }
    }

    bool Attack()
    {
        if(consecutiveCount > 0)
            return false;

        if(profile.magazine <= 0 || currentMagazine > 0)
        {
            if(timer <= 0.0f)
            {
                Burst();

                DecrementMagazine();

                consecutiveCount = profile.consecutiveCount;
                consecutiveTimer = profile.consecutiveDelay;

                timer = profile.delay;

                return true;
            }
        }
        return false;
    }

    public void Attack(bool toggle = true)
    {
        if(!profile)
            return;

        if(!toggle)
        {
            isAttacking = false;
            return;
        }
        else
        {
            isAttacking = true;
        }

        if(!profile.isContinous)
        {
            Attack();
        }
    }
}