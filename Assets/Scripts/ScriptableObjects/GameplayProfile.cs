using Unity.Cinemachine;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gameplay Profile", menuName = "Profile/Gameplay", order = 0)]
public class GameplayProfile : Profile
{
    [Header("Hunger")]
    public float hungerDepletionRate;
    public float hungerHealthDepletionRate;

    [Space]
    [Range(0.0f, 1.0f)]
    public float hungerHealthReplenishMinRatio;
    public float hungerHealthReplenishRate;
    [Range(0.0f, 1.0f)]
    public float hungerHealthReplenishLimitRatio;
}