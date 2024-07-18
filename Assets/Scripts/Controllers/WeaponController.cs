using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public WeaponProfile profile;

    public Transform muzzle;

    void Start()
    {

    }

    void Update()
    {

    }

    public void Attack(bool toggle = true)
    {
        if(!profile || !toggle) return;

        if(profile.projectile != null)
        {
            GameObject newProjectile = Instantiate(profile.projectile.prefab, muzzle.position, muzzle.rotation);
            newProjectile.GetComponent<Projectile>().Set(profile.projectile);
        }
    }
}