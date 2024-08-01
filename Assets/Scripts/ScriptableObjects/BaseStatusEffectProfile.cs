using UnityEngine;

public abstract class BaseStatusEffectProfile : Profile
{
    public string effectName;
    public string description;
    public Sprite icon;
    public float duration;
        
    public abstract void ApplyEffect(GameObject target);
        
    public abstract void RemoveEffect(GameObject target);
        
    public abstract void UpdateEffect(GameObject target);
}