using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Profile", menuName = "Profile/Weapon", order = 0)]
public class WeaponProfile : Profile
{
    public float range;
    public bool isContinous;
    public float damage;
    public float firingRate;
    public float cooldown;

    public ProjectileProfile projectile;

    public int burst;
    public float angleBurst;
    public int consecutiveCount;
    public float consecutiveDelay;
    
    //public StatusEffect statusEffect;
    //public float statusEffectChance;

    public float criticalHitChance;
}