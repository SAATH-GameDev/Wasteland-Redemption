using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveController : MonoBehaviour
{
    public List<EnemySpawner> spawners = new List<EnemySpawner>();

    public UnityEvent onCompletion;

    [Space]
    public bool isStarted = false;

    private int enemyCount;
    private float surviveTimer;

    public static WaveController Instance;

    void Start()
    {
        Instance = this;
    }

    void Update()
    {
        if(enemyCount > 0 || surviveTimer > 0)
        {
            isStarted = true;
            foreach(EnemySpawner spawner in spawners)
                enemyCount -= spawner.Spawning();
            surviveTimer -= Time.deltaTime;
        }
        else if(isStarted)
        {
            isStarted = false;
            onCompletion?.Invoke();
            enemyCount = 0;
            surviveTimer = 0.0f;
        }
    }
}