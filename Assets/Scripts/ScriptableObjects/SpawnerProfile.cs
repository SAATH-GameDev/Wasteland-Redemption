using UnityEngine;

public class SpawnerProfile<T> : Profile
{
   public T prefab;
   
   [Space]
   public int rate;
   public int limit;
   public float cooldown;
}