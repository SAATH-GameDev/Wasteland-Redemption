using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StateMachine))]
public class Detector : MonoBehaviour
{
    public string targetTag = "";
    public float delay = 0.1f;

    [HideInInspector] public Transform detectedTarget = null;

    private float timer = 0.0f;

    protected virtual void UpdateDetection() {}

    public void SetTimer(float time)
    {
        timer = time;
    }

    void Update()
    {
        if (timer <= 0.0f)
        {
            UpdateDetection();
            SetTimer(delay);
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }
}