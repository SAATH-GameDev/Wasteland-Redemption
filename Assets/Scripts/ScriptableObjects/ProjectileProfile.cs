using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile Profile", menuName = "Profile/Projectile", order = 0)]
public class ProjectileProfile : Profile
{
    public GameObject prefab;
    public float damage;
    public float speed;
    public float destroyTime;
}