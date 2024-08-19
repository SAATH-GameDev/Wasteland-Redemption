using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Grids
{
    public class Grid : MonoBehaviour
    {
        public Vector3 size = Vector3.one;
        public Vector3 gap = Vector3.zero;
        public Vector3 normal = Vector3.up;

        [Space]
        [Header("Guides")]
        public bool guides = true;
        [Range(0.0f, 1.0f)]
        public float guideAlpha = 0.5f;
        public int adjacentCubeDisplayRange = 5;

        [Space]
        public Vector3 extends = Vector3.one * 20.0f;

        [Space]
        public Transform temporaryObjectsGroup = null;

        private Camera cam;

        public static Grid Instance { get; set; }

        public static Vector2Int CursorGridPosition { get; set; } = Vector2Int.zero;
        public static Vector3 CursorRawPosition { get; set; } = Vector3.zero;

        public static int Null = -99999;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            cam = Camera.main;
        }

        private void OnValidate()
        {
            Awake();
            UpdateRawPositionsOfAllGridEntities();
        }
        
        //Sets the current position of the mouse on the grid (updates the CursorGridPosition and CursorRawPosition at all times!)
        public void SetCursorGridPositionToMousePosition(bool editor = false)
        {
            Ray ray;
            #if UNITY_EDITOR
            if (editor)
            {
                if (SceneView.currentDrawingSceneView == null) return;

                Vector3 rawPosition = Event.current.mousePosition;
                rawPosition.y = Mathf.Abs(rawPosition.y - SceneView.currentDrawingSceneView.camera.pixelHeight);
                ray = SceneView.currentDrawingSceneView.camera.ScreenPointToRay(rawPosition);
            }   
            else
            {
                ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            }
            #else
                ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            #endif

            var plane = new Plane(normal, 1.0f);
            float distanceFromRay;

            if (plane.Raycast(ray, out distanceFromRay))
            {
                CursorRawPosition = ray.GetPoint(distanceFromRay);
                UpdateCursorGridPosition();
                UpdateCursorRawPosition();
            }
        }
        
        //Updates the cell to the mouse position cell
        public void UpdateCursorGridPosition()
        {
            CursorGridPosition = ConvertRawPositionParamToGridPosition(CursorRawPosition);
        }

        //Updates the grid position to the mouse position cell
        public void UpdateCursorRawPosition()
        {
            CursorRawPosition = ConvertGridPositionParamToRawPosition(CursorGridPosition);
        }
        
        //Updates the raw position of all the objects on the grid
        public void UpdateRawPositionsOfAllGridEntities()
        {
            for (var groupIndex = 0; groupIndex < transform.childCount; groupIndex++)
            {
                var tr = transform.GetChild(groupIndex);
                for (var i = 0; i < tr.childCount; i++)
                {
                    var gridEntity = tr.GetChild(i).GetComponent<GridEntity>();
                    if (gridEntity != null) gridEntity.UpdatePosition();
                }
            }
        }
        
        //Clear the cell positions of all the objects on the grid
        public void DestroyAllGridEntities()
        {
            ClearTemporaryObjects();
            
            var objectsToBeDestroyed = new List<GameObject>();
            for (var i = 0; i < transform.childCount; i++)
                objectsToBeDestroyed.Add(transform.GetChild(i).gameObject);

            for (var i = 0; i < objectsToBeDestroyed.Count; i++)
                DestroyImmediate(objectsToBeDestroyed[i]);
            objectsToBeDestroyed.Clear();
        }

        public void ClearTemporaryObjects()
        {
            var objectsToBeDestroyed = new List<GameObject>();
            for (var i = 0; i < temporaryObjectsGroup.childCount; i++)
                objectsToBeDestroyed.Add(temporaryObjectsGroup.GetChild(i).gameObject);

            for (var i = 0; i < objectsToBeDestroyed.Count; i++)
                DestroyImmediate(objectsToBeDestroyed[i]);
            objectsToBeDestroyed.Clear();
        }

        //Remove Grid Entity script from all the objects on the grid
        public void RemoveAllGridEntityScripts()
        {
            for(int groupIndex = 0; groupIndex < transform.childCount; groupIndex++)
            {
                var tr = transform.GetChild(groupIndex);
                for (var i = 0; i < tr.childCount; i++)
                {
                    var gridEntity = tr.GetChild(i).GetComponent<GridEntity>();
                    DestroyImmediate(gridEntity);
                }
            }
        }

        //Checks if the Grid Position has a Grid Entity on it, or not (optional group index deciding which group to get grid entity from: Tiles, Red, Blue, etc...)
        public bool IsThereGridEntityAtCursorGridPosition(int groupIndex = 0)
        {
            var tr = transform.GetChild(groupIndex);
            for (var i = 0; i < tr.childCount; i++)
            {
                var gridEntity = tr.GetChild(i).GetComponent<GridEntity>();
                if (gridEntity != null && gridEntity.GridPosition == CursorGridPosition) return true;
            }

            return false;
        }

        //Checks if the Grid Position has a Grid Entity on it, or not (optional group index deciding which group to get grid entity from: Tiles, Red, Blue, etc...)
        public bool IsThereGridEntityAtGridPositionParam(Vector2Int gridPosition, int groupIndex = 0)
        {
            var tr = transform.GetChild(groupIndex);
            for (var i = 0; i < transform.GetChild(groupIndex).childCount; i++)
            {
                var gridEntity = tr.GetChild(i).GetComponent<GridEntity>();
                if (gridEntity != null && gridEntity.GridPosition == gridPosition) return true;
            }

            return false;
        }

        //Checks if the Raw Position has a Grid Entity on it, or not (optional group index deciding which group to get grid entity from: Tiles, Red, Blue, etc...)
        public bool IsThereGridEntityAtRawPositionParam(Vector3 rawPosition, int groupIndex = 0)
        {
            var tr = transform.GetChild(groupIndex);
            for (var i = 0; i < tr.childCount; i++)
            {
                var gridEntity = tr.GetChild(i).GetComponent<GridEntity>();
                if (gridEntity != null && Vector3.Distance(gridEntity.transform.position, rawPosition) <= 0.01f)
                    return true;
            }

            return false;
        }

        //Get Grid Entity from Cursor Grid Position (optional group index deciding which group to get grid entity from: Tiles, Red, Blue, etc...)
        public GridEntity GetGridEntityFromCursorGridPosition(int groupIndex = 0)
        {
            var tr = transform.GetChild(groupIndex);
            for (var i = 0; i < tr.childCount; i++)
            {
                var gridEntity = tr.GetChild(i).GetComponent<GridEntity>();
                if (gridEntity != null && gridEntity.GridPosition == CursorGridPosition) return gridEntity;
            }

            return null;
        }
        
        //Get Grid Entity from Grid Position (optional group index deciding which group to get grid entity from: Tiles, Red, Blue, etc...)
        public GridEntity GetEntity(Vector2Int gridPosition, int groupIndex = 0)
        {
            var tr = transform.GetChild(groupIndex);
            for (var i = 0; i < tr.childCount; i++)
            {
                var gridEntity = tr.GetChild(i).GetComponent<GridEntity>();
                if (gridEntity != null && gridEntity.GridPosition == gridPosition)
                    return gridEntity;
            }

            return null;
        }

        //Get Grid Entity from Raw Position (optional group index deciding which group to get grid entity from: Tiles, Red, Blue, etc...)
        public GridEntity GetGridEntityFromRawPositionParam(Vector3 rawPosition, int groupIndex = 0)
        {
            var tr = transform.GetChild(groupIndex);
            for (var i = 0; i < tr.childCount; i++)
            {
                var gridEntity = tr.GetChild(i).GetComponent<GridEntity>();
                if (gridEntity != null && Vector3.Distance(gridEntity.transform.position, rawPosition) <= 0.01f)
                    return gridEntity;
            }

            return null;
        }
        
        public bool IsThereAnyGridEntityOnGrid(int groupIndex = 0)
        {
            return transform.GetChild(groupIndex).childCount > 0;
        }
        
        //Vector2Int to set Raw Position to Grid Position
        public Vector2Int ConvertRawPositionParamToGridPosition(Vector3 rawPosition)
        {
            return new Vector2Int((int)(rawPosition.x / (size.x + gap.x)), (int)(rawPosition.z / (size.z + gap.z)));
        }

        //Vector2Int to set Grid Position to Raw Position
        public Vector3 ConvertGridPositionParamToRawPosition(Vector2Int gridPosition)
        {
            return new Vector3(gridPosition.x * (size.x + gap.x), 0.0f, gridPosition.y * (size.z + gap.z));
        }
    }
}