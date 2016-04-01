using UnityEditor;
using UnityEngine;

class CubemapsOnAll : CubemapWindow {

	[MenuItem("Tools/Other/Generate Cubemaps on all")]
	static void CreateWizard() {
		ScriptableWizard.DisplayWizard("Generate Cubemaps on all ", typeof(CubemapsOnAll), "Generate");
	}
	void OnWizardCreate() {
		GenerateCubemaps.GenerateCubemapss(false, (int)Mathf.Pow(2, 4+(int)resolution), color, reflect);
	}
}