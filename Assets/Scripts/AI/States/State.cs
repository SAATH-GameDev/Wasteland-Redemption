using UnityEngine;

[RequireComponent(typeof(StateMachine))]
public class State : MonoBehaviour
{
    [Header("Pre State")]
    public float preStateMinDelay = 0.0f;
    public float preStateMaxDelay = 0.2f;

    [HideInInspector] public Detector detector = null;

    private float preStateTimer = 0.0f;

    public Transform GetTarget()
    {
        if(detector == null)
            return null;

        return detector.detectedTarget;
    }

    void Update()
    {
        if(preStateTimer > 0.0f)
        {
            preStateTimer -= Time.deltaTime;
            return;
        }
        
        Process();
    }

    public virtual void Enter()
    {
        enabled = true;
        preStateTimer = Random.Range(preStateMinDelay, preStateMaxDelay);
    }

    public virtual void Process()
    {
        //nothing
    }

    public virtual void Exit()
    {
        enabled = false;
    }
}