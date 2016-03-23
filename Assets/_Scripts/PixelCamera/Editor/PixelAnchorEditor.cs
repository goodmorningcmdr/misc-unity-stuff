/*---------------------------------------------------------------------------------
Editor for PixelAnchor

Author:	Bob Berkebile
Email:	bobb@pixelplacement.com
---------------------------------------------------------------------------------*/

using UnityEngine;
using UnityEditor;
using System.Collections;

[ CustomEditor( typeof( PixelAnchor ) ) ]
public class PixelAnchorEditor : Editor {
	
	//-----------------------------------------------------------------------------
	// Private Variables
	//-----------------------------------------------------------------------------
	
	PixelAnchor _target;
	
	//-----------------------------------------------------------------------------
	// Init
	//-----------------------------------------------------------------------------
	
	void Awake(){
		_target = (PixelAnchor)target;
	}
	
	//-----------------------------------------------------------------------------
	// InspectorGUI
	//-----------------------------------------------------------------------------
	
	public override void OnInspectorGUI(){
		_target.anchor = (TextAnchor)EditorGUILayout.EnumPopup( "Anchor", _target.anchor );
		_target.sizeFromScale = EditorGUILayout.Toggle( "Size from scale?", _target.sizeFromScale );
		if ( !_target.sizeFromScale ) {
			_target.manualSize = EditorGUILayout.Vector3Field( "Manual Size:", _target.manualSize );
		}
	}
}
