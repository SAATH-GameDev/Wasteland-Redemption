using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffectController
{
    public class ActiveStatusEffect
    {
        public StatusEffectProfile profile;
        public float timer = 0.0f;
        public Image fill = null;
        public bool applied = false;
        public float tickTimer = 0.0f;
    }
    private List<ActiveStatusEffect> effects = new List<ActiveStatusEffect>();

    public List<float> attributeValues = new List<float>();
    public List<float> modValues = new List<float>();

    public ActiveStatusEffect GetEffectOfProfile(StatusEffectProfile profile)
    {
        foreach(ActiveStatusEffect e in effects)
            if(profile == e.profile)
                return e;
        return null;
    }

    public void Setup()
    {
        for(int i = 0; i < (int)StatusEffectProfile.Attribute.TOTAL; i++)
        {
            attributeValues.Add(0.0f);
            modValues.Add(0.0f);
        }
    }

    public void Update()
    {
        foreach (var e in effects)
        {
            if(!e.applied)
            {
                e.profile.Apply(ref attributeValues, ref modValues);
                e.applied = true;
            }

            if (e.timer > 0)
            {
                if(e.tickTimer <= 0.0f)
                {
                    e.profile.Process(ref attributeValues);
                    e.tickTimer = e.profile.tick;
                }
                else
                {
                    e.tickTimer -= Time.deltaTime;
                }

                if(e.fill)
                    e.fill.fillAmount = e.timer / e.profile.duration;
                e.timer -= Time.deltaTime;
            }
            else
            {
                e.profile.Remove(ref attributeValues, ref modValues);
                if(e.fill)
                    GameObject.Destroy(e.fill.transform.parent.gameObject);
                effects.Remove(e);
                break;
            }
        }
    }
    
    public void Add(StatusEffectProfile statusEffect, GameObject uiPrefab = null, Transform holder = null)
    {
        ActiveStatusEffect e = GetEffectOfProfile(statusEffect);
        if (e == null)
        {
            ActiveStatusEffect newActiveEffect = new ActiveStatusEffect
            {
                profile = statusEffect,
                timer = statusEffect.duration
            };

            if(uiPrefab)
            {
                GameObject activeEffectUI = GameObject.Instantiate(uiPrefab, holder);
                activeEffectUI.GetComponentInChildren<TextMeshProUGUI>().text = statusEffect.name;
                newActiveEffect.fill = activeEffectUI.transform.Find("Fill").GetComponent<Image>();

                if(statusEffect.icon)
                    newActiveEffect.fill.sprite = activeEffectUI.transform.Find("BG").GetComponent<Image>().sprite = statusEffect.icon;
            }

            effects.Add(newActiveEffect);
        }
        else
        {
            e.timer += statusEffect.duration;
        }
    }
}