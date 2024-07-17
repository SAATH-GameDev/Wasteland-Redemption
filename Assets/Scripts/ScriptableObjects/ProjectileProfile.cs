using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile Profile", menuName = "Profile/Projectile", order = 0)]
public class ProjectileProfile : Profile
{
    public float damage;
    public float speed;
    public float destroyTime;
}