using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShowLocalAxis))]
public class ShowLocalAxisInspector : Editor {
    public override void OnInspectorGUI() {
        // base.DrawDefaultInspector();
		EditorGUILayout.PropertyField(serializedObject.FindProperty("handleLength"));
        ShowLocalAxis showRotated = (ShowLocalAxis)target;
        if (showRotated.destroyWhenSafe && Event.current.type == EventType.Repaint)
        {
            DestroyImmediate(showRotated, true);
            return;
        }
		serializedObject.ApplyModifiedProperties();
    }
}