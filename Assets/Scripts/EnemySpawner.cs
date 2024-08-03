using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    public SpawnerProfile profile;
    public bool isIndependent = true;
    
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

    private void Update()
    {
        if(isIndependent && PlayerController.count > 0)
            Spawning();
    }

    public int Spawning()
    {
        int killCount = 0;

        if (timer <= 0)
        {
            SpawnEnemy();
            timer = profile.cooldown;
        }
        else
            timer -= Time.deltaTime;

        //Removing Dead Enemies
        for(int i = enemies.Count - 1; i >= 0; i--)
        {
            if(enemies[i] == null)
            {
                enemies.RemoveAt(i);
                killCount++;
            }
        }

        return killCount;
    }

    public void SpawnEnemy()
    {
        //-1 or less count means infinite
        if (count == 0 || enemies.Count >= profile.limit)
            return;
        
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
                lines = transforms.Count - 1;
                selectedIndex = Random.Range(0, lines);
                currentIndex = 0;
                a = b = null;
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
                        a = b;
                    }                
                }
                break;
            case SpawnerProfile.LocationType.LINES_LOOPED:
                lines = transforms.Count;
                selectedIndex = Random.Range(0, lines);
                currentIndex = 0;
                a = b = null;
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
                        a = b;
                    }                
                }
                if(currentIndex == lines - 1)
                {
                    b = transforms[0];
                    position = Vector3.Lerp(a.position, b.position, Random.value);
                }
                break;
            case SpawnerProfile.LocationType.RECTANGLES_OUTLINE:
                lines = transforms.Count / 2;
                selectedIndex = Random.Range(0, lines);
                currentIndex = 0;
                a = b = null;
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
                            int rectLine = Random.Range(0, 4);
                            float ax = a.position.x;
                            float bx = b.position.x;
                            float ay = a.position.z;
                            float by = b.position.z;
                            switch(rectLine)
                            {
                                case 0:
                                    position = Vector3.Lerp(new Vector3(ax, 0.0f, ay), new Vector3(ax, 0.0f, by), Random.value);
                                    break;
                                case 1:
                                    position = Vector3.Lerp(new Vector3(ax, 0.0f, ay), new Vector3(bx, 0.0f, ay), Random.value);
                                    break;
                                case 2:
                                    position = Vector3.Lerp(new Vector3(bx, 0.0f, by), new Vector3(ax, 0.0f, by), Random.value);
                                    break;
                                case 3:
                                    position = Vector3.Lerp(new Vector3(bx, 0.0f, by), new Vector3(bx, 0.0f, ay), Random.value);
                                    break;
                            }
                            break;
                        }
                        a = b = null;
                    }          
                }
                break;
            case SpawnerProfile.LocationType.RECTANGLES_FILLED:
                lines = transforms.Count / 2;
                selectedIndex = Random.Range(0, lines);
                currentIndex = 0;
                a = b = null;
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
                            float ax = a.position.x;
                            float bx = b.position.x;
                            float ay = a.position.z;
                            float by = b.position.z;
                            if(ax < bx)
                                position.x = Random.Range(ax, bx);
                            else
                                position.x = Random.Range(bx, ax);
                            if(ay < by)
                                position.z = Random.Range(ay, by);
                            else
                                position.z = Random.Range(by, ay);
                            break;
                        }
                        a = b = null;
                    }          
                }
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
            
            enemies.Add(newAIController);
            if(count > 0)
                count--;
        }
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
                    Gizmos.DrawSphere(t.position, 0.25f);
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
                a = b = null;
                foreach(Transform t in transforms)
                {
                    if(!a)
                        a = t;
                    else
                    {
                        b = t;
                        Gizmos.DrawLine(a.position, b.position);
                        a = b;
                    }
                }
                break;
            case SpawnerProfile.LocationType.LINES_LOOPED:
                a = b = null;
                foreach(Transform t in transforms)
                {
                    if(!a)
                        a = t;
                    else
                    {
                        b = t;
                        Gizmos.DrawLine(a.position, b.position);
                        a = b;
                    }
                }
                b = transforms[0];
                Gizmos.DrawLine(a.position, b.position);
                break;
            case SpawnerProfile.LocationType.RECTANGLES_OUTLINE:
                a = b = null;
                foreach(Transform t in transforms)
                {
                    if(!a)
                        a = t;
                    else
                    {
                        b = t;
                        float ax = a.position.x;
                        float bx = b.position.x;
                        float ay = a.position.z;
                        float by = b.position.z;
                        Gizmos.DrawLine(new Vector3(ax, 0.1f, ay), new Vector3(ax, 0.1f, by));
                        Gizmos.DrawLine(new Vector3(ax, 0.1f, ay), new Vector3(bx, 0.1f, ay));
                        Gizmos.DrawLine(new Vector3(bx, 0.1f, by), new Vector3(ax, 0.1f, by));
                        Gizmos.DrawLine(new Vector3(bx, 0.1f, by), new Vector3(bx, 0.1f, ay));
                        a = b = null;
                    }          
                }
                break;
            case SpawnerProfile.LocationType.RECTANGLES_FILLED:
                a = b = null;
                foreach(Transform t in transforms)
                {
                    if(!a)
                        a = t;
                    else
                    {
                        b = t;
                        float ax = a.position.x;
                        float bx = b.position.x;
                        float ay = a.position.z;
                        float by = b.position.z;
                        Gizmos.DrawCube(new Vector3((ax+bx)/2.0f, 0.1f, (ay+by)/2.0f), new Vector3(Mathf.Abs(ax - bx), 0.25f, Mathf.Abs(ay - by)));
                        a = b = null;
                    }          
                }
                break;
        }
    }
}
