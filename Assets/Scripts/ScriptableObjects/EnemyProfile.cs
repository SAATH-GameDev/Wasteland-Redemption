using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Profile", menuName = "Profile/Enemy", order = 0)]
public class EnemyProfile : Profile
{
    public float health;
    public float speed;
    
    public int experience;
}