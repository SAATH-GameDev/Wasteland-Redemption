using UnityEngine;

[CreateAssetMenu(fileName = "New Spawner Profile", menuName = "Profile/Enemy Spawner", order = 0)]
public class EnemySpawnerProfile : SpawnerProfile<AIController>
{
    public SpawnLocation spawnLocation;
    
    [Space]
    public float spawnRadius;
    
    [Space]
    public Vector2 minMapBoundaryX;
    public Vector2 minMapBoundaryZ;
}

public enum SpawnLocation
{
    RandomPositionInRadius,
    MapBoundary,
    CustomTransforms,
    AroundTarget
}