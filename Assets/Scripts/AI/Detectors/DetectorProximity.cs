using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorProximity : Detector
{
    public float radius = 10.0f;

    protected override void UpdateDetection()
    {
        detectedTarget = null;

        Collider[] colls = Physics.OverlapSphere(transform.position, radius);
        foreach(var coll in colls)
        {
            if(coll.gameObject.tag == targetTag)
            {
                detectedTarget = coll.transform;
                break;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}