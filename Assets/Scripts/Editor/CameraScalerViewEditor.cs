using UnityEditor;
using UnityEngine;
using Views;

namespace Editor
{
    [CustomEditor(typeof(CameraScalerView))]
    public class CameraScalerViewEditor : UnityEditor.Editor
    {
        private CameraScalerView _view;
        
        private void OnEnable()
        {
            _view = target as CameraScalerView;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            EditorGUILayout.Space();

            DrawScaleButton();
        }
        
        private void DrawScaleButton()
        {
            if (GUILayout.Button("Scale"))
            {
                var editorResolution = UnityStats.screenRes.Split('x');

                var resolution = new Vector2(float.Parse(editorResolution[0]), float.Parse(editorResolution[1]));
                
                _view.Scale(resolution);
            }
        }
    }
}