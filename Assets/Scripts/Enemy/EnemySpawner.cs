using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemySpawnerProfile profile;
    
    private float cooldown;
    
    private int limit;
    private int rate;

    private void Start()
    {
        cooldown = profile.cooldown;
        limit = profile.limit;
        rate = profile.rate;
    }

    private void Update()
    {
        cooldown -= Time.deltaTime;
        
        if(cooldown <= 0)
        {
            SpawnEnemy();
            cooldown = profile.cooldown;
        }
    }

    private void SpawnEnemy()
    {
        if (limit <= 0)
        {
            return;
        }

        for (int i = 0; i < rate; i++)
        {
            // todo #1 - replace spawn position to be determined by the profile with respect to player position
            
            Instantiate(profile.prefab, transform.position, Quaternion.identity);
            limit--;
        }
    }
}
