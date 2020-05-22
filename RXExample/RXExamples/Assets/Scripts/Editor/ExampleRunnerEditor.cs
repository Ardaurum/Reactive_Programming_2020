using Runner;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(ExampleRunner))]
    public sealed class ExampleRunnerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (Application.isPlaying && GUILayout.Button("Run"))
            {
                ((ExampleRunner) serializedObject.targetObject).Run();
            }
        }
    }
}