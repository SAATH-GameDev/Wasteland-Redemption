using UnityEngine;

namespace Grids
{
    public class Palette : MonoBehaviour
    {
        public PainterGroup group;

        [Space]
        public float rotationYOffset = 0.0f;
        public bool randomYRotation = false;

        [HideInInspector] public int activePainter;
        [HideInInspector] public PaintMode paintMode = PaintMode.PAINT;
        [HideInInspector] public int index;
        [HideInInspector] public Vector2Int boxGridPosition = Vector2Int.one * -9999;

        public Painter Painter
        {
            get
            {
                if (group == null || group.painters == null || group.painters.Count <= 0 ||
                    activePainter >= group.painters.Count)
                {
                    activePainter = 0;
                    return null;
                }

                return group.painters[activePainter];
            }
        }

        public GameObject Get()
        {
            return group.painters[activePainter].objects[index];
        }
    }
}