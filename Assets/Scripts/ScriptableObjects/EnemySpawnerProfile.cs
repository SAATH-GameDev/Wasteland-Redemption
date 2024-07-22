using UnityEngine;

[CreateAssetMenu(fileName = "New Spawner Profile", menuName = "Profile/Enemy Spawner", order = 0)]
public class EnemySpawnerProfile : SpawnerProfile<Enemy>
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
    RANDOMPOSITIONINRADIUS,
    MAPBOUNDARY,
    CUSTOMTRANSFORM
}