using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour
{
    public string targetTag = "Player";
    
    public UnityEvent onEnter;

    public float stayProcessDelay = 0.1f;
    public UnityEvent onStay;

    public UnityEvent onExit;

    public float triggerDestroyTimer = -1.0f;
    public UnityEvent onDestroy;

    [HideInInspector] public Transform target = null;

    private float stayProcessTimer = 0.0f;

    void OnDestroy()
    {
        onDestroy.Invoke();
    }

    void Update()
    {
        if(target != null && stayProcessTimer <= 0.0f)
        {
            onStay.Invoke();

            stayProcessTimer = stayProcessDelay;
        }
        else
        {
            stayProcessTimer -= Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(targetTag))
        {
            target = other.transform;
            onEnter.Invoke();

            if(triggerDestroyTimer > 0.0f)
                Destroy(gameObject, triggerDestroyTimer);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag(targetTag) && other.transform == target)
        {
            target = null;
            onExit.Invoke();
        }
    }
}
