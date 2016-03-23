using UnityEngine;
using UnityEditor;

public class PackageExporter {
	[MenuItem("Tools/Other/Export Project as Unity Package")]
	static void CreateUnityPackage() {
	var assetPathNames = new string[] { 
		"Assets",
		"ProjectSettings/TagManager.asset",
		"ProjectSettings/AudioManager.asset",
		"ProjectSettings/ClusterInputManager.asset",
		"ProjectSettings/GraphicsSettings.asset",
		"ProjectSettings/InputManager.asset",
		"ProjectSettings/ProjectSettings.asset",
		"ProjectSettings/QualitySettings.asset",
		"ProjectSettings/TagManager.asset",
		"ProjectSettings/TimeManager.asset"
	};
	AssetDatabase.ExportPackage(assetPathNames, Application.productName + ".unitypackage", ExportPackageOptions.Interactive | ExportPackageOptions.Recurse |ExportPackageOptions.IncludeDependencies);
	}
}