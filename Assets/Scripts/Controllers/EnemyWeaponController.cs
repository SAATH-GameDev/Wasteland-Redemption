public class EnemyWeaponController : WeaponController
{
    public override void HandleShooting()
    {
        if(!profile)
            return;
        Attack();
        ReloadingMagazine();
        timer = -0.1f;
    }
}