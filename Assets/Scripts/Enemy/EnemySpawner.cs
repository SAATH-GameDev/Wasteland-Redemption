using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public SpawnerProfile profile;
    
    //Child Transforms will act as Spawn Points
    private List<Transform> transforms = new List<Transform>();

    private List<AIController> enemies = new List<AIController>();

    private int count;
    private float timer;

    private void SetupTransforms()
    {
        transforms.Clear();
        if(transform.childCount <= 0)
            transforms.Add(transform);
        else
            for(int i = 0; i < transform.childCount; i++)
                transforms.Add(transform.GetChild(i));
    }
   
    private void Start()
    {
        SetupTransforms();
        count = profile.count;
        timer = profile.cooldown;
    }

    public void Update()
    {
        if(PlayerController.count <= 0)
            return;

        //TODO: Don't spawn here when attached with Wave System/Mechanic
        if (timer <= 0)
        {
            SpawnEnemy();
            timer = profile.cooldown;

            //Removing Dead Enemies
            for(int i = enemies.Count - 1; i >= 0; i--)
                if(enemies[i] == null)
                    enemies.RemoveAt(i);
        }
        else
            timer -= Time.deltaTime;
    }

    public List<AIController> SpawnEnemy()
    {
        //-1 or less count means infinite
        if (count == 0 || enemies.Count >= profile.limit)
            return null;

        List<AIController> spawnedEnemies = new List<AIController>();
        
        Vector3 position = Vector3.zero;
        
        switch (profile.spawnLocation)
        {
            case SpawnerProfile.LocationType.POINTS:
                position = transforms[Random.Range(0, transforms.Count)].position;
                break;
            case SpawnerProfile.LocationType.LINES_INDIVIDUAL:
                int lines = transforms.Count / 2;
                int selectedIndex = Random.Range(0, lines);
                int currentIndex = 0;
                Transform a = null;
                Transform b = null;
                foreach(Transform t in transforms)
                {
                    if(!a)
                        a = t;
                    else
                    {
                        b = t;
                        if(currentIndex != selectedIndex)
                            currentIndex++;
                        else
                        {
                            position = Vector3.Lerp(a.position, b.position, Random.value);
                            break;
                        }
                        a = b = null;
                    }                
                }
                break;
            case SpawnerProfile.LocationType.LINES_JOINED:
                break;
            case SpawnerProfile.LocationType.LINES_LOOPED:
                break;
            case SpawnerProfile.LocationType.RECTANGLE_OUTLINE:
                break;
            case SpawnerProfile.LocationType.RECTANGLE_FILLED:
                break;
            case SpawnerProfile.LocationType.AROUND_TARGET:
                float randomAngle = Random.Range(0, 360);
                float randomRadius = Random.Range(-profile.radius, profile.radius);
                
                // todo: consider adding target transform so that the position is around the target
                position = new Vector3(Mathf.Cos(randomAngle) * randomRadius, 0, Mathf.Sin(randomAngle) * randomRadius);
                break;
        }

        for (int i = 0; i < profile.rate; i++)
        {
            Vector3 offset = new Vector3(Random.Range(-profile.radius, profile.radius), 0.0f, Random.Range(-profile.radius, profile.radius));
            AIController newAIController = Instantiate(profile.prefab[Random.Range(0, profile.prefab.Length)], position + offset, Quaternion.identity).GetComponent<AIController>();
            
            spawnedEnemies.Add(newAIController);
            enemies.Add(newAIController);

            if(count > 0)
                count--;
        }

        return spawnedEnemies;
    }
    
    public void KillAllEnemies()
    {
        foreach (AIController enemy in enemies)
            Destroy(enemy.gameObject);
        enemies.Clear();
    }
    
    public int GetCount()
    {
        return enemies.Count;
    }

    void OnDrawGizmosSelected()
    {
        SetupTransforms();
        Gizmos.color = Color.red;
        switch(profile.spawnLocation)
        {
            case SpawnerProfile.LocationType.POINTS:
                foreach(Transform t in transforms)
                    Gizmos.DrawWireSphere(t.position, 0.1f);
                break;
            case SpawnerProfile.LocationType.LINES_INDIVIDUAL:
                Transform a = null;
                Transform b = null;
                foreach(Transform t in transforms)
                {
                    if(!a)
                        a = t;
                    else
                    {
                        b = t;
                        Gizmos.DrawLine(a.position, b.position);
                        a = b = null;
                    }
                }
                break;
            case SpawnerProfile.LocationType.LINES_JOINED:
                break;
            case SpawnerProfile.LocationType.LINES_LOOPED:
                break;
            case SpawnerProfile.LocationType.RECTANGLE_OUTLINE:
                break;
            case SpawnerProfile.LocationType.RECTANGLE_FILLED:
                break;
        }
    }
}
