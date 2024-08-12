using UnityEngine;
using UnityEngine.Events;

public class WeaponController : MonoBehaviour
{
    public WeaponProfile profile;

    protected Transform displayTransform;
    protected Transform muzzle;
    protected bool isAttacking = false;

    protected UnityEvent onMagazineChange = new UnityEvent();
    protected UnityEvent onWeaponChange = new UnityEvent();

    protected int currentMagazine = 0;
    protected float reloadTimer = 0.0f;

    private int consecutiveCount = 0;
    private float consecutiveTimer = 0.0f;

    protected float timer = 0.0f;

    public int GetCurrentMagazine()
    {
        return currentMagazine;
    }

    public bool IsReloading()
    {
        return reloadTimer > 0.0f;
    }

    public void Set(WeaponProfile profile = null)
    {
        this.profile = profile;
        
        //Destroy previous weapon prefab if any
        if(displayTransform.childCount > 0)
            Destroy(displayTransform.GetChild(0).gameObject);
        
        //Create new weapon prefab and set its muzzle
        muzzle = (profile && profile.prefab) ? Instantiate(profile.prefab, transform.GetChild(0)).transform.GetChild(0) : transform;

        if(muzzle.childCount > 0)
        {
            ParticleSystem muzzleFlash = muzzle.GetChild(0).GetComponent<ParticleSystem>();
            if(muzzleFlash)
                muzzleFlash.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        currentMagazine = 0;
        reloadTimer = profile ? profile.reloadDelay : 0.0f;

        onWeaponChange.Invoke();
    }

    protected virtual void Start()
    {
        displayTransform = transform.GetChild(0);
    }

    public virtual void HandleShooting(ref int ammo)
    {
        if(!profile)
            return;
        if(profile.isContinous && isAttacking)
            Attack();
        ReloadingMagazine(ref ammo);
        AttackingConsecutive();
        timer -= Time.deltaTime;
    }

    public void Reload()
    {
        if(!profile)
            return;
        if(currentMagazine < profile.magazine)
        {
            reloadTimer = profile.reloadDelay;
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
            ParticleSystem muzzleFlash = muzzle.GetChild(0).GetComponent<ParticleSystem>();
            if(muzzleFlash)
            {
                muzzleFlash.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                muzzleFlash.Play();
            }
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

    protected void ReloadingMagazine(ref int ammo)
    {
        if(IsReloading())
        {
            if(ammo <= 0)
            {
                reloadTimer = 0.0f;
                onMagazineChange.Invoke();
                return;
            }

            if(profile.magazine > 0)
            {
                reloadTimer -= Time.deltaTime;
                if(reloadTimer <= 0.0f)
                {
                    int requiredAmmoToAdd = profile.magazine - currentMagazine;
                    int ammoToAdd = ammo >= requiredAmmoToAdd ? requiredAmmoToAdd : ammo;

                    currentMagazine += ammoToAdd;
                    ammo -= ammoToAdd;

                    reloadTimer = 0.0f;

                    onMagazineChange.Invoke();
                }
            }
        }
    }

    protected void DecrementMagazine()
    {
        if(profile.magazine > 0)
        {
            currentMagazine--;
            if(currentMagazine <= 0)
                reloadTimer = profile.reloadDelay;
            
            onMagazineChange.Invoke();
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