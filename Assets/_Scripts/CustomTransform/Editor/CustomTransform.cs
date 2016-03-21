using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues; //used for "Special Operations" fade group

[CustomEditor(typeof(Transform))]
[CanEditMultipleObjects]
public class CustomTransform : Editor
{
    private Transform _transform;
	float buttonWidth = 150;

    public override void OnInspectorGUI() {
        //We need this for all OnInspectorGUI sub methods
        _transform = (Transform)target;

        StandardTransformInspector();

        EditorGUILayout.Space();

        ShowLocalAxisComponentToggle();
    }

    private void StandardTransformInspector() {
		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Reset All", GUILayout.Width(buttonWidth)))
		{
			_transform.localPosition = Vector3.zero;
			_transform.localRotation = Quaternion.identity;
			_transform.localScale = Vector3.one;
		}
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginVertical();
		GUILayout.Space(2);
        _transform.localPosition = EditorGUILayout.Vector3Field(" Position", _transform.localPosition);
        _transform.localEulerAngles = EditorGUILayout.Vector3Field(" Rotation", _transform.localEulerAngles);
        _transform.localScale = EditorGUILayout.Vector3Field(" Scale", _transform.localScale);
		EditorGUILayout.EndVertical();
    }
	
    private bool showLocalAxisToggle = true;
    private ShowLocalAxis showLocalAxis;
    
	private void ShowLocalAxisComponentToggle() {    
        showLocalAxis = _transform.gameObject.GetComponent<ShowLocalAxis>();
        if (showLocalAxis == null)
            showLocalAxisToggle = false;
        else
            showLocalAxisToggle = true;

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Reset Position", GUILayout.Width(buttonWidth)))
		{
			_transform.localPosition = Vector3.zero;
		}

		if (GUILayout.Button("Reset Rotation", GUILayout.Width(buttonWidth)))
		{
			_transform.localRotation = Quaternion.identity;
		}
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Reset Scale", GUILayout.Width(buttonWidth)))
		{
			_transform.localScale = Vector3.one;
		}

		EditorGUI.BeginChangeCheck();

		if (GUILayout.Button("Toggle Handles", GUILayout.Width(buttonWidth))) 
		{
			showLocalAxisToggle = !showLocalAxisToggle; 
		}

        if (EditorGUI.EndChangeCheck())
        {
            if (showLocalAxisToggle == true)
            {
                showLocalAxis = _transform.gameObject.AddComponent<ShowLocalAxis>();
                int componentCount = _transform.GetComponents<Component>().Length;
                for (int i = 1; i < componentCount; i++)
                {
                    UnityEditorInternal.ComponentUtility.MoveComponentUp(showLocalAxis);
                }
            }
            else
            {
                showLocalAxis.destroyWhenSafe = true;
            }
        }
		GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
    }
}
