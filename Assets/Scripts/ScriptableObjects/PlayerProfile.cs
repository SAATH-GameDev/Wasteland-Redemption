using UnityEngine;

[CreateAssetMenu(fileName = "New Player Profile", menuName = "Profile/Player", order = 0)]
public class PlayerProfile : Profile
{
    public float health;
    public float attack;
    public float stamina;
    public float speed;
    public float hunger;
}