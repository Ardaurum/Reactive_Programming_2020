using System.Reflection;
using _04_HealthViewer;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(PlayerCallback))]
    public sealed class PlayerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (Application.isPlaying)
            {
                var health = serializedObject.targetObject.GetType()
                    .GetProperty("Health", BindingFlags.Instance | BindingFlags.Public);
                health.SetValue(serializedObject.targetObject,
                    EditorGUILayout.Slider((float) health.GetValue(serializedObject.targetObject), .0f, 100.0f));
            }
        }
    }
}