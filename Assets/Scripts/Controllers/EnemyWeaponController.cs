using UnityEngine;

public class EnemyWeaponController : WeaponController
{
    protected override void Start()
    {
        base.Start();

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
        timer = -0.1f;
    }
}