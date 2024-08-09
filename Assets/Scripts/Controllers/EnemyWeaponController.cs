using UnityEngine;

public class EnemyWeaponController : WeaponController
{
    protected override void Start()
    {
        base.Start();

        Set(profile);

        onWeaponChange.AddListener( () => {
            muzzle.parent.gameObject.layer = LayerMask.NameToLayer("Enemy");
        } );
    }

    public override void HandleShooting()
    {
        if(!profile)
            return;
        Attack();
        ReloadingMagazine();
        AttackingConsecutive();
        timer -= Time.deltaTime;
    }
}