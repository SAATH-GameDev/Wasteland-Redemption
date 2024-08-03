using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventTrigger : MonoBehaviour
{
    public string targetTag = "Player";
    
    [Space]
    public UnityEvent onEnter;

    [Space]
    public float stayProcessDelay = 0.1f;
    public UnityEvent onStay;

    [Space]
    public UnityEvent onExit;

    [Space]
    public float triggerDestroyTimer = -1.0f;
    public UnityEvent onDestroy;

    [Space]
    public int sequenceIndex = -1;
    public List<UnityEvent> sequence = new List<UnityEvent>();

    [HideInInspector] public Transform target = null;

    private float stayProcessTimer = 0.0f;

    public void StepSequence()
    {
        sequenceIndex++;

        if(sequenceIndex < 0 || sequenceIndex >= sequence.Count)
            return;

        sequence[sequenceIndex].Invoke();
    }

    public void PlaySequence(int index)
    {
        if(index < 0 || index >= sequence.Count)
            return;

        sequence[index].Invoke();
    }

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
            GameManager.Instance.currentTrigger = this;
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
            GameManager.Instance.currentTrigger = null;
            onExit.Invoke();
        }
    }
}
