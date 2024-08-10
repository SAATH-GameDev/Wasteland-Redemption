using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Profile", menuName = "Profile/Enemy", order = 0)]
public class EnemyProfile : Profile
{
    public float health;
    public float speed;

    [Serializable]
    public struct Loot
    {
        public GameObject item;
        public float chance;
    }
    public List<Loot> lootDrops = new List<Loot>();
}