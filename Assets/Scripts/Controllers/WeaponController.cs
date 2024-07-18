using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public WeaponProfile profile;

    public Transform muzzle;

    private bool isAttacking = false;

    private float timer = 0.0f;

    void Start()
    {

    }

    void Update()
    {
        if(profile.isContinous && isAttacking)
        {
            Attack();
        }

        timer -= Time.deltaTime;
    }

    bool Attack()
    {
        if(timer <= 0.0f)
        {
            if(profile.projectile != null)
            {
                int burstCount = profile.burst;
                do {
                    Quaternion projectileRotation = muzzle.rotation;
                    projectileRotation = Quaternion.Euler(
                        projectileRotation.eulerAngles.x,
                        projectileRotation.eulerAngles.y + (burstCount * profile.angleBurst) - ((profile.burst * profile.angleBurst) / 2.0f),
                        projectileRotation.eulerAngles.z
                    );

                    GameObject newProjectile = Instantiate(profile.projectile.prefab, muzzle.position, projectileRotation);
                    newProjectile.GetComponent<Projectile>().Set(profile.projectile);
                    burstCount--;
                } while(burstCount > 0);
            }
            timer = profile.delay;
            return true;
        }
        return false;
    }

    public void Attack(bool toggle = true)
    {
        if(!profile) return;

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
            Attack();
    }
}