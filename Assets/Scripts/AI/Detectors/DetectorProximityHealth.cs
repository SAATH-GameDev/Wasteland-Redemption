using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorProximityHealth : Detector
{
    public float radius = 10.0f;
    public float ratio = 0.4f;
    public Comparison compare = Comparison.LESSER;

    protected override void UpdateDetection()
    {
        if(!GameManager.Compare(GetComponentInParent<GameEntity>().GetHealthRatio(), compare, ratio))
            return;

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