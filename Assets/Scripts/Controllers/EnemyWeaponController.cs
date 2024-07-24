public class EnemyWeaponController : WeaponController
{
    public override void HandleShooting()
    {
        Attack();
        ReloadingMagazine();
        timer = -0.1f;
    }
}