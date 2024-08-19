using System.Collections.Generic;
using UnityEngine;

namespace Grids
{
    [CreateAssetMenu(fileName = "Painter Group", menuName = "New Painter Group")]
    public class PainterGroup : ScriptableObject
    {
        public List<Painter> painters = new();
    }
}