using UnityEditor;
using UnityEngine;

class CubemapsOnAll : CubemapWindow {

	[MenuItem("Tools/Create/Cubemaps On Everything")]
	static void CreateWizard() {
		ScriptableWizard.DisplayWizard("Generate Cubemaps on all ", typeof(CubemapsOnAll), "Generate");
	}

	void OnWizardCreate() {
		GenerateCubemaps.GenerateCubemapss(false, (int)Mathf.Pow(2, 4+(int)resolution), color, reflect);
	}

	void OnGUI() {
		GUILayout.Space(10);
		DrawWizardGUI();
		GUILayout.Space(10);
		if (GUILayout.Button("Generate")) OnWizardCreate();
		GUILayout.Space(10);
	}
}