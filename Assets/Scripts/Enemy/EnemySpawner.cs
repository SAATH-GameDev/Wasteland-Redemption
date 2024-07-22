using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemySpawnerProfile profile;
    public Transform spawnTransform = null;
    
    private List<Enemy> enemies = new List<Enemy>();
    
    private float cooldown;
    
    private int limit;
    private int rate;
    private int currentRemainingEnemies;
    
   
    private void Start()
    {
        cooldown = profile.cooldown;
        limit = profile.limit;
        currentRemainingEnemies = limit;
        rate = profile.rate;
    }


    public void SpawnEnemy()
    {
        if (limit <= 0)
        {
            return;
        }
        
        Vector3 spawnPosition = Vector3.zero;
        
        switch (profile.spawnLocation)
        {
            case SpawnLocation.RANDOMPOSITIONINRADIUS:
                spawnPosition = new Vector3(Random.Range(-profile.spawnRadius, profile.spawnRadius), 0, Random.Range(-profile.spawnRadius, profile.spawnRadius));
                break;
            case SpawnLocation.MAPBOUNDARY:
                spawnPosition = new Vector3(Random.Range(profile.minMapBoundaryX.x, profile.minMapBoundaryX.y), 0, Random.Range(profile.minMapBoundaryZ.x, profile.minMapBoundaryZ.y));
                break;
            case SpawnLocation.CUSTOMTRANSFORM:
                spawnPosition = spawnTransform !? spawnTransform.position : transform.position;
                break;
        }

        for (int i = 0; i < rate; i++)
        {
            Enemy newEnemy = Instantiate(profile.prefab, spawnPosition, Quaternion.identity);
            enemies.Add(newEnemy);
            limit--;
        }
    }
    
    public void DespawnEnemy(Enemy enemy)
    {
        currentRemainingEnemies--;
        enemies.Remove(enemy);
        Destroy(enemy.gameObject);
    }
    
    public void KillAllEnemies()
    {
        foreach (Enemy enemy in enemies)
        {
            Destroy(enemy.gameObject);
        }
        enemies.Clear();
    }
    
    public int GetActiveEnemiesCount()
    {
        return enemies.Count;
    }
}
