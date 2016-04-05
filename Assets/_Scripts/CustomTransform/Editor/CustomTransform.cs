using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues; //used for "Special Operations" fade group

[CustomEditor(typeof(Transform))]
[CanEditMultipleObjects]
public class CustomTransform : Editor {
    private Transform _transform;
	
    public override void OnInspectorGUI() {
		if (Selection.transforms.Length > 1)
		{
			DrawDefaultInspector();
			return;
		}
        //We need this for all OnInspectorGUI sub methods
        _transform = (Transform)target;

        StandardTransformInspector();

        ShowLocalAxisComponentToggle();
    }

    private void StandardTransformInspector() {
		EditorGUILayout.BeginVertical();
        _transform.localPosition = EditorGUILayout.Vector3Field(" Position", _transform.localPosition);
        _transform.localEulerAngles = EditorGUILayout.Vector3Field(" Rotation", _transform.localEulerAngles);
        _transform.localScale = EditorGUILayout.Vector3Field(" Scale", _transform.localScale);
		EditorGUILayout.EndVertical();
		Undo.RecordObject(target, "Transform value changed.");
    }
	
	void OnInspectorUpdate() {
		Repaint();
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

		if (GUILayout.Button(" P ")) _transform.localPosition = (Event.current.modifiers == EventModifiers.Control) ? Random.insideUnitSphere * 10 : Vector3.zero;
		if (GUILayout.Button(" R ")) _transform.eulerAngles = (Event.current.modifiers == EventModifiers.Control) ? new Vector3(Mathf.Round(_transform.eulerAngles.x), Mathf.Round(_transform.eulerAngles.y), Mathf.Round(_transform.eulerAngles.z)) : Vector3.zero;
		if (GUILayout.Button(" S ")) _transform.localScale = (Event.current.modifiers == EventModifiers.Control) ? (Vector3.one * (Random.Range(1, 11))) : Vector3.one;

		if (GUILayout.Button(" All "))
		{
			_transform.localPosition = Vector3.zero;
			_transform.localRotation = Quaternion.identity;
			_transform.localScale = Vector3.one;
		}

		if (_transform.childCount > 0 && GUILayout.Button(" Children "))
		{
			foreach (Transform child in _transform.GetComponentsInChildren<Transform>())
			{
				child.localPosition = Vector3.zero;
				child.localRotation = Quaternion.identity;
				child.localScale = Vector3.one;
			}
		}

		EditorGUI.BeginChangeCheck();
		
		if (GUILayout.Button(" Handles ", GUILayout.ExpandWidth(false))) 
		{
			showLocalAxisToggle = !showLocalAxisToggle;
		}

		if (EditorGUI.EndChangeCheck())
        {
			if (showLocalAxisToggle)
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

		EditorGUILayout.EndHorizontal();
    }

	[MenuItem("Tools/Other/Round Rotations")]
	static void RoundRotations() {
		foreach(GameObject go in GameObject.FindGameObjectsWithTag("Untagged"))
		{
			if (go.scene == UnityEngine.SceneManagement.SceneManager.GetActiveScene())
			{
				go.transform.eulerAngles = new Vector3(Mathf.Round(go.transform.eulerAngles.x), Mathf.Round(go.transform.eulerAngles.y), Mathf.Round(go.transform.eulerAngles.z));
			}
		}
		UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
	}
}