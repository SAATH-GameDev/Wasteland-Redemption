using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Profile", menuName = "Profile/Weapon", order = 0)]
public class WeaponProfile : ItemProfile
{
    [Space]
    public GameObject prefab;
    public bool bothArms;
    public float recoilOffset;

    [Space]
    public bool isContinous;
    public float damage;
    public float delay;

    [Space]
    public ProjectileProfile projectile;
    public float maxDispersionAngle;

    [Space]
    public int magazine;
    public float reloadDelay;

    [Space]
    public int burst;
    public float angleBurst;
    public int burstRandomRange;

    [Space]
    public int consecutiveCount;
    public float consecutiveDelay;
    
    //public StatusEffect statusEffect;
    //public float statusEffectChance;

    [Space]
    public float criticalHitChance;
}