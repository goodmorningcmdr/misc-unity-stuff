using UnityEditor;
using UnityEngine;
class CubemapsOnSelected : CubemapWindow {
	
    [MenuItem ("Tools/Other/Generate Cubemaps")]
    static void CreateWizard () {
        ScriptableWizard.DisplayWizard("Generate Cubemaps on selected ", typeof (CubemapsOnSelected), "Generate");
    }

    void OnWizardCreate() {
        GenerateCubemaps.GenerateCubemapss(true, (int) Mathf.Pow(2,4+(int)resolution),color, reflect);
    }
}