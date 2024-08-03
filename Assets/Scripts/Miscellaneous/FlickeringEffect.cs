using UnityEngine;

[RequireComponent(typeof(Light))]
public class FlickeringEffect : MonoBehaviour
{
    public float intensityMaxRatio;
    public float intensityMinRatio;
    [Space]
    public float stableMinTime;
    public float stableMaxTime;
    [Space]
    public float unstableMinTime;
    public float unstableMaxTime;
    [Space]
    public float flickerMinTime;
    public float flickerMaxTime;

    private Light flickeringLight;
    private float baseIntensity;
    private bool isStable = true;
    private float timer = 0.0f;
    private float flickerTimer = 0.0f;

    void Start()
    {
        flickeringLight = GetComponent<Light>();
        baseIntensity = flickeringLight.intensity;
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
                    flickeringLight.intensity = baseIntensity * Random.Range(intensityMinRatio, intensityMaxRatio);
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
