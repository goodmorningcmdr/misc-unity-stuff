using UnityEngine;
using UnityEditor;

public class AlignObjects : EditorWindow {
	bool alignToX = false;
	bool alignToY = false;
	bool alignToZ = false;
	string selected = "";
	string alignTo = "";
	public Transform[] transforms;

	[MenuItem("Tools/Align Objects")]
	static void Init() {
		Selection.activeTransform = null;

		EditorWindow window = GetWindow(typeof(AlignObjects), true);
		//window.titleContent.text = "Align Transforms   ";
		//window.titleContent.
		window.titleContent = new GUIContent("Align Transforms");
		window.Show();
	}
	
	void OnInspectorUpdate() {
		Repaint();
	}
	
	void OnGUI() {
		EditorGUILayout.Space();

		if (Selection.activeTransform == null)
		{
			EditorGUILayout.HelpBox("Select the transform you want to align other transforms to", MessageType.Info);
			return;
		}
		else if (Selection.transforms.Length <= 1)
		{
			EditorGUILayout.HelpBox("Select the transforms you want to align", MessageType.Info);
			return;
		}

		selected = Selection.activeTransform ? Selection.activeTransform.name : "";
		transforms = Selection.transforms;

		foreach (Transform t in Selection.transforms)
		{
			if (t.GetInstanceID() != Selection.activeTransform.GetInstanceID()) 
			{
				alignTo += t.name;
				if (t.GetInstanceID() != Selection.transforms[Selection.transforms.Length - 1].GetInstanceID()) alignTo += ", ";
			}
		}
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("  Align these transforms:", EditorStyles.boldLabel);
		GUILayout.FlexibleSpace();
		GUILayout.Label(alignTo + "  ");
		EditorGUILayout.EndHorizontal();
		GUILayout.Space(10);
		alignTo = "";
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("  to this transform:", EditorStyles.boldLabel);
		GUILayout.FlexibleSpace();
		GUILayout.Label(selected + "  ");
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space(); EditorGUILayout.Space();
		GUILayout.Label("  On: ", EditorStyles.boldLabel);
		alignToX = EditorGUILayout.Toggle("  X axis", alignToX);
		alignToY = EditorGUILayout.Toggle("  Y axis", alignToY);
		alignToZ = EditorGUILayout.Toggle("  Z axis", alignToZ);
		GUILayout.Space(20);
		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("Align", GUILayout.Width(200), GUILayout.Height(25))) Align();
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
	}
	Vector3 newPosition;
	Vector3 alignementPosition;

	public void Align() {
		//if (selected == "" || alignTo == "") 
			//Debug.Log("No objects selected to align");
		//else
		//{
			foreach (Transform t in Selection.transforms) {
				if (t.GetInstanceID() != Selection.activeInstanceID)
				{
					alignementPosition = Selection.activeTransform.position;
					newPosition.x = alignToX ? alignementPosition.x : t.position.x;
					newPosition.y = alignToY ? alignementPosition.y : t.position.y;
					newPosition.z = alignToZ ? alignementPosition.z : t.position.z;
					t.position = newPosition;
				}
			}
		//}
	}
}