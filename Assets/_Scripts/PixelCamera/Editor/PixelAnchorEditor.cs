using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(PixelAnchor))]
public class PixelAnchorEditor : Editor {
	
	PixelAnchor _target;
	
	void Awake() {
		_target = (PixelAnchor)target;
	}
	
	public override void OnInspectorGUI() {
		_target.anchor = (TextAnchor)EditorGUILayout.EnumPopup("Anchor", _target.anchor);
		_target.sizeFromScale = EditorGUILayout.Toggle("Size from scale?", _target.sizeFromScale);

		if (!_target.sizeFromScale) 
		{
			_target.manualSize = EditorGUILayout.Vector3Field("Manual Size: ", _target.manualSize);
		}
	}
}
