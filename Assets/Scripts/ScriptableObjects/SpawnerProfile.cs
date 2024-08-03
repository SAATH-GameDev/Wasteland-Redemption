using UnityEngine;

[CreateAssetMenu(fileName = "New Spawner Profile", menuName = "Profile/Spawner", order = 0)]
public class SpawnerProfile : Profile
{
   public GameObject[] prefab;
   
   public enum LocationType
    {
        POINTS,
        LINES_INDIVIDUAL,
        LINES_JOINED,
        LINES_LOOPED,
        RECTANGLE_OUTLINE,
        RECTANGLE_FILLED,
        AROUND_TARGET
    }
   [Space]
   public LocationType spawnLocation;
   public float radius;
   [Space]
   public int rate;
   public int count;
   [Space]
   public float cooldown;
   public int limit;
}