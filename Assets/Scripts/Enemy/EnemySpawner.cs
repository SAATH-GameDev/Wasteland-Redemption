using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public EnemySpawnerProfile profile;
   
    // todo: can be a list of transforms for multiple spawn points to be chosen randomly from.
    // public Transform spawnTransform = null;
    
    private List<AIController> enemies = new List<AIController>();
    
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

    // todo: spawning enemies just for testing purpose
    // private void Update()
    // {
    //     if(PlayerController.count <= 0)
    //         return;
    //
    //     // test spawn   
    //     if (cooldown <= 0)
    //     {
    //         SpawnEnemy();
    //         cooldown = profile.cooldown;
    //     }
    //     else
    //     {
    //         cooldown -= Time.deltaTime;
    //     }
    // }


    public void SpawnEnemy()
    {
        if (limit <= 0)
        {
            return;
        }
        
        Vector3 spawnPosition = Vector3.zero;
        
        switch (profile.spawnLocation)
        {
            case SpawnLocation.RandomPositionInRadius:
                spawnPosition = new Vector3(Random.Range(-profile.spawnRadius, profile.spawnRadius), 0, Random.Range(-profile.spawnRadius, profile.spawnRadius));
                break;
            case SpawnLocation.MapBoundary:
                spawnPosition = new Vector3(Random.Range(profile.minMapBoundaryX.x, profile.minMapBoundaryX.y), 0, Random.Range(profile.minMapBoundaryZ.x, profile.minMapBoundaryZ.y));
                break;
            case SpawnLocation.CustomTransforms:
               // spawnPosition = spawnTransform !? spawnTransform.position : transform.position;
                break;
            case SpawnLocation.AroundTarget:
                float randomAngle = Random.Range(0, 360);
                float randomRadius = Random.Range(-profile.spawnRadius, profile.spawnRadius);
                
                // todo: consider adding target transform so that the position is around the target
                spawnPosition = new Vector3(Mathf.Cos(randomAngle) * randomRadius, 0, Mathf.Sin(randomAngle) * randomRadius);
                break;
        }

        for (int i = 0; i < rate; i++)
        {
            float randomOffsetX = Random.Range(-rate * 1f, rate);
            float randomOffsetZ = Random.Range(-rate * 1f, rate);
            
            Vector3 offset = new Vector3(randomOffsetX, 0, randomOffsetZ);
            
            AIController newAIController = Instantiate(profile.prefab, spawnPosition + offset, Quaternion.identity);
            enemies.Add(newAIController);
            limit--;
        }
    }
    
    public void DespawnEnemy(AIController aiController)
    {
        currentRemainingEnemies--;
        enemies.Remove(aiController);
        Destroy(aiController.gameObject);
    }
    
    public void KillAllEnemies()
    {
        foreach (AIController enemy in enemies)
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