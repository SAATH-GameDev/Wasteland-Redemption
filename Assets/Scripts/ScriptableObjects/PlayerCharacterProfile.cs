using UnityEngine;

[CreateAssetMenu(fileName = "New Player Character Profile", menuName = "Profile/Player Character", order = 0)]
public class PlayerCharacterProfile : Profile
{
    [Range(0.5f, 2f)]
    public float health;

    [Range(0.5f, 2f)]
    public float attack;

    [Range(0.5f, 2f)]
    public float stamina;

    [Range(0.5f, 2f)]
    public float speed;

    [Range(0.5f, 2f)]
    public float hunger;
}