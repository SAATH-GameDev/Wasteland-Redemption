using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetection : MonoBehaviour
{
    public LayerMask avoidanceMask;
    [HideInInspector] public List<Transform> detectedTriggers = new List<Transform>();

    void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.layer == avoidanceMask) return;
        detectedTriggers.Add(coll.transform);
    }

    void OnTriggerExit(Collider coll)
    {
        if(coll.gameObject.layer == avoidanceMask) return;
        detectedTriggers.Remove(coll.transform);
    }
}