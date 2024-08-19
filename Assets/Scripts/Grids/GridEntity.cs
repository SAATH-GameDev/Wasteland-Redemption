using System;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Grids
{
    public enum PaintMode
    {
        PAINT,
        REPLACE,
        DELETE,
        DELETE_PAINT
    }

    public class GridEntity : MonoBehaviour
    {
        [SerializeField] protected Vector2Int gridPosition = Vector2Int.zero;     // this is cell position
        [HideInInspector] public Vector3 offset = Vector3.zero;

        [HideInInspector] public GameObject prefab;

        public virtual Vector2Int GridPosition
        {
            get { return gridPosition; }
            set { gridPosition = value; }
        }

        public void UpdatePosition()
        {
            transform.position = new Vector3(
                GridPosition.x * (Grid.Instance.size.x + Grid.Instance.gap.x), 0.0f,
                GridPosition.y * (Grid.Instance.size.z + Grid.Instance.gap.z)
            ) + offset;
        }

        public virtual void Paint(Vector2Int gridPosition, Vector3 offset, Quaternion rotation, Transform parent, PaintMode mode = PaintMode.PAINT)
        {
            if (parent.parent.GetComponent<Palette>() == null) return;

            var groupIndex = parent.parent.GetComponent<Palette>().activePainter;
            if (Grid.Instance.IsThereGridEntityAtGridPositionParam(gridPosition, groupIndex))
                switch (mode)
                {
                    case PaintMode.PAINT:
                        return;
                    case PaintMode.REPLACE:
                        var gridEntityToReplace = Grid.Instance.GetEntity(gridPosition, groupIndex);
                        DestroyImmediate(gridEntityToReplace.gameObject);
                        break;
                    case PaintMode.DELETE:
                        var gridEntityToDelete = Grid.Instance.GetEntity(gridPosition, groupIndex);
                        DestroyImmediate(gridEntityToDelete.gameObject);
                        #if UNITY_EDITOR
                        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
                        #endif
                        return;
                    case PaintMode.DELETE_PAINT:
                        var gridEntityToDelete2 = Grid.Instance.GetEntity(gridPosition, groupIndex);
                        DestroyImmediate(gridEntityToDelete2.gameObject);
                        #if UNITY_EDITOR
                        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
                        #endif
                        return;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
                }

            if (mode == PaintMode.DELETE) return;

            #if UNITY_EDITOR
            var newGridEntityObject = (prefab != null ? PrefabUtility.InstantiatePrefab(prefab) : Instantiate(gameObject)) as GameObject;
            newGridEntityObject.transform.parent = parent;
            newGridEntityObject.transform.localRotation = rotation;

            var gridEntity = newGridEntityObject.GetComponent<GridEntity>();
            if(!gridEntity) gridEntity = newGridEntityObject.AddComponent<GridEntity>();
            gridEntity.gridPosition = gridPosition;
            gridEntity.offset = offset;
            gridEntity.UpdatePosition();
            
            EditorUtility.SetDirty(newGridEntityObject);

            #endif
        }
    }
}