using UnityEditor;
using UnityEngine;
class CubemapsOnSelected : CubemapWindow {

	[MenuItem("Tools/Create/Cubemaps On Selected", false)]
    static void CreateWizard() {
        ScriptableWizard.DisplayWizard("Generate Cubemaps on selected ", typeof(CubemapsOnSelected), "Generate", "Cancel");
    }

	[MenuItem("Tools/Create/Cubemaps On Selected", true)]
	static bool ValidateCreateWizard() {
		return Selection.activeTransform != null;
	}

	void OnGUI() {
		if (Selection.activeTransform == null)
			EditorGUILayout.HelpBox("Nothing is selected", MessageType.Warning);

		isValid = Selection.activeTransform ? true : false;

		GUILayout.Space(10);
		DrawWizardGUI();
		GUILayout.Space(10);
		if (GUILayout.Button("Generate")) OnWizardCreate();
		GUILayout.Space(10);
	}

    void OnWizardCreate() {
		if (Selection.activeTransform == null) return;
        GenerateCubemaps.GenerateCubemapss(true, (int)Mathf.Pow(2, 4 + (int)resolution), color, reflect);
    }
}