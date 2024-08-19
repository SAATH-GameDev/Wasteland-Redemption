using UnityEditor;
using UnityEngine;

namespace Grids
{
    [CustomEditor(typeof(Grid))]
    public class GridEditor : Editor
    {
        private void OnSceneGUI()
        {
            HandleUtility.Repaint();

            var grid = (Grid)target;

            grid.SetCursorGridPositionToMousePosition(true);

            if(grid.guides && grid.guideAlpha > 0.0f)
            {
                DrawExtends(grid);

                DrawAdjacentTiles(grid);

                Handles.color = new Color(0.0f, 1.0f, 0.0f, grid.guideAlpha);
                Handles.DrawWireCube(Grid.CursorRawPosition, grid.size);
            }

            grid.UpdateCursorGridPosition();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Update"))
            {
                var grid = (Grid)target;
                Grid.Instance = grid;
                Grid.Instance.UpdateRawPositionsOfAllGridEntities();
            }
            else if (GUILayout.Button("Clear"))
            {
                var grid = (Grid)target;
                Grid.Instance = grid;
                Grid.Instance.DestroyAllGridEntities();
            }
            else if (GUILayout.Button("Remove Scripts"))
            {
                var grid = (Grid)target;
                Grid.Instance = grid;
                Grid.Instance.RemoveAllGridEntityScripts();
            }
        }

        private void DrawAdjacentTiles(Grid grid)
        {
            for (var z = -grid.adjacentCubeDisplayRange; z <= grid.adjacentCubeDisplayRange; z++)
            for (var y = -grid.adjacentCubeDisplayRange; y <= grid.adjacentCubeDisplayRange; y++)
            for (var x = -grid.adjacentCubeDisplayRange; x <= grid.adjacentCubeDisplayRange; x++)
            {
                var tileDistance = Mathf.Abs(x) + Mathf.Abs(y) + Mathf.Abs(z);
                if (tileDistance > grid.adjacentCubeDisplayRange || tileDistance <= 0) continue;
                Handles.color = Color.Lerp(Color.black, Color.clear, tileDistance / (float)grid.adjacentCubeDisplayRange);
                Handles.color = Color.Lerp(Handles.color, Color.clear, 1.0f - grid.guideAlpha);
                Handles.DrawWireCube(Grid.CursorRawPosition
                                    + x * (Vector3.right * Mathf.Abs(grid.normal.x - 1.0f)) * (grid.size.x + grid.gap.x)
                                    + y * (Vector3.up * Mathf.Abs(grid.normal.y - 1.0f)) * (grid.size.y + grid.gap.y)
                                    + z * (Vector3.forward * Mathf.Abs(grid.normal.z - 1.0f)) * (grid.size.z + grid.gap.z),
                    grid.size);
            }
        }

        private void DrawExtends(Grid grid)
        {
            //ONLY COVERS Y AXIS NORMAL
            Vector3[] p =
            {
                new(
                    0.0f,
                    0.0f,
                    0.0f
                ),
                new(
                    grid.extends.x * (grid.size.x + grid.gap.x),
                    0.0f,
                    0.0f
                ),
                new(
                    grid.extends.x * (grid.size.x + grid.gap.x),
                    0.0f,
                    grid.extends.z * (grid.size.z + grid.gap.z)
                ),
                new(
                    0.0f,
                    0.0f,
                    grid.extends.z * (grid.size.z + grid.gap.z)
                ),
                new(
                    0.0f,
                    0.0f,
                    0.0f
                )
            };

            Handles.color = Color.black;
            Handles.color = Color.Lerp(Handles.color, Color.clear, 1.0f - grid.guideAlpha);
            for (var i = 1; i < p.Length; i++)
                Handles.DrawDottedLine(p[i - 1], p[i], 4.0f);
        }
    }
}