using System.Collections.Generic;
using UnityEngine;

namespace Grids
{
    [CreateAssetMenu(fileName = "Painter", menuName = "New Painter")]
    public class Painter : ScriptableObject
    {
        public List<GameObject> objects = new();
        public Vector3 offset = Vector3.zero;
    }
}