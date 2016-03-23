using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
	
public class ScenePostProcess {
	[PostProcessSceneAttribute(-10)]
	public static void OnPostprocessScene() {
		if (Application.isPlaying)
			return;
		GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		cube.transform.position = new Vector3(0.0f, 0.5f, 0.0f);
	}
}