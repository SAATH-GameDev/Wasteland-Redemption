using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Status Effect", menuName = "Profile/Status Effect", order = 0)]
public class StatusEffectProfile : Profile
{
    public string description;
    public Sprite icon;
    public float duration;
    public float tick;

    public enum Attribute
    {
        HEALTH,
        ATTACK,
        STAMINA,
        SPEED,
        HUNGER,
        TOTAL
    }

    public enum Operator
    {
        ADD,
        SUBTRACT,
        MULTIPLY,
        DIVIDE
    }

    public enum OperationType
    {
        ONCE, //applies a permanent operation once
        OVERTIME, //applies the operation repeatedly for the duration
        MOD //applies a temporarily operation which gets removed after the duration
    }

    [Serializable]
    public class AttributeEffect
    {
        public Attribute attribute;
        public Operator op;
        public float value;
        public OperationType opFrequency;
    }

    [Space]
    public List<AttributeEffect> attributeEffects;

    public float PerformOperation(float value, AttributeEffect effect)
    {
        float originalValue = value;
        switch(effect.op)
        {
            case Operator.ADD:
                value += effect.value;
                break;
            case Operator.SUBTRACT:
                value -= effect.value;
                break;
            case Operator.MULTIPLY:
                value *= effect.value;
                break;
            case Operator.DIVIDE:
                value /= effect.value;
                break;
        }
        return value - originalValue;
    }
    
    public void Apply(ref List<float> attributeValues, ref List<float> modValues)
    {
        foreach(AttributeEffect effect in attributeEffects)
        {
            float attVal = attributeValues[(int)effect.attribute];
            switch(effect.opFrequency)
            {
                case OperationType.ONCE:
                    attributeValues[(int)effect.attribute] += PerformOperation(attVal, effect);
                break;

                case OperationType.MOD:
                    modValues[(int)effect.attribute] += PerformOperation(attVal, effect);
                break;
            }
        }
    }
    
    public void Remove(ref List<float> attributeValues, ref List<float> modValues)
    {
        foreach(AttributeEffect effect in attributeEffects)
        {
            float attVal = attributeValues[(int)effect.attribute];
            switch(effect.opFrequency)
            {
                case OperationType.MOD:
                    modValues[(int)effect.attribute] -= PerformOperation(attVal, effect);
                break;
            }
        }
    }
        
    public void Process(ref List<float> attributeValues)
    {
        foreach(AttributeEffect effect in attributeEffects)
        {
            float attVal = attributeValues[(int)effect.attribute];
            switch(effect.opFrequency)
            {
                case OperationType.OVERTIME:
                    attributeValues[(int)effect.attribute] += PerformOperation(attVal, effect);
                break;
            }
        }
    }
}