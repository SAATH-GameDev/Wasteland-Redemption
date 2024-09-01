using UnityEditor;
using UnityEngine;

namespace Grids
{
    [CustomEditor(typeof(Palette))]
    public class PaletteEditor : Editor
    {
        private string replacementString = " Painter";

        private int activePainterIndex = -1;
        private int activeObjectIndex = -1;

        private GameObject displayObject = null;

        public void SetupDisplayObject(ref Palette palette)
        {
            if(palette.activePainter != activePainterIndex || palette.index != activeObjectIndex)
            {
                Grid.Instance.ClearTemporaryObjects();

                activePainterIndex = palette.activePainter;
                activeObjectIndex = palette.index;
            }

            if(!displayObject)
                displayObject = Instantiate(palette.Get(), Grid.Instance.temporaryObjectsGroup);

            GridEntity displayGridEntity = displayObject.GetComponent<GridEntity>();

            if(!displayGridEntity)
            {
                displayGridEntity = displayObject.AddComponent<GridEntity>();
                displayGridEntity.prefab = null;
            }
            else
            {
                displayGridEntity.prefab = palette.Get();
            }

            displayGridEntity.GridPosition = Grid.CursorGridPosition;
            displayGridEntity.offset = palette.Painter.offset;
            displayGridEntity.UpdatePosition();

            UpdateDisplayObject(ref palette);
        }

        public void UpdateDisplayObject(ref Palette palette)
        {
            if(!displayObject)
            {
                SetupDisplayObject(ref palette);
                return;
            }

            displayObject.transform.rotation = Quaternion.Euler(0.0f, palette.rotationYOffset, 0.0f);
        }

        public void OnSceneGUI()
        {
            HandleUtility.Repaint();

            var palette = (Palette)target;
            SetupDisplayObject(ref palette);

            if(palette.randomYRotation)
                palette.rotationYOffset = Random.Range(0, 4) * 90.0f;

            if (palette.Painter != null)
            {
                var e = Event.current;
                var controlID = GUIUtility.GetControlID(FocusType.Passive);
                switch (e.GetTypeForControl(controlID))
                {
                    case EventType.MouseDown:
                        if (e.button == 0)
                        {
                            GUIUtility.hotControl = controlID;

                            var gridEntity = displayObject.GetComponent<GridEntity>();
                            if (gridEntity != null)
                            {
                                gridEntity.Paint(Grid.CursorGridPosition, palette.Painter.offset,
                                    Quaternion.Euler(0.0f, palette.rotationYOffset, 0.0f),
                                    GetGroupTransform(palette), palette.paintMode);
                            }

                            e.Use();
                        }

                        break;

                    case EventType.MouseUp:
                        GUIUtility.hotControl = 0;
                        e.Use();
                        break;

                    case EventType.MouseDrag:
                        if (e.button == 0)
                        {
                            GUIUtility.hotControl = controlID;

                            var gridEntity = displayObject.GetComponent<GridEntity>();
                            if (gridEntity != null)
                            {
                                gridEntity.Paint(Grid.CursorGridPosition, palette.Painter.offset,
                                    Quaternion.Euler(0.0f, palette.rotationYOffset, 0.0f),
                                        GetGroupTransform(palette), palette.paintMode);
                            }

                            e.Use();
                        }

                        break;

                    case EventType.KeyDown:
                        if (e.keyCode == KeyCode.M)
                        {
                            GUIUtility.hotControl = controlID;
                            palette.paintMode = palette.paintMode == PaintMode.DELETE_PAINT
                                ? PaintMode.PAINT
                                : palette.paintMode + 1;
                            e.Use();
                        }
                        else if (e.keyCode == KeyCode.Z)
                        {
                            GUIUtility.hotControl = controlID;
                            if (--palette.index < 0) palette.index = palette.Painter.objects.Count - 1;
                            e.Use();
                        }
                        else if (e.keyCode == KeyCode.X)
                        {
                            GUIUtility.hotControl = controlID;
                            if (++palette.index >= palette.Painter.objects.Count) palette.index = 0;
                            e.Use();
                        }
                        else if (e.keyCode == KeyCode.BackQuote
                                || e.keyCode == KeyCode.Tilde
                                || e.keyCode == KeyCode.Backslash)
                        {
                            GUIUtility.hotControl = controlID;
                            if (++palette.activePainter >= palette.group.painters.Count) palette.activePainter = 0;
                            e.Use();
                        }
                        else if (e.keyCode == KeyCode.LeftBracket || e.keyCode == KeyCode.LeftCurlyBracket)
                        {
                            GUIUtility.hotControl = controlID;
                            palette.rotationYOffset -= 90;
                            UpdateDisplayObject(ref palette);
                            e.Use();
                        }
                        else if (e.keyCode == KeyCode.RightBracket || e.keyCode == KeyCode.RightCurlyBracket)
                        {
                            GUIUtility.hotControl = controlID;
                            palette.rotationYOffset += 90;
                            UpdateDisplayObject(ref palette);
                            e.Use();
                        }
                        else if (e.keyCode != KeyCode.B)
                        {
                            GUIUtility.hotControl = controlID;
                            e.Use();
                        }

                        break;

                    case EventType.KeyUp:
                        if (e.keyCode == KeyCode.B)
                        {
                            GUIUtility.hotControl = controlID;
                            if (palette.boxGridPosition.x == -9999)
                            {
                                palette.boxGridPosition = Grid.CursorGridPosition;
                            }
                            else
                            {
                                var gridEntity = displayObject.GetComponent<GridEntity>();
                                if (gridEntity != null && palette.boxGridPosition.x != -9999)
                                {
                                    var yMin = palette.boxGridPosition.y <= Grid.CursorGridPosition.y ? palette.boxGridPosition.y : Grid.CursorGridPosition.y;
                                    var yMax = palette.boxGridPosition.y > Grid.CursorGridPosition.y ? palette.boxGridPosition.y : Grid.CursorGridPosition.y;
                                    var xMin = palette.boxGridPosition.x <= Grid.CursorGridPosition.x ? palette.boxGridPosition.x : Grid.CursorGridPosition.x;
                                    var xMax = palette.boxGridPosition.x > Grid.CursorGridPosition.x ? palette.boxGridPosition.x : Grid.CursorGridPosition.x;

                                    for (var y = yMin; y <= yMax; y++)
                                        for (var x = xMin; x <= xMax; x++)
                                        {
                                            gridEntity.Paint(new Vector2Int(x, y), palette.Painter.offset,
                                                Quaternion.Euler(0.0f, palette.rotationYOffset, 0.0f),
                                                GetGroupTransform(palette), palette.paintMode);
                                        }

                                    palette.boxGridPosition = Vector2Int.one * -9999;
                                }
                            }
                            e.Use();
                        }

                        break;
                }

                if (palette.boxGridPosition.x != -9999)
                {
                    Handles.color = Color.yellow;
                    var boxGridPosition = new Vector3(palette.boxGridPosition.x * (Grid.Instance.size.x + Grid.Instance.gap.x),
                        0.0f, palette.boxGridPosition.y * (Grid.Instance.size.z + Grid.Instance.gap.z));
                    var currentRawPosition = new Vector3(Grid.CursorGridPosition.x * (Grid.Instance.size.x + Grid.Instance.gap.x), 0.0f,
                        Grid.CursorGridPosition.y * (Grid.Instance.size.z + Grid.Instance.gap.z));
                    Handles.DrawWireCube(Vector3.Lerp(boxGridPosition, currentRawPosition, 0.5f), new Vector3(
                        Mathf.Abs(boxGridPosition.x - currentRawPosition.x) + Grid.Instance.size.x,
                        Mathf.Abs(boxGridPosition.y - currentRawPosition.y) + Grid.Instance.size.y,
                        Mathf.Abs(boxGridPosition.z - currentRawPosition.z) + Grid.Instance.size.z
                    ));
                }
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var palette = (Palette)target;

            var originalBGColor = GUI.backgroundColor;
            var originalFontColor = GUI.contentColor;

            GUILayout.BeginHorizontal();
            GUI.backgroundColor = palette.paintMode == PaintMode.PAINT ? Color.black : originalBGColor;
            if (GUILayout.Button("PAINT", GUILayout.Height(40.0f)))
                palette.paintMode = PaintMode.PAINT;
            GUI.backgroundColor = palette.paintMode == PaintMode.REPLACE ? Color.black : originalBGColor;
            if (GUILayout.Button("REPLACE", GUILayout.Height(40.0f)))
                palette.paintMode = PaintMode.REPLACE;
            GUI.backgroundColor = palette.paintMode == PaintMode.DELETE ? Color.black : originalBGColor;
            if (GUILayout.Button("DELETE", GUILayout.Height(40.0f)))
                palette.paintMode = PaintMode.DELETE;
            GUI.backgroundColor = palette.paintMode == PaintMode.DELETE_PAINT ? Color.black : originalBGColor;
            if (GUILayout.Button("DEL. PAINT", GUILayout.Height(40.0f)))
                palette.paintMode = PaintMode.DELETE_PAINT;
            GUILayout.EndHorizontal();

            GUILayout.Space(8.0f);

            if (palette.group != null)
            {
                GUILayout.BeginHorizontal();
                for (var i = 0; i < palette.group.painters.Count; i++)
                {
                    GUI.backgroundColor = i == palette.activePainter
                        ? Color.Lerp(Color.black, Color.green, 0.5f)
                        : originalBGColor;
                    if (GUILayout.Button(palette.group.painters[i].name.Replace(replacementString, ""), GUILayout.Height(30.0f)))
                    {
                        palette.activePainter = i;
                        break;
                    }
                }

                GUILayout.EndHorizontal();
            }

            if (palette.Painter != null)
            {
                if (palette.index >= palette.Painter.objects.Count) palette.index = 0;

                for (var i = 0; i < palette.Painter.objects.Count; i++)
                {
                    if (i == palette.index)
                    {
                        GUI.backgroundColor = Color.black;
                        GUI.contentColor = Color.green;
                    }
                    else
                    {
                        GUI.backgroundColor = Color.gray;
                        GUI.contentColor = originalFontColor;
                    }

                    if (GUILayout.Button(palette.Painter.objects[i].name, GUILayout.Height(20.0f)))
                    {
                        palette.index = i;
                        break;
                    }
                }
            }

            GUI.backgroundColor = originalBGColor;
            GUI.contentColor = originalFontColor;

            GUILayout.Space(8.0f);

            GUILayout.Label("Press B for Box Select");
            GUILayout.Label("Press M to change Paint Mode");
            GUILayout.Label("Press / or ` to change Painter");
            GUILayout.Label("Press Z or X to change Object");
            GUILayout.Label("Press [ or ] to change rotation offset");
        }

        public Transform GetGroupTransform(Palette palette)
        {
            var groupName = palette.Painter.name.Replace(replacementString, "");
            for (var i = 0; i < palette.transform.childCount; i++)
                if (palette.transform.GetChild(i).name == groupName)
                    return palette.transform.GetChild(i).gameObject.transform;

            Transform groupTransform = null;
            for (var g = 0; g < palette.group.painters.Count; g++)
            {
                var newGroupObject = new GameObject(palette.group.painters[g].name.Replace(replacementString, ""));
                newGroupObject.transform.parent = palette.transform;

                if (newGroupObject.name == groupName) groupTransform = newGroupObject.transform;
            }

            return groupTransform;
        }
    }
}