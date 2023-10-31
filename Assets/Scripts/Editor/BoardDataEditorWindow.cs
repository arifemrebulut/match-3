using System;
using UnityEditor;
using UnityEngine;

namespace Agave
{
    public class BoardDataEditorWindow : EditorWindow
    {
        private Vector2 _dimensions;
        private bool[] _spawnerValues;

        private bool _drawBoard;
         
        private void OnGUI()
        {
            if (_drawBoard)
            {
                DrawBackButton();
                DrawBoard(36f, 1f, Color.gray);
            }
            else
            {
                DrawFields();
            }
        }

        private void DrawFields()
        {
            DrawLabel();
            DrawDimensionsHorizontalGroup();
            DrawCreateButton();
        }

        private void DrawLabel()
        {
            EditorGUILayout.Space();
            
            var labelStyle = new GUIStyle
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 18,
                normal =
                {
                    textColor = Color.white
                }
            };

            EditorGUILayout.LabelField("Board Dimensions", labelStyle);
            
            EditorGUILayout.Space();
        }

        private void DrawBackButton()
        {
            EditorGUILayout.Space(8);
            
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            if (GUILayout.Button("Back", GUILayout.Width(position.width / 3f)))
            {
                _drawBoard = false;
            }
            
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        private void DrawDimensionsHorizontalGroup()
        {
            EditorGUILayout.BeginHorizontal();

            var width = GUILayout.Width(100);
            
            GUILayout.FlexibleSpace();
            EditorGUIUtility.labelWidth = 58;
            _dimensions = EditorGUILayout.Vector2Field("", _dimensions, width);
            GUILayout.FlexibleSpace();
            
            EditorGUILayout.EndHorizontal();
        }

        private void DrawCreateButton()
        {
            EditorGUILayout.Space(8);
            
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            if (GUILayout.Button("Create Board", GUILayout.Width(position.width / 2f))
                && _dimensions.x != 0 && _dimensions.y != 0)
            {
                _drawBoard = true;
            }
            
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        
        private void DrawBoard(float gridSpacing, float gridOpacity, Color gridColor)
        {
            _spawnerValues ??= new bool[(int)_dimensions.x];
            
            var boardHalfHeight = (_dimensions.y * gridSpacing) / 2f;
            var boardHalfWidth = (_dimensions.x * gridSpacing) / 2f;

            var windowHalfHeight = position.height / 2f;
            var windowHalfWidth = position.width / 2f;
            
            var xLineStartPoint = windowHalfWidth - boardHalfWidth;
            var xLineEndPoint = windowHalfWidth + boardHalfWidth;
            
            var yLineStartPoint = windowHalfHeight - boardHalfHeight;
            var yLineEndPoint = windowHalfHeight + boardHalfHeight;
            
            var labelStyle = new GUIStyle
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 14,
                normal =
                {
                    textColor = Color.white
                }
            };

            GUILayout.Space(16);
            GUILayout.Label("Activate the spawner of the desired columns with toggles.", labelStyle);
            
            Handles.BeginGUI();
            
            Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);
            
            for (int i = 0; i < _dimensions.x + 1; i++)
            {
                if (i < _dimensions.x)
                {
                    var toggleRect = new Rect
                    {
                        x = (xLineStartPoint + gridSpacing * i) + gridSpacing / 2f - 7f,
                        y = yLineStartPoint - 24f,
                        width = 16f,
                        height = 16f,
                    };
                        
                    GUILayout.BeginArea(toggleRect);

                    _spawnerValues[i] = EditorGUILayout.Toggle("", _spawnerValues[i]);
                    
                    GUILayout.EndArea();
                }
                
                Handles.DrawLine(new Vector3( xLineStartPoint + gridSpacing * i, yLineStartPoint, 0), new Vector3(xLineStartPoint + gridSpacing * i, yLineEndPoint, 0f));
            }
            
            for (int j = 0; j < _dimensions.y + 1; j++)
            {
                Handles.DrawLine(new Vector3(xLineStartPoint, yLineStartPoint + gridSpacing * j, 0), new Vector3(xLineEndPoint,  yLineStartPoint + gridSpacing * j, 0f));
            }
            
            Handles.color = Color.white;
            
            Handles.EndGUI();
            
            var buttonAreaRect = new Rect
            {
                x = (position.width / 2f) - (position.width / 2f) / 2f,
                y = yLineStartPoint + gridSpacing * _dimensions.y + 32,
                width = position.width / 2f,
                height = 32f
            };
            
            GUILayout.BeginArea(buttonAreaRect);
            
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            
            if (GUILayout.Button("Save Board Data", GUILayout.Width(position.width / 2f)))
            {
                CreateAndEditBoardDataAsset();
                Close();
            }
            
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            
            GUILayout.EndArea();
        }
        
        private void CreateAndEditBoardDataAsset()
        {
            BoardDataSO boardData = CreateInstance<BoardDataSO>();
            
            Vector2Int dimensions = new Vector2Int((int)_dimensions.x, (int)_dimensions.y);
            boardData.dimensions = dimensions;
            boardData.spawnerColumnIndexes = _spawnerValues;

            var uniqueSuffix = Guid.NewGuid().ToString().Substring(0,3);
            string assetPath = $"Assets/Scriptables/BoardData/BoardData_{uniqueSuffix}.asset";
            AssetDatabase.CreateAsset(boardData, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}