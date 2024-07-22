using UnityEngine;


[CreateAssetMenu(fileName = "New Spawner Profile", menuName = "Profile/Spawner", order = 0)]
public class SpawnerProfile : Profile
{
   public GameObject prefab;
   
   [Space]
   public int rate;
   public int limit;
   public float cooldown;
}

