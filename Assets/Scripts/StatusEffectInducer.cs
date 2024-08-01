using System;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectInducer : MonoBehaviour
{
    [Serializable]
    public class InductEffect
    {
        public StatusEffectProfile effect;
        public float chance;
    }
    public List<InductEffect> effects = new List<InductEffect>();
    public bool tryTrigger = false;
    public bool destroyOnContact = false;

    public void OnTriggerEnter(Collider collider)
    {
        if(!tryTrigger)
            return;

        IDamageable damageable = collider.GetComponent<IDamageable>();

        if(damageable == null)
            damageable = collider.GetComponentInParent<IDamageable>();

        if(damageable != null)
        {
            if(damageable is PlayerController)
            {
                PlayerController player = (PlayerController)damageable;
                if(player)
                    foreach(InductEffect e in effects)
                        if(UnityEngine.Random.value < e.chance)
                            player.AddStatusEffect(e.effect);
            }
        }

        if(destroyOnContact)
            Destroy(gameObject);
    }

    public void OnCollisionEnter(Collision collision)
    {
        IDamageable damageable = collision.transform.GetComponent<IDamageable>();
        
        if(damageable == null)
            damageable = collision.transform.GetComponentInParent<IDamageable>();

        if(damageable != null)
        {
            if(damageable is PlayerController)
            {
                PlayerController player = (PlayerController)damageable;
                if(player)
                    foreach(InductEffect e in effects)
                        if(UnityEngine.Random.value < e.chance)
                            player.AddStatusEffect(e.effect);
            }
        }

        if(destroyOnContact)
            Destroy(gameObject);
    }
}
