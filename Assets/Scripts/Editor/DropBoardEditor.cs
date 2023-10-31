using UnityEngine;
using UnityEditor;

namespace Agave
{
    [CustomEditor(typeof(DropBoard))]
    public class DropBoardEditor : Editor
    {
        private DropBoard _dropBoard;
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            _dropBoard = target as DropBoard;
            
            EditorGUILayout.Separator();
            
            if (GUILayout.Button("Create New Board Data"))
            {
                EditorWindow.CreateWindow<BoardDataEditorWindow>("Board Data Editor");
            }
        }
    }
}