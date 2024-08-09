using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorTrigger : Detector
{
    public TriggerDetection triggerDetection = null;
    public float delayAfterDetectingTarget = 0.0f;

    private float detectedTargetTimer = 0.0f;

    protected override void UpdateDetection()
    {
        if(detectedTargetTimer <= 0.0f)
        {
            if(triggerDetection.detectedTriggers.Count > 0)
            {
                foreach(var detectedTrigger in triggerDetection.detectedTriggers)
                {
                    if(detectedTrigger != null && detectedTrigger.tag == targetTag)
                    {
                        detectedTarget = detectedTrigger;
                        detectedTargetTimer = delayAfterDetectingTarget;
                    }
                }
                if(detectedTargetTimer <= 0.0f) detectedTarget = null;
            }
            else
            {
                detectedTarget = null;
            }
        }
        else
        {
            detectedTargetTimer -= delay;
        }
    }
}