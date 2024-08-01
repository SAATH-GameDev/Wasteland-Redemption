using System.Collections.Generic;
using UnityEngine;

public class StatusEffectController : MonoBehaviour
{
    private Dictionary<BaseStatusEffectProfile, GameObject> activeStatusEffects = new Dictionary<BaseStatusEffectProfile, GameObject>();
    
    public List<float> statusEffectTimers = new List<float>();

    private void Update()
    {
        int index = 0;
        foreach (var activeStatusEffect in activeStatusEffects)
        {
            if (statusEffectTimers[index] > 0)
            {
                activeStatusEffect.Key.UpdateEffect(activeStatusEffect.Value);
                statusEffectTimers[index] -= Time.deltaTime;
            }
            else
            {
                RemoveStatusEffect(activeStatusEffect.Key, activeStatusEffect.Value);
            }
            index++;
        }
    }
    
    public void AddStatusEffect(BaseStatusEffectProfile statusEffect, GameObject target)
    {
        if (activeStatusEffects.TryAdd(statusEffect, target))
        {
            statusEffect.ApplyEffect(target);
            statusEffectTimers.Add(statusEffect.duration);
        }
    }
    
    public void RemoveStatusEffect(BaseStatusEffectProfile statusEffect, GameObject target)
    {
        if (activeStatusEffects.ContainsKey(statusEffect))
        {
            activeStatusEffects.Remove(statusEffect);
            statusEffect.RemoveEffect(target);
        }
    }

}