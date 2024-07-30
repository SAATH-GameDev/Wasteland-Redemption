using UnityEngine;
using UnityEngine.Events;

public class WeaponController : MonoBehaviour
{
    public WeaponProfile profile;

    public Transform muzzle;

    protected bool isAttacking = false;
    protected UnityEvent onMagazineChange = new UnityEvent();

    [HideInInspector] public int currentMagazine = 0;
    [HideInInspector] public float reloadTimer = 0.0f;

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

    protected virtual void Start()
    {
        Set();

        if(muzzle.childCount > 0)
            muzzle.GetChild(0).GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    public virtual void HandleShooting()
    {
        if(profile.isContinous && isAttacking)
            Attack();
        ReloadingMagazine();
        AttackingConsecutive();
        timer -= Time.deltaTime;
    }

    public void Reload()
    {
        if(currentMagazine < profile.magazine)
        {
            reloadTimer = profile.reloadDelay;
            currentMagazine = 0;
            onMagazineChange.Invoke();
        }
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
        projectile.SetShotBy(transform.parent.parent);

        if(muzzle.childCount > 0)
        {
            ParticleSystem muzzleVFX = muzzle.GetChild(0).GetComponent<ParticleSystem>();
            muzzleVFX.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            muzzleVFX.Play();
        }
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
        if(profile.magazine > 0 && reloadTimer > 0)
        {
            reloadTimer -= Time.deltaTime;
            if(reloadTimer <= 0.0f)
            {
                currentMagazine = profile.magazine;
                onMagazineChange.Invoke();

                reloadTimer = 0.0f;
            }
        }
    }

    protected void DecrementMagazine()
    {
        if(profile.magazine > 0)
        {
            currentMagazine--;
            onMagazineChange.Invoke();

            if(currentMagazine <= 0)
                reloadTimer = profile.reloadDelay;
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