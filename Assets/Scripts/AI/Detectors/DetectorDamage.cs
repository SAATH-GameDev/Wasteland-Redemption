using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorDamage : Detector
{
    private Transform damagedBy;

    public void SetDamagedBy(Transform damagedBy)
    {
        this.damagedBy = damagedBy;
    }

    protected override void UpdateDetection()
    {
        if(damagedBy.CompareTag(targetTag))
            detectedTarget = damagedBy;
        damagedBy = null;
    }
}