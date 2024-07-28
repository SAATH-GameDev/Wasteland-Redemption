using UnityEngine;

[RequireComponent(typeof(Light))]
public class FlickeringEffect : MonoBehaviour
{
    public float stableMinTime;
    public float stableMaxTime;
    [Space]
    public float unstableMinTime;
    public float unstableMaxTime;
    [Space]
    public float flickerMinTime;
    public float flickerMaxTime;

    private Light flickeringLight;
    private bool isStable = true;
    private float timer = 0.0f;
    private float flickerTimer = 0.0f;

    void Start()
    {
        flickeringLight = GetComponent<Light>();
        isStable = true;
        timer = Random.Range(stableMinTime, stableMaxTime);
        flickerTimer = 0.0f;
    }

    void Update()
    {
        if(timer > 0.0f)
        {
            if(!isStable)
            {
                if(flickerTimer <= 0.0f)
                {
                    flickeringLight.enabled = !flickeringLight.enabled;
                    flickerTimer = Random.Range(flickerMinTime, flickerMaxTime);
                }
                else
                {
                    flickerTimer -= Time.deltaTime;
                }
            }

            timer -= Time.deltaTime;

            if(timer <= 0.0f)
            {
                if(isStable)
                {
                    isStable = false;
                    timer = Random.Range(unstableMinTime, unstableMaxTime);
                    flickerTimer = Random.Range(flickerMinTime, flickerMaxTime);
                }
                else
                {
                    isStable = true;
                    timer = Random.Range(stableMinTime, stableMaxTime);
                    flickerTimer = 0.0f;
                }
            }
        }
    }
}
