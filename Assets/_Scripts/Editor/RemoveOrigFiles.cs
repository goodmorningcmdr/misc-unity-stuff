using UnityEngine;
using UnityEditor;

public class RemoveOrigFiles : Editor {
	[MenuItem("Tools/Other/Remove orig files")] //generated from sourcetree
	static void RemoveEm() {
		Debug.Log("Moving all .orig files to trash");
		string[] guids = AssetDatabase.FindAssets("");
		foreach (string guid in guids)
		{
			if (AssetDatabase.GUIDToAssetPath(guid).Contains(".orig"))
			{
				AssetDatabase.MoveAssetToTrash(AssetDatabase.GUIDToAssetPath(guid));
			}
		}
	}
}